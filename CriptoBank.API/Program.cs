
using System;
using System.Text;
using CriptoBank.API.Middlewares;
using CriptoBank.Application.AutoMapperProfile;
using CriptoBank.Application.Interfaces.BuyService;
using CriptoBank.Application.Interfaces.CoinService;
using CriptoBank.Application.Interfaces.HoldingService;
using CriptoBank.Application.Interfaces.ReportService;
using CriptoBank.Application.Interfaces.Token;
using CriptoBank.Application.Interfaces.TransactionService;
using CriptoBank.Application.Interfaces.UnitOfWork;
using CriptoBank.Application.Repositories;
using CriptoBank.Application.Repositories.Token;
using CriptoBank.Application.Services;
using CriptoBank.Domain.Repositories;
using CriptoBank.Infrastructure.Context;
using CriptoBank.Infrastructure.Repositories;
using CriptoBank.Infrastructure.Repositories.Security;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CriptoBank.API
{
    public class Program
    {        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
              c.SwaggerDoc("v1", new OpenApiInfo { Title = "CriptoBank API", Version = "v1" });

              c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
              {
                 Description = @"JWT Authorization header usando o esquema Bearer. 
                              Entre com 'Bearer' [espaço] e então seu token no campo abaixo.
                              Exemplo: 'Bearer 12345abcdef'",
                 Name = "Authorization",
                 In = ParameterLocation.Header,
                 Type = SecuritySchemeType.ApiKey,
                 Scheme = "Bearer"
              });

              c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
            });
            builder.Services.AddMemoryCache();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserTokenRepositories, UserTokenRepositories>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddHttpClient<ICoinService, CoinService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<IWalletHistoryRepository, WalletHistoryRepository>();
            builder.Services.AddScoped<IHoldingRepository, HoldingRepository>();
            builder.Services.AddScoped<IHoldingService, HoldingService>();
            builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            builder.Services.AddScoped<ICryptoTransactionService, CryptoTransactionService>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<ICryptoRepository, CryptoRepository>();

            builder.Services.AddAutoMapper(typeof(UserProfile));
            var myhandlers = AppDomain.CurrentDomain.Load("CriptoBank.Application");
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(myhandlers));

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddMassTransit(x =>
            {

                x.SetInMemorySagaRepositoryProvider();

                x.UsingRabbitMq((context, cfg) =>
                {

                    var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq";

                    cfg.Host(rabbitHost, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });


            builder.Services.AddDbContext<CriptoDbContext>(options =>
                    options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                var context = services.GetRequiredService<CriptoDbContext>();

                int retries = 10; 
                while (retries > 0)
                {
                    try
                    {
                        
                        if (context.Database.GetPendingMigrations().Any())
                        {
                            context.Database.Migrate();
                            Console.WriteLine("✅ Migrations aplicadas com sucesso!");
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        retries--;
                        if (retries == 0)
                        {
                            Console.WriteLine("❌ Erro fatal: Não foi possível conectar ao banco após várias tentativas.");
                            throw;
                        }
                        Console.WriteLine("⏳ Banco de dados ainda não está pronto... tentando novamente em 5s.");
                        Thread.Sleep(5000);
                    }
                }
            }
            app.Run();
        }
    }
}

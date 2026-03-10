
using CriptoBank.Application.Repositories.Token;
using CriptoBank.Application.Repositories;
using CriptoBank.Infrastructure.Context;
using CriptoBank.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using CriptoBank.Application.Interfaces.Token;
using CriptoBank.Application.AutoMapperProfile;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CriptoBank.Application.Interfaces.UnitOfWork;
using CriptoBank.Application.Interfaces.CoinService;
using CriptoBank.Application.Services;
using CriptoBank.API.Middlewares;
using CriptoBank.Application.Interfaces.BuyService;
using CriptoBank.Domain.Repositories;
using CriptoBank.Infrastructure.Repositories.Security;
using CriptoBank.Application.Interfaces.HoldingService;
using CriptoBank.Application.Interfaces.TransactionService;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserTokenRepositories, UserTokenRepositories>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddHttpClient<ICoinService, CoinService>();
            builder.Services.AddHttpContextAccessor();

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

            app.Run();
        }
    }
}

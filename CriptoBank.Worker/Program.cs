using MassTransit;
using Microsoft.EntityFrameworkCore;
using CriptoBank.Worker.Consumers;
using CriptoBank.Infrastructure.Context;
using CriptoBank.Infrastructure.Repositories;
using CriptoBank.Application.Interfaces.ReportService;
using CriptoBank.Domain.Repositories;
using QuestPDF.Infrastructure;
using CriptoBank.Infrastructure.Repositories.Security;


QuestPDF.Settings.License = LicenseType.Community;

var builder = Host.CreateApplicationBuilder(args);


Console.WriteLine($"[DEBUG] Worker iniciado em: {AppContext.BaseDirectory}");

builder.Services.AddDbContext<CriptoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IReportService, ReportService>();


builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<GenerateReportConsumer>();
    x.AddConsumer<GenerateAndSendEmailReportConsumer>();
    x.AddConsumer<SendEmailConsumer>(); 

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/");


        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();

Console.WriteLine(">>> Worker CriptoBank aguardando mensagens no RabbitMQ...");

await host.RunAsync();
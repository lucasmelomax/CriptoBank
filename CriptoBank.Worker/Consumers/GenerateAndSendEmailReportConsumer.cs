using CriptoBank.Application.DTOs.Message;
using CriptoBank.Application.DTOs.Transaction;
using CriptoBank.Application.Interfaces.ReportService;
using CriptoBank.Domain.Repositories;
using CriptoBank.Worker.Consumers;
using MassTransit;

public class GenerateAndSendEmailReportConsumer : IConsumer<EmailGenerateReportMessage>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly IReportService _reportService;
    private readonly ILogger<GenerateReportConsumer> _logger;

    public GenerateAndSendEmailReportConsumer(ITransactionRepository transactionRepository, IPortfolioRepository portfolioRepository, IReportService reportService, ILogger<GenerateReportConsumer> logger)
    {
        _transactionRepository = transactionRepository;
        _portfolioRepository = portfolioRepository;
        _reportService = reportService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmailGenerateReportMessage> context)
    {

        var msg = context.Message;
        _logger.LogInformation("Recebido pedido de PDF para: {Email}", msg.UserEmail);

        var portfolio = await _portfolioRepository.GetByUserIdAsync(msg.UserId);
        if (portfolio == null) throw new Exception("Usuário não possui um portfólio configurado.");

        var transactions = await _transactionRepository.GetByPortfolioIdAsync(portfolio.Id);

        var transactionsDto = transactions.Select(t => new TransactionReportDTO(
            t.CreatedAt,
            t.Type.ToString(),
            t.Crypto.Name,
            t.Quantity,
            t.TotalValue
        )).ToList();

        var pdfBytes = _reportService.GenerateTransactionReport(msg.UserEmail, transactionsDto);

        var fileNameDisplay = $"Extrato_{DateTime.Now:yyyyMMdd}.pdf";

        await context.Publish(new SendEmailMessage
        {
            ToEmail = msg.UserEmail,
            Subject = "Seu Extrato Chegou!",
            Body = "Olá, seu PDF está em anexo.",
            AttachmentPath = fileNameDisplay,
            AttachmentBytes = pdfBytes 
        });

        _logger.LogInformation("PDF gerado e enviado para a fila de e-mail.");
    }
}
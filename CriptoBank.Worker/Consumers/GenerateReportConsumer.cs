using CriptoBank.Application.DTOs.Message;
using CriptoBank.Application.DTOs.Transaction;
using CriptoBank.Application.Interfaces.ReportService;
using CriptoBank.Domain.Repositories;
using MassTransit;

namespace CriptoBank.Worker.Consumers
{
    public class GenerateReportConsumer : IConsumer<GenerateReportMessage>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IReportService _reportService;
        private readonly ILogger<GenerateReportConsumer> _logger;

        public GenerateReportConsumer(ITransactionRepository transactionRepository, IPortfolioRepository portfolioRepository, IReportService reportService, ILogger<GenerateReportConsumer> logger)
        {
            _transactionRepository = transactionRepository;
            _portfolioRepository = portfolioRepository;
            _reportService = reportService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<GenerateReportMessage> context)
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

            var path = @"C:\CriptoBank\Relatorios";
            Directory.CreateDirectory(path);
            var fileName = Path.Combine(path, $"Extrato_{DateTime.Now:yyyyMMddHH}.pdf");

            await File.WriteAllBytesAsync(fileName, pdfBytes);

            _logger.LogInformation("PDF salvo em: {Path}", fileName);
        }
    }
}

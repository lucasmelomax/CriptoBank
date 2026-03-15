

using CriptoBank.Application.DTOs.Transaction;
using CriptoBank.Application.Interfaces.ReportService;
using CriptoBank.Application.Interfaces.Token;
using CriptoBank.Domain.Repositories;
using MediatR;

namespace CriptoBank.Application.Handlers.Transactions
{
    internal class GetTransactionReportQuerieHandler : IRequestHandler<GetTransactionReportQuerie, byte[]>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IReportService _reportService;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetTransactionReportQuerieHandler(ITransactionRepository transactionRepository, IReportService reportService, IPortfolioRepository portfolioRepository, ICurrentUserService currentUserService)
        {
            _transactionRepository = transactionRepository;
            _reportService = reportService;
            _portfolioRepository = portfolioRepository;
            _currentUserService = currentUserService;
        }

        public async Task<byte[]> Handle(GetTransactionReportQuerie request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
            var userEmail = _currentUserService.Email ?? "Usuário CriptoBank";

            var portfolio = await _portfolioRepository.GetByUserIdAsync(userId);
            if (portfolio == null) throw new Exception("Usuário não possui um portfólio configurado.");

            var transactions = await _transactionRepository.GetByPortfolioIdAsync(portfolio.Id);

            var reportData = transactions.Select(t => new TransactionReportDTO(
                t.CreatedAt,
                t.Type.ToString(), 
                t.Crypto.Name,
                t.Quantity,
                t.UnitPrice * t.Quantity 
            )).ToList();

            return _reportService.GenerateTransactionReport(userEmail, reportData);
        }
    }
}

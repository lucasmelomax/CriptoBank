

using CriptoBank.Application.DTOs.Transaction;
using CriptoBank.Application.Interfaces.TransactionService;
using CriptoBank.Domain.Repositories;

namespace CriptoBank.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IHoldingRepository _holdingRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IPortfolioRepository portfolioRepository, IHoldingRepository holdingRepository, ITransactionRepository transactionRepository)
        {
            _portfolioRepository = portfolioRepository;
            _holdingRepository = holdingRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<TransactionDTO>> GetTransactions(Guid userId)
        {

            var portfolio = await _portfolioRepository.GetByUserIdAsync(userId);
            if (portfolio == null) throw new Exception("Usuário não possui um portfólio configurado.");

            var transactions = await _transactionRepository.GetByPortfolioIdAsync(portfolio.Id);

            return transactions
                    .OrderByDescending(t => t.CreatedAt) 
                    .Select(t => new TransactionDTO
                    {
                        Crypto = t.Crypto.Symbol.ToUpper(),
                        Type = t.Type.ToString(), 
                        Quantity = t.Quantity,
                        UnitPrice = t.UnitPrice,
                        TotalValue = t.Quantity * t.UnitPrice,
                        TransactionDate = t.CreatedAt.ToString("dd/MM/yyyy HH:mm")
                    })
                    .ToList();
        }
    }
}

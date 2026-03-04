
using CriptoBank.Application.Interfaces.BuyService;
using CriptoBank.Domain.Enums;
using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;

namespace CriptoBank.Application.Services
{
    public class CryptoTransactionService : ICryptoTransactionService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IHoldingRepository _holdingRepository;
        private readonly ITransactionRepository _transactionRepository;

        public CryptoTransactionService(IPortfolioRepository portfolioRepository, IHoldingRepository holdingRepository, ITransactionRepository transactionRepository)
        {
            _portfolioRepository = portfolioRepository;
            _holdingRepository = holdingRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task BuyAsync(Guid userId, Guid cryptoId, TransactionType type, decimal quantity, decimal unitPrice, CancellationToken ct)
        {
            var portfolio = await _portfolioRepository.GetByUserIdAsync(userId);
            if (portfolio == null) throw new Exception("Usuário não possui um portfolio configurado.");

            var holding = await _holdingRepository.GetByCrypto(portfolio.Id, cryptoId);

            if (holding == null)
            {
                holding = new Holding(portfolio.Id, cryptoId);
                holding.AddPurchase(quantity, unitPrice);
                await _holdingRepository.Add(holding);
            }
            else
            {
                holding.AddPurchase(quantity, unitPrice);
            }

            var transaction = new Transaction(portfolio.Id, cryptoId, type, quantity, unitPrice);
            await _transactionRepository.Add(transaction);
        }
    }
}


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
        private readonly ICryptoRepository _cryptoRepository;

        public CryptoTransactionService(IPortfolioRepository portfolioRepository, IHoldingRepository holdingRepository, ITransactionRepository transactionRepository, ICryptoRepository cryptoRepository)
        {
            _portfolioRepository = portfolioRepository;
            _holdingRepository = holdingRepository;
            _transactionRepository = transactionRepository;
            _cryptoRepository = cryptoRepository;
        }

        public async Task BuyAsync(Guid userId, string cryptoName, TransactionType type, decimal quantity, decimal unitPrice, CancellationToken ct)
        {
            var crypto = await _cryptoRepository.GetByNameAsync(cryptoName);
            if (crypto == null)
                throw new Exception($"A moeda '{cryptoName}' não foi encontrada no sistema.");

            var portfolio = await _portfolioRepository.GetByUserIdAsync(userId);
            if (portfolio == null)
                throw new Exception("Usuário não possui um portfólio configurado.");

            var holding = await _holdingRepository.GetByCrypto(portfolio.Id, crypto.Id);

            if (holding == null)
            {
                holding = new Holding(portfolio.Id, crypto.Id);
                holding.AddPurchase(quantity, unitPrice);
                await _holdingRepository.Add(holding);
            }
            else
            {
                holding.AddPurchase(quantity, unitPrice);
            }

            var transaction = new Transaction(portfolio.Id, crypto.Id, type, quantity, unitPrice);
            await _transactionRepository.Add(transaction);
        }

        public async Task SellAsync(Guid userId, string cryptoName, TransactionType type, decimal quantity, decimal unitPrice, CancellationToken ct)
        {
            var crypto = await _cryptoRepository.GetByNameAsync(cryptoName);

            if (crypto == null)
                throw new Exception($"A moeda '{cryptoName}' não foi encontrada no sistema.");

            var portfolio = await _portfolioRepository.GetByUserIdAsync(userId);
            if (portfolio == null) throw new Exception("Usuário não possui um portfólio.");

            var holding = await _holdingRepository.GetByCrypto(portfolio.Id, crypto.Id);

            if (holding == null || holding.Quantity < quantity)
            {
                throw new Exception("Saldo insuficiente para realizar a venda.");
            }

            holding.RemoveBalance(quantity);

            var transaction = new Transaction(portfolio.Id, crypto.Id, type, quantity, unitPrice);
            await _transactionRepository.Add(transaction);
        }
    }
}

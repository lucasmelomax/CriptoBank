
using CriptoBank.Application.Interfaces.BuyService;
using CriptoBank.Application.Interfaces.CoinService;
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
        private readonly ICoinService _coinGecko;

        public CryptoTransactionService(IPortfolioRepository portfolioRepository, IHoldingRepository holdingRepository, ITransactionRepository transactionRepository, ICryptoRepository cryptoRepository, ICoinService coinGecko)
        {
            _portfolioRepository = portfolioRepository;
            _holdingRepository = holdingRepository;
            _transactionRepository = transactionRepository;
            _cryptoRepository = cryptoRepository;
            _coinGecko = coinGecko;
        }

        public async Task BuyAsync(Guid userId, string cryptoName, decimal quantity, CancellationToken ct)
        {

            var crypto = await _cryptoRepository.GetByNameAsync(cryptoName);
            if (crypto == null)
                throw new Exception($"A moeda '{cryptoName}' não foi encontrada.");

            var coinData = await _coinGecko.GetCoinDataAsync(crypto.ExternalId);
            if (coinData == null)
                throw new Exception("Não foi possível obter o preço atual da moeda.");

            var unitPrice = coinData.Current_Price;

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

            var transaction = new Transaction(portfolio.Id, crypto.Id, TransactionType.Buy, quantity, unitPrice);
            await _transactionRepository.Add(transaction);
        }

        public async Task SellAsync(Guid userId, string cryptoName, decimal quantity, CancellationToken ct)
        {
            var crypto = await _cryptoRepository.GetByNameAsync(cryptoName);
            if (crypto == null)
                throw new Exception($"A moeda '{cryptoName}' não foi encontrada.");

            var coinData = await _coinGecko.GetCoinDataAsync(crypto.ExternalId);
            if (coinData == null)
                throw new Exception("Não foi possível obter o preço atual da moeda.");

            var unitPrice = coinData.Current_Price;

            var portfolio = await _portfolioRepository.GetByUserIdAsync(userId);
            if (portfolio == null) throw new Exception("Usuário não possui um portfólio.");

            var holding = await _holdingRepository.GetByCrypto(portfolio.Id, crypto.Id);

            if (holding == null || holding.Quantity < quantity)
            {
                throw new Exception("Saldo insuficiente para realizar a venda.");
            }

            holding.RemoveBalance(quantity);

            var transaction = new Transaction(portfolio.Id, crypto.Id, TransactionType.Sell, quantity, unitPrice);
            await _transactionRepository.Add(transaction);
        }
    }
}

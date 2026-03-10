
using System.Security.Claims;
using CriptoBank.Application.Interfaces.BuyService;
using CriptoBank.Application.Interfaces.CoinService;
using CriptoBank.Application.Interfaces.UnitOfWork;
using CriptoBank.Domain.Enums;
using CriptoBank.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CriptoBank.Application.Handlers.SellCrypto
{
    public class SellCryptoCommandHandler : IRequestHandler<SellCryptoCommand, bool>
    {
        private readonly ICryptoTransactionService _transactionService;
        private readonly IUnitOfWork _uow;
        private readonly ICoinService _coinGecko;
        private readonly ICryptoRepository _criptoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SellCryptoCommandHandler(ICryptoTransactionService transactionService, IUnitOfWork uow, ICoinService coinGecko, ICryptoRepository criptoRepository, IHttpContextAccessor httpContextAccessor)
        {
            _transactionService = transactionService;
            _uow = uow;
            _coinGecko = coinGecko;
            _criptoRepository = criptoRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(SellCryptoCommand request, CancellationToken ct)
        {

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) throw new UnauthorizedAccessException("Usuário não autenticado.");
            var userId = Guid.Parse(userIdClaim);

            var crypto = await _criptoRepository.GetByNameAsync(request.cryptoName);
            if (crypto == null) throw new Exception("Criptomoeda não encontrada no sistema.");

            var coinData = await _coinGecko.GetCoinDataAsync(crypto.ExternalId);

            if (coinData == null)
                throw new Exception("Não foi possível obter o preço atual da moeda.");

            var unitPrice = coinData.Current_Price;

            await _transactionService.SellAsync(
                userId,
                request.cryptoName,
                TransactionType.Sell,
                request.Quantity,
                unitPrice,
                ct);

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}

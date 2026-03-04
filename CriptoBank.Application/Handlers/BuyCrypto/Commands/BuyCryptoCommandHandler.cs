
using System.Security.Claims;
using CriptoBank.Application.Interfaces.BuyService;
using CriptoBank.Application.Interfaces.CoinService;
using CriptoBank.Application.Interfaces.UnitOfWork;
using CriptoBank.Domain.Enums;
using CriptoBank.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CriptoBank.Application.Handlers.BuyCrypto.Commands
{
    public class BuyCryptoCommandHandler : IRequestHandler<BuyCryptoCommand, bool>
    {
        private readonly ICryptoTransactionService _transactionService;
        private readonly IUnitOfWork _uow;
        private readonly ICoinService _coinGecko; 
        private readonly ICryptoRepository _criptoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BuyCryptoCommandHandler(ICryptoTransactionService transactionService, IUnitOfWork uow, ICoinService coinGecko, ICryptoRepository criptoRepository, IHttpContextAccessor httpContextAccessor)
        {
            _transactionService = transactionService;
            _uow = uow;
            _coinGecko = coinGecko;
            _criptoRepository = criptoRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(BuyCryptoCommand request, CancellationToken ct)
        {


            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) throw new UnauthorizedAccessException("...");
            var userId = Guid.Parse(userIdClaim);


            var crypto = await _criptoRepository.GetByIdAsync(request.CryptoId);
            if (crypto == null) throw new Exception("Criptomoeda não encontrada no sistema.");


            var coinData = await _coinGecko.GetCoinDataAsync(crypto.ExternalId);
            var unitPrice = coinData.Current_Price;


            await _transactionService.BuyAsync(
                userId,
                request.CryptoId,
                TransactionType.Buy,
                request.Quantity,
                unitPrice,
                ct);

            await _uow.SaveChangesAsync();

            return true;
        }
    }
    
}

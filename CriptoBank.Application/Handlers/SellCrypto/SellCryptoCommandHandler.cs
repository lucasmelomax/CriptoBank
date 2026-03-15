
using CriptoBank.Application.Interfaces.BuyService;
using CriptoBank.Application.Interfaces.Token;
using CriptoBank.Application.Interfaces.UnitOfWork;
using MediatR;

namespace CriptoBank.Application.Handlers.SellCrypto
{
    public class SellCryptoCommandHandler : IRequestHandler<SellCryptoCommand, bool>
    {
        private readonly ICryptoTransactionService _transactionService;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUserService;

        public SellCryptoCommandHandler(ICryptoTransactionService transactionService, IUnitOfWork uow, ICurrentUserService currentUserService)
        {
            _transactionService = transactionService;
            _uow = uow;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(SellCryptoCommand request, CancellationToken ct)
        {

            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Usuário não identificado.");

            await _transactionService.SellAsync(
                userId,
                request.cryptoName,
                request.Quantity,
                ct);

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}

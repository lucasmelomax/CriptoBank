
using CriptoBank.Application.Interfaces.BuyService;
using CriptoBank.Application.Interfaces.Token;
using CriptoBank.Application.Interfaces.UnitOfWork;
using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;
using MediatR;

namespace CriptoBank.Application.Handlers.BuyCrypto.Commands
{
    public class BuyCryptoCommandHandler : IRequestHandler<BuyCryptoCommand, bool>
    {
        private readonly ICryptoTransactionService _transactionService;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWalletRepository _walletRepository;

        public BuyCryptoCommandHandler(ICryptoTransactionService transactionService, IUnitOfWork uow, ICurrentUserService currentUserService, IWalletRepository walletRepository)
        {
            _transactionService = transactionService;
            _uow = uow;
            _currentUserService = currentUserService;
            _walletRepository = walletRepository;
        }

        public async Task<bool> Handle(BuyCryptoCommand request, CancellationToken ct)
        {

            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Usuário não identificado.");

            var saldo = await _walletRepository.GetById(userId);

            await _transactionService.BuyAsync(
                userId,
                request.cryptoName,
                request.Quantity,
                ct);

            await _uow.SaveChangesAsync();

            return true;
        }
    }
    
}

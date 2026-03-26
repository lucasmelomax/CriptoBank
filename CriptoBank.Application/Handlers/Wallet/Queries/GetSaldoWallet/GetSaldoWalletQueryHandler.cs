

using CriptoBank.Application.DTOs.Wallet;
using CriptoBank.Application.Handlers.Wallet.Queries.GetSaldo;
using CriptoBank.Application.Interfaces.Token;
using CriptoBank.Domain.Repositories;
using MediatR;

namespace CriptoBank.Application.Handlers.Wallet.Queries.GetSaldoWallet
{
    public class GetSaldoWalletQueryHandler : IRequestHandler<GetSaldoWalletQuery, WalletSaldoDTO>
    {

        private readonly IWalletRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public GetSaldoWalletQueryHandler(IWalletRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<WalletSaldoDTO> Handle(GetSaldoWalletQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var userEmail = _currentUserService.Email;

            if (userId == null || userEmail == null) throw new UnauthorizedAccessException("Usuário não identificado.");

            var wallet = await _repository.GetById(userId.Value);
            if (wallet == null) throw new Exception("Erro ao achar wallet do usuario.");

            var walletDTO = new WalletSaldoDTO(userEmail, wallet.Balance);

            return walletDTO;

        }
    }
}


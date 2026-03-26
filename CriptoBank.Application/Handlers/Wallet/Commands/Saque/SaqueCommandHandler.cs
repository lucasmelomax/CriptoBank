

using CriptoBank.Application.Interfaces.Token;
using CriptoBank.Application.Interfaces.UnitOfWork;
using CriptoBank.Domain.Enums;
using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;
using MediatR;

namespace CriptoBank.Application.Handlers.Wallet.Commands.Saque
{
    public class SaqueCommandHandler : IRequestHandler<SaqueCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletHistoryRepository _walletHistoryRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public SaqueCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IWalletRepository walletRepository, IWalletHistoryRepository walletHistoryRepository, IPortfolioRepository portfolioRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _walletRepository = walletRepository;
            _walletHistoryRepository = walletHistoryRepository;
            _portfolioRepository = portfolioRepository;
        }

        public async Task<bool> Handle(SaqueCommand request, CancellationToken ct)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new UnauthorizedAccessException("Usuário não identificado.");

            if (request.valor <= 0)
                throw new ArgumentException("O valor do saque deve ser maior que zero.");

            var portfolio = await _portfolioRepository.GetByUserIdAsync(userId.Value);
            if (portfolio == null)
                throw new KeyNotFoundException("Portfolio não encontrado.");

            var wallet = await _walletRepository.GetById(userId.Value);

            if (wallet == null)
                throw new InvalidOperationException("Carteira não encontrada ou saldo insuficiente.");

            wallet.WithDraw(request.valor);

            var historico = new WalletHistory(wallet.Id, request.valor, TransactionType.Saque);
            await _walletHistoryRepository.Add(historico);

            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }
    }
}

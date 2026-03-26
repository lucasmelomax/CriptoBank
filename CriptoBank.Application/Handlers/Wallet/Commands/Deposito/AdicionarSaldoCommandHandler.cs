using CriptoBank.Application.DTOs.Transaction;
using CriptoBank.Application.Interfaces.Token;
using CriptoBank.Application.Interfaces.UnitOfWork;
using CriptoBank.Application.Repositories.Token;
using CriptoBank.Domain.Enums;
using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;
using MediatR;

namespace CriptoBank.Application.Handlers.Wallet.Commands.Deposito
{
    public class AdicionarSaldoCommandHandler : IRequestHandler<AdicionarSaldoCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWalletRepository _walletRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IWalletHistoryRepository _walletHistoryRepository;

        public AdicionarSaldoCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IWalletRepository walletRepository, IPortfolioRepository portfolioRepository, IWalletHistoryRepository walletHistoryRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _walletRepository = walletRepository;
            _portfolioRepository = portfolioRepository;
            _walletHistoryRepository = walletHistoryRepository;
        }

        public async Task<bool> Handle(AdicionarSaldoCommand request, CancellationToken ct)
        {

            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new UnauthorizedAccessException("Usuário não identificado.");

            if (request.saldo <= 0)
                throw new ArgumentException("O valor do saque deve ser maior que zero.");

            var portfolio = await _portfolioRepository.GetByUserIdAsync(userId.Value);
            if (portfolio == null)
                throw new KeyNotFoundException("Portfolio não encontrado.");

            var wallet = await _walletRepository.GetById(userId.Value);

            if (wallet == null)
                throw new InvalidOperationException("Carteira não encontrada ou saldo insuficiente.");

            wallet.Deposit(request.saldo);

            var historico = new WalletHistory(wallet.Id, request.saldo, TransactionType.Deposito);
            await _walletHistoryRepository.Add(historico);

            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }
    }
}

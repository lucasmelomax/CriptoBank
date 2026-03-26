using CriptoBank.Application.DTOs.Wallet;
using CriptoBank.Application.Interfaces.Token;
using CriptoBank.Domain.Repositories;
using MediatR;

namespace CriptoBank.Application.Handlers.Wallet.Queries.GetAll
{
    public class GetAllWalletsHistoryQueryHandler : IRequestHandler<GetAllWalletsHistoryQuery, IEnumerable<WalletDTO>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletHistoryRepository _walletHistoryRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public GetAllWalletsHistoryQueryHandler(ICurrentUserService currentUserService, IWalletRepository walletRepository, IWalletHistoryRepository walletHistoryRepository, IPortfolioRepository portfolioRepository)
        {
            _currentUserService = currentUserService;
            _walletRepository = walletRepository;
            _walletHistoryRepository = walletHistoryRepository;
            _portfolioRepository = portfolioRepository;
        }

        public async Task<IEnumerable<WalletDTO>> Handle(GetAllWalletsHistoryQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var userEmail = _currentUserService.Email;
            if (userId == null || userEmail == null)
                throw new UnauthorizedAccessException("Usuário não identificado.");

            var wallet = await _walletRepository.GetById(userId.Value);
            if (wallet == null)
                throw new InvalidOperationException("Carteira não encontrada ou saldo insuficiente.");

            var walletHistory = await _walletHistoryRepository.GetAllWalletHistories(wallet.Id);

            var historyDTO = walletHistory.Select(x => new WalletDTO(
                userEmail,
                x.Amount,
                x.Type,
                x.CreatedAt
            )).ToList();

            return historyDTO;
        }
    }
}

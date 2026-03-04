
using CriptoBank.Application.Interfaces.CoinService;
using CriptoBank.Application.Interfaces.UnitOfWork;
using MediatR;

namespace CriptoBank.Application.Handlers.Coins.Commands
{
    public class SyncCryptosCommandHandler : IRequestHandler<SyncCryptosCommand, bool>
    {
        private readonly ICoinService _coinService;
        private readonly IUnitOfWork _uow;

        public SyncCryptosCommandHandler(ICoinService coinService, IUnitOfWork uow)
        {
            _coinService = coinService;
            _uow = uow;
        }

        public async Task<bool> Handle(SyncCryptosCommand request, CancellationToken ct)
        {

            await _coinService.SyncCryptosAsync();

            var rowsAffected = await _uow.SaveChangesAsync();

            return rowsAffected > 0;
        }
    }
}

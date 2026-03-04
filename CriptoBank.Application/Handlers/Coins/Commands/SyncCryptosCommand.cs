using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CriptoBank.Application.Handlers.Coins.Commands
{
    public record SyncCryptosCommand() : IRequest<bool>
    {
    }
}

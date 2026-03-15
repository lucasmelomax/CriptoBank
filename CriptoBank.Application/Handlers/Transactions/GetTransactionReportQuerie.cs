
using MediatR;

namespace CriptoBank.Application.Handlers.Transactions
{
    public record GetTransactionReportQuerie() : IRequest<byte[]>
    {
    }
}

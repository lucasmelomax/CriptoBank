
using CriptoBank.Application.DTOs.Transaction;
using MediatR;

namespace CriptoBank.Application.Handlers.Transactions
{
    public record GetTransactionsQuery() : IRequest<IEnumerable<TransactionDTO>>
    {
    }
}

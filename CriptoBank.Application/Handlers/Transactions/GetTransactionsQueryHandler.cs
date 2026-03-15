
using CriptoBank.Application.Interfaces.TransactionService;
using MediatR;
using CriptoBank.Application.DTOs.Transaction;
using CriptoBank.Application.Interfaces.Token;

namespace CriptoBank.Application.Handlers.Transactions
{
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, IEnumerable<TransactionDTO>>
    {
        private readonly ITransactionService _transactionService;
        private readonly ICurrentUserService _currentUserService;

        public GetTransactionsQueryHandler(ITransactionService transactionService, ICurrentUserService currentUserService)
        {
            _transactionService = transactionService;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<TransactionDTO>> Handle(GetTransactionsQuery request, CancellationToken ct)
        {
            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Usuário não identificado.");

            var transactions = await _transactionService.GetTransactions(userId);

            return transactions;
        }
    }
}

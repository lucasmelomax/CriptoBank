
using System.Security.Claims;
using CriptoBank.Application.Interfaces.TransactionService;
using MediatR;
using Microsoft.AspNetCore.Http;
using CriptoBank.Application.DTOs.Transaction;

namespace CriptoBank.Application.Handlers.Transactions
{
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, IEnumerable<TransactionDTO>>
    {
        private readonly ITransactionService _transactionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTransactionsQueryHandler(ITransactionService transactionService, IHttpContextAccessor httpContextAccessor)
        {
            _transactionService = transactionService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<TransactionDTO>> Handle(GetTransactionsQuery request, CancellationToken ct)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            var userId = Guid.Parse(userIdClaim);

            var transactions = await _transactionService.GetTransactions(userId);

            return transactions;
        }
    }
}

using CriptoBank.Application.Handlers.Transactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriptoBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {

        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("transactions")]

        public async Task<ActionResult> GetAllTransactions()
        {
            var transactions = await _mediator.Send(new GetTransactionsQuery());

            return Ok(transactions);
        }
    }
}

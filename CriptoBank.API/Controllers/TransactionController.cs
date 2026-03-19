using CriptoBank.Application.DTOs.Message;
using CriptoBank.Application.Handlers.Transactions;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CriptoBank.Application.Interfaces.Token;

namespace CriptoBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public TransactionController(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("transactions")]

        public async Task<ActionResult> GetAllTransactions()
        {
            var transactions = await _mediator.Send(new GetTransactionsQuery());

            return Ok(transactions);
        }

        [HttpGet("report")]
        public async Task<IActionResult> DownloadReport([FromServices] ICurrentUserService userContext)
        {
            await _publishEndpoint.Publish(new GenerateReportMessage
            {
                UserId = (Guid)userContext.UserId,
                UserEmail = userContext.Email
            });

            return Accepted(new { message = "Relatorio em processamento. Verifique a pasta de destino." });
        }

        [HttpPost("send-via-email")]
        public async Task<IActionResult> SendViaEmail([FromServices] ICurrentUserService userContext)
        {
            await _publishEndpoint.Publish(new EmailGenerateReportMessage
            {
                UserId = (Guid)userContext.UserId,
                UserEmail = userContext.Email
            });

            return Accepted(new { message = $"Processamento iniciado para o e-mail {userContext.Email}!" });
        }
    }
}

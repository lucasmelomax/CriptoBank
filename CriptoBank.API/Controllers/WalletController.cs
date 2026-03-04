
using CriptoBank.Application.Handlers.BuyCrypto.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriptoBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {

        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy([FromBody] BuyCryptoCommand command)
        {

            var result = await _mediator.Send(command);

            if (!result) return BadRequest("Não foi possível processar a compra.");

            return Ok("Compra realizada com sucesso!");
        }
    }
}

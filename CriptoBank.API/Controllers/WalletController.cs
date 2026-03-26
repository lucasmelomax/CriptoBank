
using CriptoBank.Application.Handlers.BuyCrypto.Commands;
using CriptoBank.Application.Handlers.SellCrypto;
using CriptoBank.Application.Handlers.Wallet.Commands.Deposito;
using CriptoBank.Application.Handlers.Wallet.Commands.Saque;
using CriptoBank.Application.Handlers.Wallet.Queries.GetAll;
using CriptoBank.Application.Handlers.Wallet.Queries.GetSaldo;
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

        [HttpGet("saldo")]
        public async Task<IActionResult> Saldo()
        {
            var query = new GetSaldoWalletQuery();
            var result = await _mediator.Send(query);

            if (result == null) return BadRequest("Não foi possível mostrar o saldo.");

            return Ok(result);
        }

        [HttpGet("historico")]
        public async Task<IActionResult> Historico()
        {
            var query = new GetAllWalletsHistoryQuery();
            var result = await _mediator.Send(query);

            if (result == null) return BadRequest("Não foi possível mostrar o histórico.");

            return Ok(result);
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy([FromBody] BuyCryptoCommand command)
        {

            var result = await _mediator.Send(command);

            if (!result) return BadRequest("Não foi possível processar a compra.");

            return Ok("Compra realizada com sucesso!");
        }

        [HttpPost("sell")]
        public async Task<IActionResult> Sell([FromBody] SellCryptoCommand command)
        {

            var result = await _mediator.Send(command);

            if (!result) return BadRequest("Não foi possível processar a venda.");

            return Ok("Venda realizada com sucesso!");
        }

        [HttpPost("deposito")]
        public async Task<IActionResult> Deposito([FromBody] AdicionarSaldoCommand command)
        {

            var result = await _mediator.Send(command);

            if (!result) return BadRequest("Não foi possível fazer deposito.");

            return Ok("Deposito feito com sucesso!");
        }


        [HttpPost("saque")]
        public async Task<IActionResult> Saque([FromBody] SaqueCommand command)
        {

            var result = await _mediator.Send(command);

            if (!result) return BadRequest("Não foi possível fazer o saque.");

            return Ok("Saque feito com sucesso!");
        }
    }
}


using CriptoBank.Application.DTOs.Crypto;
using CriptoBank.Application.Handlers.Coins.Commands;
using CriptoBank.Application.Handlers.Coins.Queries.GetAllCoins;
using CriptoBank.Application.Handlers.Coins.Queries.GetCoin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriptoBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoinController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoinController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<CoinMarketDto>>> GetAllCoins(CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetAllCoinsQuerie(), ct);
            return Ok(result);
        }

        [HttpGet("{nome}")]
        public async Task<ActionResult<List<CoinMarketDto>>> GetCoin(string nome, CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetCoinQuerie(nome), ct);
            if (result == null ) return NotFound($"Nenhuma informação encontrada para a moeda: {nome}");

            return Ok(result);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> Sync()
        {
            var result = await _mediator.Send(new SyncCryptosCommand());
            return result ? Ok("Sincronização concluída") : BadRequest("Falha na sincronização");
        }
    }
}

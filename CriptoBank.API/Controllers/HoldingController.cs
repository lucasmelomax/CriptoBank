
using CriptoBank.Application.Handlers.Holdings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriptoBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HoldingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HoldingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("holdings")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllHoldingsQuery());
            return Ok(result);
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult> GetDashboard()
        {
            var result = await _mediator.Send(new GetDashboardQuery());
            return Ok(result);
        }
    }
}

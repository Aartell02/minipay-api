using Microsoft.AspNetCore.Mvc;
using MiniPay.Application.Commands;
using MiniPay.Application.Handlers;
using MiniPay.Application.Interfaces;
using MiniPay.Domain.Aggregates;
using MediatR;

namespace MiniPay.API.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController(IMediator mediator) : ControllerBase
    {

        [HttpPost("initiate")]
        public async Task<IActionResult> Initiate([FromBody] InitiatePaymentCommand request)
        {
            var response = await mediator.Send(request);

            return Ok(new { response });
        }

        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] AuthorizePaymentCommand request)
        {
            var response = await mediator.Send(request);

            return Ok(new { response });
        }

        [HttpPost("settle")]
        public async Task<IActionResult> Settle([FromBody]SettlePaymentCommand request)
        {
            var response = await mediator.Send(request);

            return Ok(new { response });
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await mediator.Send(new GetPaymentByIdCommand(id));

            return Ok(new { response });
        }
    }
}

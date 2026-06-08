using Microsoft.AspNetCore.Mvc;
using MiniPay.Application.Commands;
using MediatR;

namespace MiniPay.API.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController(IMediator mediator) : ControllerBase
    {

        [HttpPost("initiate")]
        public async Task<IActionResult> Initiate([FromBody] TransactionInitiateCommand request)
        {
            var response = await mediator.Send(request);

            return Ok(new { response });
        }

        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] TransactionAuthorizeCommand request)
        {
            var response = await mediator.Send(request);

            return Ok(new { response });
        }

        [HttpPost("settle")]
        public async Task<IActionResult> Settle([FromBody]TransactionSettleCommand request)
        {
            var response = await mediator.Send(request);

            return Ok(new { response });
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await mediator.Send(new TransactionGetByIdCommand(id));

            return Ok(new { response });
        }
    }
}

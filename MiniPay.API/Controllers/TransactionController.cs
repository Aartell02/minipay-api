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
        public async Task<IActionResult> Initiate(InitiatePaymentCommand request)
        {
            var id = await mediator.Send(request);

            return Ok(new { id });
        }

        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize(AuthorizePaymentCommand request)
        {
            var id = await mediator.Send(request);

            return Ok(new { id });
        }

        [HttpPost("settle")]
        public async Task<IActionResult> Settle(SettlePaymentCommand request)
        {
            var id = await mediator.Send(request);

            return Ok(new { id });
        }
        /*
        // GET /api/transactions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var events = await _eventStore.LoadAsync(id);
            if (!events.Any())
                return NotFound();

            var transaction = Transaction.Rewind(events);

            return Ok(new
            {
                transaction.Id,
                transaction.Amount,
                transaction.Currency,
                transaction.Status,
                History = events.Select(e => new
                {
                    Type = e.GetType().Name,
                    e.OccurredAt
                })
            });

        }
        */
    }
}

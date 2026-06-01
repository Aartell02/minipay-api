using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using MiniPay.API.Domain.Aggregates;
using MiniPay.API.Infrastructure.EventStore;

namespace MiniPay.API.API.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly IEventStore _eventStore;

        public TransactionController(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        [HttpPost]
        public async Task<IActionResult> Initiate([FromBody] InitiatePaymentRequest request)
        {
            var transaction = Transaction.Initiate(request.Amount, request.Currency);
            await _eventStore.SaveAsync(transaction.UncommittedEvents);

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, new
            {
                transaction.Id,
                transaction.Amount,
                transaction.Currency,
                transaction.Status
            });
        }

        [HttpPost("{id}/authorize")]
        public async Task<IActionResult> Authorize(Guid id)
        {
            var events = await _eventStore.LoadAsync(id);
            if (!events.Any())
                return NotFound();

            var transaction = Transaction.Replay(events);
            transaction.Authorize();
            await _eventStore.SaveAsync(transaction.UncommittedEvents);

            return Ok(new { transaction.Id, transaction.Status });
        }

        // POST /api/transactions/{id}/settle
        [HttpPost("{id}/settle")]
        public async Task<IActionResult> Settle(Guid id)
        {
            var events = await _eventStore.LoadAsync(id);
            if (!events.Any())
                return NotFound();

            var transaction = Transaction.Replay(events);
            transaction.Settle();
            await _eventStore.SaveAsync(transaction.UncommittedEvents);

            return Ok(new { transaction.Id, transaction.Status });
        }

        // GET /api/transactions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var events = await _eventStore.LoadAsync(id);
            if (!events.Any())
                return NotFound();

            var transaction = Transaction.Replay(events);

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
    }
    public record InitiatePaymentRequest(decimal Amount, string Currency);
}

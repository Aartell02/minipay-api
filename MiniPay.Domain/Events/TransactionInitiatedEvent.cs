using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPay.Domain.Events
{
    public record TransactionInitiatedEvent(Guid TransactionId, decimal Amount, string Currency) : TransactionEvent(TransactionId);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPay.Application.DTOs
{
    public record TransactionDto(
        Guid Id,
        decimal Amount,
        string Currency,
        string Status,
        IEnumerable<string> History);
}

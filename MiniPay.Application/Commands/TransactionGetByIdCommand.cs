using MediatR;
using MiniPay.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPay.Application.Commands
{
    public record TransactionGetByIdCommand(Guid Id) : IRequest<TransactionDto>;
}

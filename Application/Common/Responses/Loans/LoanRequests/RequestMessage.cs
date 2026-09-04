using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses.Loans.LoanRequests
{
    public record struct RequestMessage(bool Success,string Message,string? Errors=null);
}

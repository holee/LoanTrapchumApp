using Applications.Responses;
using Domains.Models.Lists.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Customers
{
    public interface ICustomerNoteService
    {
        Task<Result<string>> AddCustomerNote(CustomerNote model, CancellationToken cancellation = default);
        Task<Result<IEnumerable<T>>> GetCustomerNotes<T>(string? customerId, CancellationToken cancellation = default) where T : class;
    }
}

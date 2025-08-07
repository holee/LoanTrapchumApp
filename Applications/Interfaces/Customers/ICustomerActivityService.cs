using Applications.Responses;
using Domains.Models.Lists.Customers;

namespace Applications.Interfaces.Customers;

public interface ICustomerActivityService
{
    Task<Result<string>> AddCustomerActivity(CustomerActivity model,CancellationToken cancellation = default);
    Task<Result<IEnumerable<T>>> GetCustomerActivies<T>(string? customerId, CancellationToken cancellation = default) where T : class;
}

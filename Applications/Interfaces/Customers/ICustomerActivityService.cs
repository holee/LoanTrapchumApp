using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Applications.Interfaces.Customers;

public interface ICustomerActivityService
{
    Task<Result<string>> AddCustomerActivity(CustomerActivity model,CancellationToken cancellation = default);
    Task<Result<IEnumerable<VCustomerActivities>>> GetCustomerActivies(string? customerId, CancellationToken cancellation = default);
}

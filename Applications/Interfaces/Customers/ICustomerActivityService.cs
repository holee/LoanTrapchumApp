using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Applications.Interfaces.Customers;

public interface ICustomerActivityService
{
    Task<ErrorOr<int>> AddOrUpdateActivity(CustomerActivity model, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> DeleteActivityAsync(int? id, string customerId, CancellationToken cancellation = default);
    Task<Result<IEnumerable<VCustomerActivities>>> GetCustomerActivies(string? customerId, CancellationToken cancellation = default);
    Task<Result<VCustomerActivities>> GetCustomerActivity(int? id, string? customerId, CancellationToken cancellationToken=default);
}

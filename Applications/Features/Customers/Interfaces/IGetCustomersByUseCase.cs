using Applications.Features.Customers.Response;
using Applications.Responses.Paginations;

namespace Applications.Features.Customers.Interfaces
{
    public interface IGetCustomersByUseCase
    {
        Task<Pager<VCustomerList>> ExecuteAsync(int? branchId, string? userId, string? customerId, QueryParam param);
    }
}
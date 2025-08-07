using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;

namespace Applications.Features.Customers.UseCases
{
    public sealed class GetCustomerActivityUseCase
    {
        private readonly ICustomerActivityService _actService;

        public GetCustomerActivityUseCase(ICustomerActivityService actService)
        {
            _actService = actService;
        }

        public async Task<Result<IEnumerable<VCustomerActivities>>> GetCustomerActivities(string? customerId, CancellationToken cancellation = default)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return Result.Failure<IEnumerable<VCustomerActivities>>(ErrorResponse.Validation("Customer ID Is Required."));
            }
            return await _actService.GetCustomerActivies<VCustomerActivities>(customerId, cancellation);
        }
    }

}

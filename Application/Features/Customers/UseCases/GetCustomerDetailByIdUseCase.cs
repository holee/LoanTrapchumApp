using Application.Interfaces.Customers;

namespace Application.Features.Customers.UseCases
{
    public sealed class GetCustomerDetailByIdUseCase
    {
        private readonly ICustomerService _custService;

        public GetCustomerDetailByIdUseCase(ICustomerService custService)
        {
            _custService = custService;
        }

        //public async Task<Result<VCustomerDetails>> ExecuteAsync(string? customerId, bool includes)
        //{
        //    if (string.IsNullOrWhiteSpace(customerId))
        //    {
        //        return Result.Failure<VCustomerDetails>(ErrorResponse.Validation("Customer ID is required."));
        //    }

        //    return await _custService.GetCustomerDetailByIdAsync(customerId, includes);
        //}

    }
}

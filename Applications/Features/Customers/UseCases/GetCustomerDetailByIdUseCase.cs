using Applications.Features.Customers.Response;
using Applications.Interfaces.Customers;
using Applications.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Customers.UseCases
{
    public sealed class GetCustomerDetailByIdUseCase
    {
        private readonly ICustomerService _custService;

        public GetCustomerDetailByIdUseCase(ICustomerService custService)
        {
            _custService = custService;
        }

        public async Task<Result<VCustomerDetails>> ExecuteAsync(string? customerId, bool includes)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return Result.Failure<VCustomerDetails>(ErrorResponse.Validation("Customer ID is required."));
            }

            return await _custService.GetCustomerDetailByIdAsync(customerId, includes);
        }

    }
}

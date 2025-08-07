using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Customers.UseCases
{
    public sealed class GetCustomerDetailByIdCommand
    {
        private readonly ICustomerService _custService;

        public GetCustomerDetailByIdCommand(ICustomerService custService)
        {
            _custService = custService;
        }

        public async Task<Result<VCustomerList>> ExecuteAsync(string? customerId, params IEnumerable<Expression<Func<VCustomerList, object>>> includes)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return Result.Failure<VCustomerList>(ErrorResponse.Validation("Customer ID is required."));
            }

            return await _custService.GetCustomerDetailByIdAsync(customerId, includes);
        }

    }
}

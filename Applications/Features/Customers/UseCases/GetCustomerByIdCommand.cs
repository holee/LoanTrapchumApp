using Applications.Interfaces.Customers;
using Applications.Responses;
using Domains.Models.Lists;

namespace Applications.Features.Customers.UseCases;

public sealed class GetCustomerByIdCommand
{
    private readonly ICustomerService _custService;

    public GetCustomerByIdCommand(ICustomerService custService)
    {
        _custService = custService;
    } 

    //public async Task<Result<EntityNames>> ExecuteAsync(string? customerId)
    //{
    //    if (string.IsNullOrWhiteSpace(customerId))
    //    {
    //        return Result.Failure<EntityNames>(ErrorResponse.Validation("Customer ID is required."));
    //    }

    //    return await _custService.GetCustomerByIdAsync(customerId);
    //}
}

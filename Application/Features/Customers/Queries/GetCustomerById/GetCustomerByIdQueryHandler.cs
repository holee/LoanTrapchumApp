using Application.Commons.Messages.Customers;
using Application.Interfaces.Customers;
using Application.Utils;
using Domains.Models.Lists;
using ErrorOr;

namespace Application.Features.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ErrorOr<EntityNames>>
{
    private readonly ICustomerService _custService;

    public GetCustomerByIdQueryHandler(ICustomerService custService)
    {
        _custService = custService;
    }


    public async Task<ErrorOr<EntityNames>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerId))
        {
            return Error.NotFound(CustomerMessage.Required);
        }

        var customer = await _custService.GetCustomerByIdAsync(request.CustomerId);

        if(customer is null)
        {
            return Error.NotFound(CustomerMessage.NotFound);
        }

        return customer;

    }
}

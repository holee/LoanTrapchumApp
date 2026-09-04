using Application.Commons;
using Application.Extensions;
using Application.Interfaces.Customers;
using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;
using FluentValidation;

namespace Application.Features.Activities.ActitvityQuery.GetActivityByCustomer;

public sealed class GetActivityByCustomerIdQueryHandler
    : IRequestHandler<GetActivityByCustomerIdQuery, ErrorOr<IEnumerable<VCustomerActivities>>>
{
    private readonly ICustomerActivityService _customerActivityService;

    public GetActivityByCustomerIdQueryHandler(ICustomerActivityService customerActivityService)
    {
        _customerActivityService = customerActivityService;
    }


    public async Task<ErrorOr<IEnumerable<VCustomerActivities>>> Handle(GetActivityByCustomerIdQuery request, CancellationToken cancellationToken = default)
    {
        var rsp = await _customerActivityService.GetCustomerActivies(request.CustomerId, cancellationToken);

        if (rsp == null)
        {
            return Error.NotFound("There is no any customer notes.");
        }

        return rsp.ToList();
    }
}
using Applications.Commons;
using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Activities.ActitvityQuery.GetActivityByCustomer;

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
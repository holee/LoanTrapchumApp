using Application.Interfaces.Customers;
using Application.Responses.ViewModels.Customers;
using Application.Utils;

namespace Application.Features.Activities.ActitvityQuery.GetActivitiesQuery;

public sealed class GetCustomerActivityQueryHandler : IRequestHandler<GetCustomerActivityQuery, IEnumerable<VCustomerActivities>>
{
    private readonly ICustomerActivityService _customerActivity;

    public GetCustomerActivityQueryHandler(ICustomerActivityService customerActivity)
    {
        _customerActivity = customerActivity;
    }

    public async Task<IEnumerable<VCustomerActivities>> Handle(GetCustomerActivityQuery request, CancellationToken cancellationToken = default)
    {
        return await _customerActivity.GetCustomerActivies(request.CustomerId, cancellationToken);
    }
}

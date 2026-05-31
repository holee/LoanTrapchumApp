using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;

namespace Applications.Features.Activities.ActitvityQuery.GetActivitiesQuery;

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

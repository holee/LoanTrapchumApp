using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.GetCustomerActivityQuery;

public sealed class GetCustomerActivityQueryHandler : IRequestHandler<GetCustomerActivityQuery, IEnumerable<VCustomerActivities>>
{
    private readonly ICustomerActivityService _customerActivity;

    public GetCustomerActivityQueryHandler(ICustomerActivityService customerActivity)
    {
        _customerActivity = customerActivity;
    }

    public async Task<IEnumerable<VCustomerActivities>> Handle(GetCustomerActivityQuery request, CancellationToken cancellationToken = default)
    {
        var rsp = await _customerActivity.GetCustomerActivies(request.CustomerId, cancellationToken);
        return rsp.IsSuccess && rsp.Data is not null ? rsp.Data : [];
    }
}

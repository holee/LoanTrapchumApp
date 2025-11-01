using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Activities.ActitvityQuery.GetActivityByCustomer;

public sealed record GetActivityByCustomerIdQuery(string CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerActivities>>>;

using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.ActitvityQuery.GetActivityByCustomer;

public sealed record GetActivityByCustomerIdQuery(string CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerActivities>>>;

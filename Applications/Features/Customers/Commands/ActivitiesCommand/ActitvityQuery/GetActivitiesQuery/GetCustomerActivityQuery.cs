using Applications.Responses.ViewModels.Customers;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.GetCustomerActivityQuery;

public sealed record GetCustomerActivityQuery(string? CustomerId) : IRequest<IEnumerable<VCustomerActivities>>;

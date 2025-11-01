using Applications.Responses.ViewModels.Customers;
using Applications.Utils;

namespace Applications.Features.Activities.ActitvityQuery.GetActivitiesQuery;

public sealed record GetCustomerActivityQuery(string? CustomerId) : IRequest<IEnumerable<VCustomerActivities>>;

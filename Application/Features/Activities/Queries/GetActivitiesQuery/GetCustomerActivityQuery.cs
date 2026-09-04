using Application.Responses.ViewModels.Customers;
using Application.Utils;

namespace Application.Features.Activities.ActitvityQuery.GetActivitiesQuery;

public sealed record GetCustomerActivityQuery(string? CustomerId) : IRequest<IEnumerable<VCustomerActivities>>;

using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Activities.ActitvityQuery.GetActivityByCustomer;

public sealed record GetActivityByCustomerIdQuery(string CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerActivities>>>;

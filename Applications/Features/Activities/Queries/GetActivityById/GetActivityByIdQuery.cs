using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Activities.ActitvityQuery.GetActivityById;

public record GetActivityByIdQuery(int? Id, string? CustomerId) : IRequest<ErrorOr<VCustomerActivities>>;

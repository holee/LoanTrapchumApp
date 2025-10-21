using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.ActitvityQuery.GetActivityById;

public record GetActivityByIdQuery(int Id, string CustomerId)
    : IRequest<ErrorOr<VCustomerActivities>>;

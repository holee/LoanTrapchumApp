using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Activities.ActitvityQuery.GetActivityById;

public record GetActivityByIdQuery(int? Id, string? CustomerId) : IRequest<ErrorOr<VCustomerActivities>>;

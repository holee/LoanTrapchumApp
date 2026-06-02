using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Activities.ActitvityQuery.GetActivityById;

public sealed class GetActivityByIdQueryHandler :IRequestHandler<GetActivityByIdQuery,ErrorOr<VCustomerActivities>>
{
    private readonly ICustomerActivityService  _cusActivityService;
    public GetActivityByIdQueryHandler(ICustomerActivityService cusActivityService)
    {
        _cusActivityService = cusActivityService;
    }

    public async Task<ErrorOr<VCustomerActivities>> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken = default)
    {
        var act= await _cusActivityService.GetCustomerActivity(request.Id, request.CustomerId, cancellationToken);
    
        if(act is null)
        {
            return Error.NotFound(description:"Customer has no any activies.");
        }

        return act;
    }
}
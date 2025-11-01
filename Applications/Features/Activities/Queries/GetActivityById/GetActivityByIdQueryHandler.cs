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
        
        var rsp =await _cusActivityService.GetCustomerActivity(request.Id, request.CustomerId, cancellationToken);

        if (rsp is { IsSuccess: true, Data: not null })
        {
            return rsp.Data; 
        }
        
        return Error.NotFound(rsp.ErrMessage);
        
    }
}
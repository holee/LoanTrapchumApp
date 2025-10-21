using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.ActitvityQuery.GetActivityById;

public sealed class GetActivityByIdQueryHandler :IRequestHandler<GetActivityByIdQuery,ErrorOr<VCustomerActivities>>
{
    private readonly ICustomerActivityService  _cusActivityService;
   private IValidator<GetActivityByIdQuery> _validator;
    public GetActivityByIdQueryHandler(ICustomerActivityService cusActivityService, IValidator<GetActivityByIdQuery> validator)
    {
        _cusActivityService = cusActivityService;
        _validator = validator;
    }

    public async Task<ErrorOr<VCustomerActivities>> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken = default)
    {
        var validator= await _validator.ValidateAsync(request);

        if (!validator.IsValid)
        {
            return validator.ToErrorOr<VCustomerActivities>();
        }
        
        var rsp =await _cusActivityService.GetCustomerActivity(request.Id, request.CustomerId, cancellationToken);

        if (rsp is { IsSuccess: true, Data: not null })
        {
            return rsp.Data; 
        }
        
        return Error.NotFound(rsp.ErrMessage);
        
    }
}
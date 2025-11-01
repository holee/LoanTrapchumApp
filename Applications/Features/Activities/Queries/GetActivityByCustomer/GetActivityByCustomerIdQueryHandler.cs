using Applications.Commons;
using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Activities.ActitvityQuery.GetActivityByCustomer;

public sealed class GetActivityByCustomerIdQueryHandler
    :IRequestHandler<GetActivityByCustomerIdQuery,ErrorOr<IEnumerable<VCustomerActivities>>>
{
    private readonly ICustomerActivityService  _customerActivityService;

    private readonly IValidator<GetActivityByCustomerIdQuery> _validator;
    public GetActivityByCustomerIdQueryHandler(ICustomerActivityService customerActivityService, IValidator<GetActivityByCustomerIdQuery> validator)
    {
        _customerActivityService = customerActivityService;
        _validator = validator;
    }


    public async Task<ErrorOr<IEnumerable<VCustomerActivities>>> Handle(GetActivityByCustomerIdQuery request, CancellationToken cancellationToken = default)
    {
        
        var validator =await _validator.ValidateAsync(request, cancellationToken);

        if (!validator.IsValid)
        {
            return validator.ToErrorOr<IEnumerable<VCustomerActivities>>();
        }
        
        var rsp =await _customerActivityService.GetCustomerActivies(request.CustomerId, cancellationToken);

        if (rsp.IsSuccess && rsp.Data != null)
        {
            return ErrorOrFactory.From(rsp.Data);
        }

        return Error.NotFound(rsp.ErrMessage);
    }
}
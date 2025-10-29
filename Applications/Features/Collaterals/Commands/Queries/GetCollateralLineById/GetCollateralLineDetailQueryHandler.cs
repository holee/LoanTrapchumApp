using Applications.Commons;
using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses.Lists.Collaterals;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Collaterals.Commands.Queries.GetCollateralLineById;

public sealed class GetCollateralLineDetailQueryHandler : IRequestHandler<GetCollateralLineDetailQuery, ErrorOr<VCollateralLines>>
{
    private readonly ICustomerCollateralService _collateralRepository;

    private readonly IValidator<GetCollateralLineDetailQuery> _validator;
    public GetCollateralLineDetailQueryHandler(ICustomerCollateralService collateralRepository, IValidator<GetCollateralLineDetailQuery> validator)
    {
        _collateralRepository = collateralRepository; 

        _validator = validator;
    }
    public async Task<ErrorOr<VCollateralLines>> Handle(GetCollateralLineDetailQuery request, CancellationToken cancellationToken)
    {

        var validatorResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validatorResult.IsValid)
        {
            return validatorResult.ToErrorOr<VCollateralLines>();
        }

        var collateralLine = await _collateralRepository.GetCollateralLinesAsync(request.Id, request.CollateralId!);

        if (collateralLine is null)
        {
            return Error.NotFound("There is no any collateral.");
        }

        return collateralLine;

    }

}

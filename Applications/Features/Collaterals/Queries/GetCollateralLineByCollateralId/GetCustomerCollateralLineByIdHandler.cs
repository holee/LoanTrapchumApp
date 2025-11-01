using Applications.Commons;
using Applications.Extensions;
using Applications.Features.Collaterals.Dtos;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Collaterals.Queries.GetCollateralLineByCollateralId;

public sealed class GetCustomerCollateralLineByIdHandler : IRequestHandler<GetCollateralLineByCollateralIdQuery, ErrorOr<IEnumerable<VCollateralLines>>>
{

    private readonly ICollateralService _collateralsService;

    private readonly IValidator<GetCollateralLineByCollateralIdQuery> _validator;

    public GetCustomerCollateralLineByIdHandler(ICollateralService collateralsService, IValidator<GetCollateralLineByCollateralIdQuery> validator)
    {
        _collateralsService = collateralsService;
        _validator = validator;
    }

    public async Task<ErrorOr<IEnumerable<VCollateralLines>>> Handle(GetCollateralLineByCollateralIdQuery request, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validatorResult.IsValid)
        {
            return validatorResult.ToErrorOr<IEnumerable<VCollateralLines>>();
        }


        return await _collateralsService.GetCollateralLinesAsync(request.CollateralId);

      

    }
}

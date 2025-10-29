using Applications.Commons;
using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses.Lists.Collaterals;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Collaterals.Commands.Queries.GetCollateralLineByCollateralId;

public sealed class GetCustomerCollateralLineByIdHandler : IRequestHandler<GetCollateralLineByCollateralIdQuery, ErrorOr<IEnumerable<VCollateralLines>>>
{

    private readonly ICustomerCollateralService _collateralsService;

    private readonly IValidator<GetCollateralLineByCollateralIdQuery> _validator;

    public GetCustomerCollateralLineByIdHandler(ICustomerCollateralService collateralsService, IValidator<GetCollateralLineByCollateralIdQuery> validator)
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


        var rsp = await _collateralsService.GetCustomerCollateralLinesAsync(request.CollateralId);

        if (rsp.Data == null)

            return Error.NotFound(description: "No collateral lines found.");

        return ErrorOrFactory.From<IEnumerable<VCollateralLines>>(rsp.Data);

    }
}

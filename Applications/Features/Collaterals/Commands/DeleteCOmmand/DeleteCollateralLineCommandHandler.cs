using Applications.Bases;
using Applications.Extensions;
using Applications.Features.Collaterals.Commands.DeleteCOmmand;
using Applications.Interfaces.Customers;
using Applications.Responses.Lists.Collaterals;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Collaterals.Commands.CollateralLineQuery;

public sealed class DeleteCollateralLineCommandHandler : IRequestHandler<DeleteCollateralLineCommand, ErrorOr<DeleteResult>>
{
    private readonly ICustomerCollateralService _collateralRepository;

    private readonly IValidator<DeleteCollateralLineCommand> _validator;
    public DeleteCollateralLineCommandHandler(ICustomerCollateralService collateralRepository, IValidator<DeleteCollateralLineCommand> validator)
    {
        _collateralRepository = collateralRepository;

        _validator = validator;
    }
    public async Task<ErrorOr<DeleteResult>> Handle(DeleteCollateralLineCommand request, CancellationToken cancellationToken)
    {

        var validatorResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validatorResult.IsValid)
        {
            return validatorResult.ToErrorOr<DeleteResult>();
        }

        var rsp = await _collateralRepository.DeleteCollateralLineAsync(request.Id, request.CollateralId!);

        if (!rsp.IsSuccess)
        {
            return Error.Unexpected(rsp.Data!.Message ?? "Something went wrong,please contact your adminstrator");
        }

        return rsp.Data!;

    }

}

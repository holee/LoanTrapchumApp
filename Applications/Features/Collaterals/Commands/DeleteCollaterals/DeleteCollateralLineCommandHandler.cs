using Applications.Bases;
using Applications.Commons;
using Applications.Features.Collaterals.Commands.DeleteCOmmand;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Collaterals.Commands.CollateralLineQuery;

public sealed class DeleteCollateralLineCommandHandler : IRequestHandler<DeleteCollateralLineCommand, ErrorOr<DeleteResult>>
{
    private readonly ICollateralService _collateralRepository;

    private readonly IValidator<DeleteCollateralLineCommand> _validator;
    public DeleteCollateralLineCommandHandler(ICollateralService collateralRepository, IValidator<DeleteCollateralLineCommand> validator)
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

        return await _collateralRepository.DeleteCollateralLineAsync(request.Id, request.CollateralId!);

     

    }

}

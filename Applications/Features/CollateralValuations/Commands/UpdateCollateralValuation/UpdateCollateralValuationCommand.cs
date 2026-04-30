using Applications.Features.CollateralValuations.Dtos;
using Applications.Interfaces;
using Domains.Models.Collaterals;
using ErrorOr;
using Mapster;

namespace Applications.Features.CollateralValuations.Commands.UpdateCollateralValuation
{
    public class UpdateCollateralValuationCommand
    {
        public CollateralValuationDtoUpdate Request { get; set; } = new();
    }

    public class UpdateCollateralValuationCommandHandler(ICollateralValuationService service)
    {
        public async Task<ErrorOr<string>> Handle(UpdateCollateralValuationCommand command, CancellationToken cancellationToken = default)
        {
            var entity = command.Request.Adapt<CollateralEvaluations>();
            entity.ModifiedDT = DateTimeOffset.UtcNow;
            return await service.UpdateAsync(entity, cancellationToken);
        }
    }
}

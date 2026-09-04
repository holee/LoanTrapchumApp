using Application.Features.CollateralValuations.Dtos;
using Application.Interfaces;
using Domains.Models.Collaterals;
using ErrorOr;
using Mapster;

namespace Application.Features.CollateralValuations.Commands.CreateCollateralValuation
{
    public class CreateCollateralValuationCommand
    {
        public CollateralValuationDtoCreate Request { get; set; } = new();
    }

    public class CreateCollateralValuationCommandHandler(ICollateralValuationService service)
    {
        public async Task<ErrorOr<string>> Handle(CreateCollateralValuationCommand command, CancellationToken cancellationToken = default)
        {
            var entity = command.Request.Adapt<CollateralEvaluations>();
            entity.CreatedDT = DateTimeOffset.UtcNow;
            entity.ModifiedDT = DateTimeOffset.UtcNow;
            return await service.CreateAsync(entity, cancellationToken);
        }
    }
}

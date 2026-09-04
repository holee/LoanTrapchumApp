using Application.Interfaces;
using ErrorOr;

namespace Application.Features.CollateralValuations.Commands.DeleteCollateralValuation
{
    public class DeleteCollateralValuationCommand
    {
        public long Id { get; set; }
    }

    public class DeleteCollateralValuationCommandHandler(ICollateralValuationService service)
    {
        public async Task<ErrorOr<bool>> Handle(DeleteCollateralValuationCommand command, CancellationToken cancellationToken = default)
        {
            return await service.DeleteAsync(command.Id, cancellationToken);
        }
    }
}

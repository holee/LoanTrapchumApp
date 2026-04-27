using Applications.Features.AdditionalInformations.Dto;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.AdditionalInformations.Command.CreateSalutation;

public sealed class CreateSalutationCommandHandler(ISalutationService salutationService)
    : IRequestHandler<CreateSalutationCommand, ErrorOr<SalutationDto>>
{
    public async Task<ErrorOr<SalutationDto>> Handle(CreateSalutationCommand request,
        CancellationToken cancellationToken = default)
    {
        var model = new Salutation
        {
            Title = request.Request.Title
        };

        var result =await salutationService.AddOrUpdateAsync(model,cancellationToken);

        if (result is null) return Error.Failure(description: "Salutation create not created");

        return result;

    }
}
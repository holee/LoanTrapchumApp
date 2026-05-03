using Applications.Features.CollateralValuations.Commands.CreateCollateralValuation;
using Applications.Features.CollateralValuations.Dtos;
using Applications.Interfaces;

namespace TrapchumApp.Pages.Lists.CollateralValuations;

public partial class CollateralValuationCreateModal
{
    [Inject] private ICollateralValuationService Service { get; set; } = default!;

    [Parameter] public EventCallback<bool> OnClose { get; set; }

    private CollateralValuationDtoCreate Req { get; set; } = new();

    async Task Create()
    {
        var handler = new CreateCollateralValuationCommandHandler(Service);
        var result = await handler.Handle(new CreateCollateralValuationCommand { Request = Req });
        if (!result.IsError)
        {
            NotificationService.Notify(NotificationSeverity.Success, "Created", "Collateral valuation created successfully.");
            await OnClose.InvokeAsync(true);
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, "Error", result.FirstError.Description);
        }
    }

    async Task Close() => await OnClose.InvokeAsync(false);
}


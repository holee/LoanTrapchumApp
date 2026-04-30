using Applications.Features.CollateralValuations.Commands.UpdateCollateralValuation;
using Applications.Features.CollateralValuations.Dtos;
using Applications.Interfaces;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Lists.CollateralValuations;

public partial class CollateralValuationEditModal
{
    [Parameter] public long Id { get; set; }
    [Parameter] public EventCallback<bool> OnClose { get; set; }

    [Inject] private ICollateralValuationService Service { get; set; } = default!;

    private CollateralValuationDtoUpdate Req { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var result = await Service.GetByIdAsync(Id);
        if (!result.IsError)
        {
            var item = result.Value;
            Req = new CollateralValuationDtoUpdate
            {
                Id = item.Id,
                CollateralLineRefId = item.CollateralLineRefId,
                CollateralRefId = item.CollateralRefId,
                ValueDate = item.ValueDate,
                ValueBy = item.ValueBy,
                ValueAmount = item.ValueAmount,
                CurrencyRefId = item.CurrencyRefId,
            };
        }
        await base.OnInitializedAsync();
    }

    async Task Save()
    {
        var handler = new UpdateCollateralValuationCommandHandler(Service);
        var result = await handler.Handle(new UpdateCollateralValuationCommand { Request = Req });
        if (!result.IsError)
        {
            NotificationService.Notify(NotificationSeverity.Success, "Updated", "Collateral valuation updated successfully.");
            await OnClose.InvokeAsync(true);
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, "Error", result.FirstError.Description);
        }
    }

    async Task Close() => await OnClose.InvokeAsync(false);
}

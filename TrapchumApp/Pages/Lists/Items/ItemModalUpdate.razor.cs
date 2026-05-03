using Applications.Features.Items.Dto;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Transactions;
using Applications.Responses.ViewModels.Ledgers;

namespace TrapchumApp.Pages.Lists.Items;

public partial class ItemModalUpdate
{
    private readonly IItemService _itemService;
    private readonly ILedgerService _ledgerService;

    public ItemModalUpdate(IItemService itemService, ILedgerService ledgerService)
    {
        _itemService = itemService;
        _ledgerService = ledgerService;
    }

    [Parameter] public int Id { get; set; }

    private ItemDtoUpdate model = new();
    private IEnumerable<VLedger> Ledgers = [];

    protected override async Task OnInitializedAsync()
    {
        var ledgersRsp = await _ledgerService.GetLedgersAsync(ledgerId: 0, currencyId: 0);

        if (!ledgersRsp.IsError)
        {
            Ledgers = ledgersRsp.Value;
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Unable to load ledgers.");
        }

        var rsp = await _itemService.GetItemByIdAsync(Id);

        if (!rsp.IsError)
        {
            model = new ItemDtoUpdate
            {
                Id = rsp.Value.Id,
                ItemName = rsp.Value.ItemName,
                Description = rsp.Value.Description,
                LedgerRefId = rsp.Value.LedgerRefId,
                IsActive = rsp.Value.IsActive
            };
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
        }

        await base.OnInitializedAsync();
    }

    protected async Task Update()
    {
        var confirm = await DialogService.Confirm($"Do you want to update item ID#{Id}?", options: new ConfirmOptions
        {
            CancelButtonText = "Cancel",
            OkButtonText = "OK"
        });

        if (confirm != true)
        {
            return;
        }

        var rsp = await _itemService.UpdateAsync(model);

        if (!rsp.IsError)
        {
            NotificationService.Notify(NotificationSeverity.Success, "Item updated successfully.");
            DialogService.Close(true);
            return;
        }

        NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
    }

    private void Close()
    {
        DialogService.Close(false);
    }
}

using Applications.Features.Items.Dto;
using Applications.Features.Ledgers.Queries.GetLedgerByCurrency;
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
        await GetAllLedgers();

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
            _toastService.ShowError(rsp.GetMessage(), "Error");
        }

        await base.OnInitializedAsync();
    }

    private async Task GetAllLedgers()
    {
        var ledgersRsp = await mediator.Send(new GetLedgerByCurrencyQuery(0, 0, new(0, 0)));

        Ledgers = ledgersRsp.Items;
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

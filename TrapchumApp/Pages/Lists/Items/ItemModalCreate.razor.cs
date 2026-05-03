using Applications.Features.Items.Dto;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Transactions;
using Applications.Responses.ViewModels.Ledgers;

namespace TrapchumApp.Pages.Lists.Items;

public partial class ItemModalCreate
{
    private readonly IItemService _itemService;
    private readonly ILedgerService _ledgerService;

    public ItemModalCreate(IItemService itemService, ILedgerService ledgerService)
    {
        _itemService = itemService;
        _ledgerService = ledgerService;
    }

    private ItemDtoCreate model = new();
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

        await base.OnInitializedAsync();
    }

    protected async Task Create()
    {
        var rsp = await _itemService.CreateAsync(model);

        if (!rsp.IsError)
        {
            NotificationService.Notify(NotificationSeverity.Success, "Item created successfully.");
            DialogService.Close(rsp.Value);
            return;
        }

        NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
    }

    private void Close()
    {
        DialogService.Close(null);
    }
}

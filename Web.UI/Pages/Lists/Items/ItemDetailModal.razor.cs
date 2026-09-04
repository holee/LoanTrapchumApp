using Applications.Features.Items.Dto;
using Applications.Interfaces.Lists;

namespace Web.UI.Pages.Lists.Items;

public partial class ItemDetailModal
{
    private readonly IItemService _itemService;

    public ItemDetailModal(IItemService itemService)
    {
        _itemService = itemService;
    }

    [Parameter] public int Id { get; set; }

    private ItemDto? item;

    protected override async Task OnInitializedAsync()
    {
        var rsp = await _itemService.GetItemByIdAsync(Id);

        if (!rsp.IsError)
        {
            item = rsp.Value;
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
        }

        await base.OnInitializedAsync();
    }
}

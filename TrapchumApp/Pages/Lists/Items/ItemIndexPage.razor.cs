using Applications.Features.Items.Dto;
using Applications.Interfaces.Lists;

namespace TrapchumApp.Pages.Lists.Items;

public partial class ItemIndexPage
{
    private readonly IItemService _itemService;

    public ItemIndexPage(IItemService itemService)
    {
        _itemService = itemService;
    }

    private List<ItemDto> Items = [];
    private string? searchText = string.Empty;
    private bool includeInactive;
    private int pageIndex;
    private int pageSize = 15;
    private bool sortDescending;
    private string sortField = nameof(ItemDto.ItemName);
    private bool isLoading;
    private int counts;

    protected override async Task OnInitializedAsync()
    {
        await GetItems();
        await base.OnInitializedAsync();
    }

    protected async Task ShowCreate()
    {
        var result = await DialogService.OpenAsync<ItemModalCreate>("Create Item", options: new DialogOptions
        {
            Draggable = true,
            Resizable = true,
            Width = "620px"
        });

        if (result is ItemDto)
        {
            await GetItems(searchText);
        }
    }

    protected async Task Edit(int id)
    {
        var result = await DialogService.OpenAsync<ItemModalUpdate>("Edit Item",
            new Dictionary<string, object> { { "Id", id } },
            new DialogOptions
            {
                Draggable = true,
                Resizable = true,
                Width = "620px"
            });

        if (result is true)
        {
            await GetItems(searchText);
        }
    }

    protected async Task Details(int id)
    {
        await DialogService.OpenAsync<ItemDetailModal>("Item Details",
            new Dictionary<string, object> { { "Id", id } },
            new DialogOptions
            {
                Draggable = true,
                Width = "520px"
            });
    }

    protected async Task Delete(int id)
    {
        var confirm = await DialogService.Confirm("Do you want to delete this item?", "Delete", new ConfirmOptions
        {
            CancelButtonText = "Cancel",
            OkButtonText = "OK"
        });

        if (confirm != true)
        {
            return;
        }

        var rsp = await _itemService.DeleteAsync(id);

        if (!rsp.IsError)
        {
            NotificationService.Notify(NotificationSeverity.Success, rsp.Value);
            await GetItems(searchText);
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
        }
    }

    protected async Task SearchChanged(string filter)
    {
        searchText = filter;
        await GetItems(filter);
    }

    protected async Task ShowInactiveAndActive(bool value)
    {
        includeInactive = value;
        await GetItems(searchText);
    }

    private async Task LoadItemData(LoadDataArgs args)
    {
        pageIndex = args.Skip ?? 0;

        if (args.OrderBy.Any())
        {
            sortField = args.Sorts.First().Property;
            sortDescending = args.Sorts.First().SortOrder == SortOrder.Descending;
        }

        var filterColumns = args.FilterValue(nameof(ItemDto.ItemName), nameof(ItemDto.Description));
        var filterString = filterColumns ?? searchText;

        await GetItems(filterString);
    }

    private async Task GetItems(string? filter = null)
    {
        isLoading = true;

        var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filter);

        var rsp = await _itemService.GetItemsAsync(param);

        counts = rsp.Count;
        Items = rsp.Items ?? [];

        isLoading = false;
        StateHasChanged();
    }
}

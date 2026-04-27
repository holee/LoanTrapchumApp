using Applications.Extensions;
using Applications.Features.Addresses.Dto;
using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Commands.ChangeVendorStatus;
using Applications.Features.Vendors.Commands.DeleteVendor;
using Applications.Features.Vendors.Dto.Responses;
using Applications.Features.Vendors.Queries.GetVendorsAsPaging;
using TrapchumApp.Components.Messageboxes;

namespace TrapchumApp.Pages.Lists.Vendors;

public partial class VendorIndexPage
{
    [Parameter] public int selectedIndex { get; set; } = 0;
    private string? filter;
    private List<VendorDto> Vendors = [];
    private int pageIndex = 0;
    private int pageSize = 15;
    private int counts = 0;
    private string sortField = "VendorId";
    private bool sortDescending = false;
    private bool includeInactive = false;
    private bool isLoading = false;
    private List<CustomTab> _dynamicTabs = [];
    protected override async Task OnInitializedAsync()
    {

        await GetVendors();

        await base.OnInitializedAsync();
    }

    protected async Task SearchChanged(string filter)
    {
        this.filter = filter;

        await GetVendors(filter);
    }

    async Task PageSizeChanged(int page)
    {
        pageSize = page;

        pageIndex = 0;

        await GetVendors(filter);
    }

    async Task GetVendors(string? filterText = "")
    {
        isLoading = true;

        var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filterText);

        var resp = await mediator.Send(new GetVendorsAsPagingQuery(param));

        counts = resp.Count;

        Vendors = resp.Items;

        isLoading = false;

        StateHasChanged();
    }

    async Task LoadData(LoadDataArgs e)
    {
        pageIndex = e.Skip!.Value;
        pageSize = e.Top ?? 15;
        (sortField, sortDescending) = e.PageInfo();

        var filterColumn = e.Filters?.FirstOrDefault(pr =>
                               pr.Property == nameof(VendorDto.AltName) ||
                               pr.Property == nameof(VendorDto.VendorId) ||
                               pr.Property == nameof(VendorDto.FirstName) ||
                               pr.Property == nameof(VendorDto.Salutation) ||
                               pr.Property == nameof(VendorDto.LastName) ||
                               pr.Property == nameof(VendorDto.Phone) ||
                               pr.Property == nameof(VendorDto.EmailAddress))?.FilterValue!.ToString() ?? string.Empty;

        filter = filterColumn ?? string.Empty;

        await GetVendors(filter);
    }

    protected async Task IncludeInActiveChanged(bool e)
    {
        includeInactive = e;
        await GetVendors(filter);
    }

    // ── Dynamic Tab Management ──

    void AddNewTab()
    {
        var tabId = Guid.NewGuid().ToString("N");

        RenderFragment content = builder =>
        {
            builder.OpenComponent(0, typeof(VendorCreatePage));
            builder.AddAttribute(1, "Closed", EventCallback.Factory.Create(this, _ => CloseTab(tabId)));
            builder.SetKey(tabId);
            builder.CloseComponent();
        };

        var tab = new CustomTab(tabId, "New Vendor", TabActions.Create, content);

        _dynamicTabs.Add(tab);

        selectedIndex = _dynamicTabs.Count;

        StateHasChanged();
    }

    void OpenDetailTab(string? id, string? name = null)
    {
        ContextMenuService.Close();

        string newKey = $"details_{id}";

        if (!id.IsNotNullOrEmpty()) return;

        var existing = _dynamicTabs.FindIndex(t => t.Key == newKey && t.TabAction == TabActions.Details);

        if (existing >= 0)
        {
            selectedIndex = existing + 1;

            StateHasChanged();

            return;
        }

        RenderFragment content = builder =>
        {
            builder.OpenComponent(0, typeof(VendorDetailPage));
            builder.AddAttribute(1, "Id", id);
            builder.AddAttribute(2, "Closed", EventCallback.Factory.Create(this, _ => CloseTab(newKey)));
            builder.AddAttribute(3, "OnEdit", EventCallback.Factory.Create<string>(this, editId => OpenEditTab(editId)));
            builder.SetKey(newKey);
            builder.CloseComponent();
        };

        var tab = new CustomTab(newKey, name ?? "Details", TabActions.Details, content);

        _dynamicTabs.Add(tab);

        selectedIndex = _dynamicTabs.Count;

        StateHasChanged();
    }

    void OpenEditTab(string? id, string? name = null)
    {
        ContextMenuService.Close();

        if (!id.IsNotNullOrEmpty()) return;

        string newKey = $"edit_{id}";

        var existing = _dynamicTabs.FindIndex(t => t.Key == newKey && t.TabAction == TabActions.Edit);

        if (existing >= 0)
        {
            selectedIndex = existing + 1;

            StateHasChanged();

            return;
        }

        RenderFragment content = builder =>
        {
            builder.OpenComponent(0, typeof(VendorEditPage));
            builder.AddAttribute(1, "Id", id);
            builder.AddAttribute(2, "Closed", EventCallback.Factory.Create(this, async () => CloseTab(newKey)));
            builder.SetKey(newKey);
            builder.CloseComponent();
        };

        var tab = new CustomTab(newKey, $"Edit {name ?? ""}".Trim(), TabActions.Edit, content);

        _dynamicTabs.Add(tab);

        selectedIndex = _dynamicTabs.Count;

        StateHasChanged();
    }

    void CloseTab(string? tabId)
    {
        var tab = _dynamicTabs.FirstOrDefault(t => t.Key == tabId);

        if (tab is null) return;

        var idx = _dynamicTabs.IndexOf(tab);

        _dynamicTabs.Remove(tab);

        selectedIndex = Math.Max(0, idx);

        StateHasChanged();
    }


    // ── Delete & Status ──

    async Task OnDelete(string? id)
    {
        ContextMenuService.Close();

        var dia = await MsgBox.ShowAsync("Vendor deleted will not be able to recover, would you like to continue?", "Confirm", MsgBoxButtons.YesNo, MessageBoxIcon.Question);

        if (dia == MsgBoxResult.No) return;

        var command = new DeleteVendorCommand(id);

        var result = await mediator.Send(command); //_otherNameService!.DeleteAsync(id);

        if (result.IsSuccess())
        {
            NotificationService.Notify(NotificationSeverity.Success, "Delete Vendor", "Vendor was deleted successfully!");

            await GetVendors(filter);
        }
    }

    protected async Task DeactiveOrActive(bool arg, string vendorId)
    {

        var dia = await MsgBox.ShowAsync($"Do you want to change vendor ID #{vendorId} status?", "Confirm", MsgBoxButtons.YesNo, MessageBoxIcon.Question);

        if (dia == MsgBoxResult.No) return;

        var rsp = await mediator.Send(new ChangeVendorStatusCommand(vendorId, arg));

        if (rsp.IsSuccess())
        {
            _toastService.ShowSuccess("Vendor status changed successfully!");

            await GetVendors(filter);
        }
        else
        {
            _toastService.ShowError(rsp.GetMessage());
        }
    }
}

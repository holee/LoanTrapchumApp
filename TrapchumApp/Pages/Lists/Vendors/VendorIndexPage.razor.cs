using Applications.Extensions;
using Applications.Features.Addresses.Dto;
using Applications.Features.OtherNames.Dto;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Vendors;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.Vendors;

public enum VendorTabType { Create, Edit, Detail }

public record VendorTab(string Id, string Title, string Icon, VendorTabType Type, string? EntityId = null);

public partial class VendorIndexPage
{
    readonly IVendorServiceImpl _vendorService;
    readonly IOtherNameService _otherNameService;

    public VendorIndexPage(IVendorServiceImpl vendorService, IOtherNameService otherNameService)
    {
        _vendorService = vendorService;
        _otherNameService = otherNameService;
    }

    [Parameter] public int selectedIndex { get; set; } = 0;

    string? filter;
    IEnumerable<ProvinceDto>? ProvinceList = [];
    EntNameDtoUpdate? model = new();
    EntNameDto? OtherNames = new();
    List<VendorVm> Vendors = [];
    int pageIndex = 0;
    int pageSize = 15;
    int counts = 0;
    string sortField = "VendorId";
    bool sortDescending = false;
    bool includeInactive = false;
    bool isLoading = false;

    List<VendorTab> _dynamicTabs = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await GetVendors();
        }
        catch (Exception ex)
        {
            await DialogService.Alert(ex.Message, "Exception");
        }
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
        var resp = await _vendorService.GetVendorsAsync(param);
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
                               pr.Property == nameof(VendorVm.AltName) ||
                               pr.Property == nameof(VendorVm.VendorId) ||
                               pr.Property == nameof(VendorVm.FirstName) ||
                               pr.Property == nameof(VendorVm.Salutation) ||
                               pr.Property == nameof(VendorVm.LastName) ||
                               pr.Property == nameof(VendorVm.Phone) ||
                               pr.Property == nameof(VendorVm.EmailAddress))?.FilterValue.ToString() ?? string.Empty;

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
        var tab = new VendorTab(
            Id: Guid.NewGuid().ToString("N"),
            Title: "New Vendor",
            Icon: "person_add",
            Type: VendorTabType.Create);

        _dynamicTabs.Add(tab);
        selectedIndex = _dynamicTabs.Count;
        StateHasChanged();
    }

    void OpenDetailTab(string? id, string? name = null)
    {
        ContextMenuService.Close();
        if (!id.IsNotNullOrEmpty()) return;

        var existing = _dynamicTabs.FindIndex(t => t.EntityId == id && t.Type == VendorTabType.Detail);
        if (existing >= 0)
        {
            selectedIndex = existing + 1;
            StateHasChanged();
            return;
        }

        var tab = new VendorTab(
            Id: Guid.NewGuid().ToString("N"),
            Title: name ?? "Details",
            Icon: "info",
            Type: VendorTabType.Detail,
            EntityId: id);

        _dynamicTabs.Add(tab);
        selectedIndex = _dynamicTabs.Count;
        StateHasChanged();
    }

    void OpenEditTab(string? id, string? name = null)
    {
        ContextMenuService.Close();
        if (!id.IsNotNullOrEmpty()) return;

        var existing = _dynamicTabs.FindIndex(t => t.EntityId == id && t.Type == VendorTabType.Edit);
        if (existing >= 0)
        {
            selectedIndex = existing + 1;
            StateHasChanged();
            return;
        }

        var tab = new VendorTab(
            Id: Guid.NewGuid().ToString("N"),
            Title: $"Edit {name ?? ""}".Trim(),
            Icon: "edit",
            Type: VendorTabType.Edit,
            EntityId: id);

        _dynamicTabs.Add(tab);
        selectedIndex = _dynamicTabs.Count;
        StateHasChanged();
    }

    void CloseTab(string tabId)
    {
        var idx = _dynamicTabs.FindIndex(t => t.Id == tabId);
        if (idx < 0) return;

        _dynamicTabs.RemoveAt(idx);
        selectedIndex = 0;
        StateHasChanged();
    }

    async Task OnDynamicEditClose(string tabId, bool isEdited)
    {
        CloseTab(tabId);
        if (isEdited)
        {
            await GetVendors(filter);
        }
    }

    // ── Delete & Status ──

    async Task OnDelete(string? id)
    {
        ContextMenuService.Close();
        var confirm = (bool)await DialogService.Confirm(
            "Vendor deleted will not be able to recover, would you like to continue?",
            "Delete Vendor",
            new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });

        if (!confirm) return;

        var result = await _otherNameService!.DeleteAsync(id);

        if (result.IsSuccess())
        {
            NotificationService.Notify(NotificationSeverity.Success, "Delete Vendor", "Vendor was deleted successfully!");
            await GetVendors(filter);
        }
    }

    protected async Task DeactiveOrActive(bool arg, string vendorId)
    {
        var confirm = await DialogService.Confirm(
            $"Do you want to change vendor ID #{vendorId} status?", "Confirm",
            options: new ConfirmOptions { CancelButtonText = "Cancel", OkButtonText = "Yes" });

        if (confirm == false) return;

        var rsp = await _vendorService.ChangeVendorStatus(vendorId, arg);

        if (rsp.IsSuccess)
        {
            NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            await GetVendors(filter);
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
        }
    }
}

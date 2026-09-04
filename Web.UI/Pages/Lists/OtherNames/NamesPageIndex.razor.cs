using Applications.Extensions;
using Applications.Responses.ViewModels.OtherNames;
using Radzen.Blazor;
using Applications.Features.Addresses.Dto;
using Applications.Features.OtherNames.Dto;
using Applications.Features.OtherNames.Commands.DeleteOtherName;
using Applications.Features.OtherNames.Queries.GetOtherNamesAsPaging;

namespace Web.UI.Pages.Lists.OtherNames;

public enum DynamicTabType { Create, Edit, Detail }

public record DynamicTab(string Id, string Title, string Icon, DynamicTabType Type, string? EntityId = null);

public partial class NamesPageIndex
{

    List<VOtherNames> Names = [];
    int pageIndex = 0;
    int pageSize = 15;
    int counts = 0;
    string sortField = "EntId";
    bool sortDescending = false;
    bool includeInactive = false;
    RadzenTabs? tabs;
    [Parameter] public int selectedIndex { get; set; } = 0;
    protected IEnumerable<string> Salutations = [];
    string? q;
    protected IEnumerable<ProvinceDto>? ProvinceList = [];
    protected EntityNamesCreate? modelEdit = new();
    bool isLoading = false;

    List<DynamicTab> _dynamicTabs = [];
    protected override async Task OnInitializedAsync()
    {

        await GetOtherNames();

        await base.OnInitializedAsync();
    }

    protected async Task SearchChanged(string filter)
    {
        q = filter;
        await GetOtherNames(filter);
    }

    async Task PageSizeChanged(int page)
    {
        pageSize = page;
        await GetOtherNames(q);
    }

    async Task GetOtherNames(string? filterText = "")
    {
        isLoading = true;

        var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filterText);

        var command = new GetOtherNamesAsPagingQuery(param, null);

        var resp = await mediator.Send(command);   //_otherNameService.GetNamesAsync(param);

        counts = resp.Count;

        Names = resp.Items;

        isLoading = false;

        StateHasChanged();
    }

    async Task LoadData(LoadDataArgs e)
    {
        pageIndex = e.Skip!.Value;
        (sortField, sortDescending) = e.PageInfo();

        var filterColunm = e.Filters?.FirstOrDefault(p =>
                                p.Property == nameof(VOtherNames.AltName) ||
                                p.Property == nameof(VOtherNames.Phone) ||
                                p.Property == nameof(VOtherNames.EmailAddress) ||
                                p.Property == nameof(VOtherNames.FirstName) ||
                                p.Property == nameof(VOtherNames.Gender) ||
                                p.Property == nameof(VOtherNames.LastName)
                            )?.FilterValue!.ToString() ?? string.Empty;

        q = filterColunm ?? string.Empty;
        await GetOtherNames(q);
    }

    protected async Task IncludeInActiveChanged(bool e)
    {
        includeInactive = e;
        await GetOtherNames(q);
    }

    //private async Task GetSalutations()
    //{
    //    var salutations = await _salutationService.GetSalutions();
    //    if (salutations.IsSuccess)
    //    {
    //        Salutations = salutations!.Data!.Select(x => x.Title!).ToList();
    //    }
    //    else
    //    {
    //        Salutations = [];
    //    }
    //}

    // ── Dynamic Tab Management ──

    void AddTabOtherName()
    {
        var tab = new DynamicTab(
            Id: Guid.NewGuid().ToString("N"),
            Title: "New Name",
            Icon: "person_add",
            Type: DynamicTabType.Create);

        _dynamicTabs.Add(tab);
        selectedIndex = _dynamicTabs.Count;
        StateHasChanged();
    }

    void OpenDetailTab(string? id, string? name = null)
    {
        ContextMenuService.Close();
        if (!id.IsNotNullOrEmpty()) return;

        var existing = _dynamicTabs.FindIndex(t => t.EntityId == id && t.Type == DynamicTabType.Detail);
        if (existing >= 0)
        {
            selectedIndex = existing + 1;
            StateHasChanged();
            return;
        }

        var tab = new DynamicTab(
            Id: Guid.NewGuid().ToString("N"),
            Title: name ?? "Details",
            Icon: "info",
            Type: DynamicTabType.Detail,
            EntityId: id);

        _dynamicTabs.Add(tab);
        selectedIndex = _dynamicTabs.Count;
        StateHasChanged();
    }

    void OpenEditTab(string? id, string? name = null)
    {
        ContextMenuService.Close();
        if (!id.IsNotNullOrEmpty()) return;

        var existing = _dynamicTabs.FindIndex(t => t.EntityId == id && t.Type == DynamicTabType.Edit);
        if (existing >= 0)
        {
            selectedIndex = existing + 1;
            StateHasChanged();
            return;
        }

        var tab = new DynamicTab(
            Id: Guid.NewGuid().ToString("N"),
            Title: $"Edit {name ?? ""}".Trim(),
            Icon: "edit",
            Type: DynamicTabType.Edit,
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
            await GetOtherNames(q);
        }
    }

    // ── Existing actions (Convert, Delete, Status) ──

    protected async Task ConvertToCustomer(string? id)
    {
        ContextMenuService.Close();
        var options = new DialogOptions { Width = "53rem", Draggable = true };
        bool? dialogueResult = await DialogService.OpenAsync<ConvertToCustomer>("Customer", new Dictionary<string, object?>() { { "Id", id } }, options);
        if (dialogueResult is not null && dialogueResult == true)
        {
            await GetOtherNames(q);
        }
    }

    protected async Task ConvertToVedor(string? id)
    {
        ContextMenuService.Close();
        var options = new DialogOptions { Width = "43rem", Draggable = true };
        bool? dialogueResult = await DialogService.OpenAsync<ConvertToVendor>("Vendor", new Dictionary<string, object?>() { { "Id", id } }, options);
        if (dialogueResult is not null && dialogueResult == true)
        {
            await GetOtherNames(q);
        }
    }

    protected async Task ConvertToEmployee(string? id)
    {
        ContextMenuService.Close();
        var options = new DialogOptions { Width = "43rem", Draggable = true };
        bool? rsp = await DialogService.OpenAsync<ConvertToEmployee>("Convert to Employee", new Dictionary<string, object?>() { { "Id", id } }, options);
        if (rsp is bool result && result == true)
        {
            await GetOtherNames(q);
        }
    }

    protected EntNameDto? OtherNames = new();

    async Task OnDelete(string? id)
    {

        var confirm = await MsgBox.ShowAsync("Other Name deleted will not be able to recover, would you like to continue?", "Delete Other Name", MsgBoxButtons.YesNo, MsgBoxBoxIcon.Question);

        if (confirm == MsgBoxResult.Yes)
        {

            var command = new DeleteOtherNameCommand(id);

            var result = await mediator.Send(command); //   _otherNameService!.DeleteAsync(id);

            if (result.IsSuccess())
            {

                _toastService.ShowSuccess("Other Name was deleted successful!");

                ContextMenuService.Close();

                return;
            }
        }
        else
        {
            Console.WriteLine("Canceled");

            ContextMenuService.Close();
        }
    }

    private async Task OnChanged(bool args, string? id)
    {
        var confirm = await DialogService.Confirm("Do you want enable or disable?", "Confirm", new ConfirmOptions
        {
            OkButtonText = "Yes",
            CancelButtonText = "Cancel"
        });

        if (confirm == false) return;

        //var rsp = await _otherNameService.UpdateStatusAsync(id, args);

        //if (rsp.IsSuccess)
        //{
        await GetOtherNames(q);

        return;
        // }

        //NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
    }
}

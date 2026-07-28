using Applications.Bases;
using Applications.Extensions;
using Applications.Extensions.Attrs;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.Employees.Commands.ChangeEmployeeState;
using Applications.Features.Employees.Commands.DeleteEmployee;
using Applications.Features.Employees.Queries.GetEmployeeAsPaging;
using Applications.Interfaces.Lists;
using Applications.Responses.Lists.Employees;
using Applications.Responses.ViewModels.Employees;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Employees;

public partial class EmployeesIndex(ILogger<EmployeesIndex> logger)
{

    [PersistentState] public List<VEmployeeList>? Employees { get; set; }

    private int selectedIndex = 0;
    private string? _lastEmployeeId = string.Empty;
    List<VBranch> Branches = [];
    int pageSize = 15;
    int counts = 0;
    int pageIndex = 0;
    string sortField = nameof(VEmployeeList.FirstName);
    bool sortDescending = true;
    string? _empId = string.Empty;
    int? branchId = 0;
    public EmployeeDto? employee;

    string searchText = string.Empty;

    protected bool includeInActive = false;

    bool isLoading = false;
    bool _pendingReload = false;

    private RadzenDataGrid<VEmployeeList> _gridEmployee = default!;

    private List<CustomTab> _tabs = [];

    string? filter = string.Empty;

    IList<VEmployeeList> SelectedEmployees = [];


    void OnCellRender(RowRenderEventArgs<VEmployeeList> e)
    {
        if (e.Data!.EmployeeId == _lastEmployeeId)
        {
            e.Attributes.Add("class", "rz-state-highlight rz-data-row recently-edited");
        }
    }


    private void OnRowSelected(VEmployeeList args)
    {

    }

    void OnRowRender(RowRenderEventArgs<VEmployeeList> args)
    {
        if(args.Data is null) return;

        var isSearchMatch = !string.IsNullOrWhiteSpace(searchText) &&
            (args.Data!.AltName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
             args.Data!.FirstName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
              args.Data!.LastName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
             args.Data!.Phone?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
             args.Data!.EmailAddress?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
             args.Data!.DepartmentName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
             args.Data!.Occupation?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
             args.Data!.BranchName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true);

        if (isSearchMatch)
        {
            args.Attributes.Add("class", "rz-state-highlight rz-data-row recently-edited");
        }
        else if (args.Data!.ModifiedDT.IsRecently())
        {
            args.Attributes.Add("class", "rz-state-highlight rz-data-row recently-edited");
        }

        args.Data.RowIndex = args.Index + 1; // store 1-based index
    }

    async Task PageSizeChanged(int page)
    {
        pageSize = page;

        pageIndex = 0;

        _pendingReload = true;

        await _gridEmployee.Reload();
    }


    async Task OnCheckChanged(string? employeeId, bool state)
    {

        var confirm = await DialogService.Confirm($"Do you want to Deactivate/Activate Employee with ID#{branchId}?", options: new ConfirmOptions
        {
            CancelButtonText = "Cancel",

            OkButtonText = "OK",
        });

        if (confirm == false) return;

        var command = new ChangeEmployeeStateCommand(employeeId, state);

        var rsp = await mediator.Send(command);

        if (!rsp.IsSuccess())
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());

            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Employee was changed");

        var foundIndex = Employees!.FindIndex(e => e.EmployeeId == rsp.Value.EmployeeId);

        if (foundIndex != -1)
        {
            Employees[foundIndex] = rsp.Value;
        }

        await _gridEmployee.Reload();

    }

    async Task GetBranches()
    {


        var command = new GetBranchAsPagingQuery(0, new QueryParam(0, 0, nameof(VBranch.BranchName)));


        var results = await mediator.Send(command);

        var unionBranches = new List<VBranch>
        {
            new VBranch
            {
                BranchId = 0,
                BranchName = "All"
            }
        };


        var branches = results.Items.Union(unionBranches);

        Branches = branches.OrderBy(x => x.BranchId).ToList();

    }

    void CreateNewEmployeeTab()
    {

        var newTabId = Guid.NewGuid().ToString("N");

        RenderFragment childContent = builer =>
        {
            builer.OpenComponent(0, typeof(EmployeeCreatePage));

            builer.AddAttribute(1, "OnClose", EventCallback.Factory.Create(this, () =>
            {
                CloseTab(newTabId);

            }));

            builer.AddAttribute(3, "UpdateStateChanged", EventCallback.Factory.Create(this, async (VEmployeeList x) =>
            {
                await ReloadAfterChange(x.EmployeeId);
            }));


            builer.SetKey(newTabId);

            builer.CloseComponent();
        };

        var newTab = new CustomTab
        {
            Key = newTabId,
            Title = "New Employee",
            TabAction = TabActions.Create,
            Content = childContent,
        };

        _tabs.Add(newTab);

        selectedIndex = _tabs.Count;

        StateHasChanged();
    }


    void AddEditTab(string? id)
    {

        ContextMenuService.Close();

        var editTabId = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString("N") : $"{id}_edit";

        var existingTabIndex = _tabs.FindIndex(t => t.Key == editTabId);

        if (existingTabIndex != -1)
        {
            selectedIndex = existingTabIndex + 1;
            return;
        }

        RenderFragment childContent = builer =>
        {
            builer.OpenComponent(0, typeof(EmployeeEditPage));

            builer.AddAttribute(1, "EmployeeId", id);

            builer.AddAttribute(2, "OnClose", EventCallback.Factory.Create(this, () =>
            {
                CloseTab(editTabId);
            }));

            builer.AddAttribute(4, "UpdateChanged", EventCallback.Factory.Create(this, async (VEmployeeList x) =>
            {

                CloseTab(editTabId);

                await ReloadAfterChange(x.EmployeeId);

            }));

            builer.SetKey(editTabId);

            builer.CloseComponent();
        };


        var newTab = new CustomTab
        {
            Key = editTabId,

            Title = "Edit",

            TabAction = TabActions.Edit,

            Content = childContent,
        };

        _tabs.Add(newTab);

        selectedIndex = _tabs.Count;

        StateHasChanged();
    }


    void AddDetailsTab(string? id)
    {
        ContextMenuService.Close();

        var existingTabIndex = _tabs.FindIndex(t => t.Key == id);

        if (existingTabIndex != -1)
        {
            selectedIndex = existingTabIndex + 1;

            StateHasChanged();

            return;
        }

        RenderFragment childContent = builer =>
        {
            builer.OpenComponent(0, typeof(EmployeeDetails));
            builer.AddAttribute(1, "BranchId", branchId);
            builer.AddAttribute(2, "EmployeeId", id);
            builer.AddAttribute(3, "Close", EventCallback.Factory.Create(this, () =>
            {
                CloseTab(id);
            }));

            builer.AddAttribute(4, "OnEdit", EventCallback.Factory.Create(this, async (string x) =>
            {
                AddEditTab(x);
            }));


            builer.SetKey(id);

            builer.CloseComponent();

        };

        var newTab = new CustomTab
        {
            Key = id ?? string.Empty,

            Title = "Details",

            TabAction = TabActions.Details,

            Content = childContent,
        };

        _tabs.Add(newTab);

        selectedIndex = _tabs.Count;

        StateHasChanged();
    }



    private void CloseTab(string? id)
    {
        var tab = _tabs.FirstOrDefault(t => t.Key == id);

        if (tab != null)
        {
            var idx = _tabs.IndexOf(tab);

            _tabs.Remove(tab);

            selectedIndex = Math.Max(0, idx);
        }

    }


    void TabChanged(int e)
    {
        selectedIndex = e;

        StateHasChanged();
    }

    async void GetIncludeInactive()
    {
        await GetEmployees(branchId, searchText);
    }

    protected async Task PerformSearch(string? searchtext = null)
    {
        searchText = searchtext ?? string.Empty;

        await GetEmployees(branchId, searchText);
    }

    protected async Task Reload()
    {
        await GetEmployees(branchId, searchText);
    }

    protected override async Task OnInitializedAsync()
    {
        Employees = null;
        _lastEmployeeId = null;

        await GetBranches();

        await GetEmployees(branchId);

    }

    async Task DeleteEmployee(string? employeeId)
    {
        ContextMenuService.Close();

        var dia = await MsgBox.ConfirmAsync("Deleted Employee will not be able to recover, would you like to continue?", "Confirm Delete");

        if (dia == MsgBoxResult.No) return;

        try
        {

            var command = new DeleteEmployeeCommand(employeeId, branchId);

            var rsp = await mediator.Send(command);// _employeeService!.DeleteAsync(employeeId);

            if (rsp.Value.Status == DeleteStatus.Success)
            {
                await GetEmployees(branchId, searchText);

                _toastService.ShowSuccess("Employee was deleted successfully");

                return;
            }

            if (rsp.Value.Status == DeleteStatus.InUse)
            {

                var confirm = await MsgBox.ConfirmAsync("This employee is currently in use. Do you want to deactivate this employee instead?", "Employee In Use");

                if (confirm == MsgBoxResult.Yes)
                {
                    var changeStateCommand = new ChangeEmployeeStateCommand(employeeId, false);

                    var changeStateRsp = await mediator.Send(changeStateCommand);

                    if (changeStateRsp.IsSuccess())
                    {
                        await GetEmployees(branchId, searchText);

                        _toastService.ShowSuccess("Employee was deactivated successfully");

                        return;
                    }

                    _toastService.ShowError($"An error occurred while deactivating employee : {changeStateRsp.GetMessage()}");

                }

            }

        }
        catch (Exception ex)
        {
            _toastService.ShowError($"An error occurred while deleting employee : {ex.Message}");
        }
    }


    async Task GetEmployees(int? branchId, string? q = null)
    {

        Tracker.Show();

        logger.LogInformation("try to getting employee information");

        try
        {
            var param = new QueryParam(pageIndex, pageSize, sortField, includeInActive, sortDescending, q);

            var query = new GetEmployeeAsPagingQuery(branchId, null, param);

            var pageDate = await mediator.Send(query);

            counts = pageDate.Count;

            Employees = pageDate.Items;

            isLoading = false;

            Tracker.Hide();
        }
        catch (Exception ex)
        {

            Tracker.Hide();

            logger.LogError(ex, "Error while getting employee information {error}", ex.Message);
        }

        StateHasChanged();
    }

    private async Task ReloadAfterChange(string? employeeId)
    {
        _lastEmployeeId = employeeId;

        _pendingReload = true;

        pageIndex = 0;

        sortField = nameof(VEmployeeList.ModifiedDT);

        sortDescending = true;

        await _gridEmployee.FirstPage(true);
    }

    async Task LoadData(LoadDataArgs e)
    {

        pageIndex = e.Skip ?? 0;

        pageSize = e.Top ?? 15;

        if (_pendingReload)
        {
            _pendingReload = false;
        }
        else
        {
            (sortField, sortDescending) = e.PageInfo(nameof(VEmployeeList.FirstName));
        }

        var filterValue =e.FilterValue(nameof(VEmployeeList.Phone),
                                           nameof(VEmployeeList.EmailAddress),
                                           nameof(VEmployeeList.DepartmentName),
                                           nameof(VEmployeeList.Occupation),
                                           nameof(VEmployeeList.BranchName));

        searchText = filterValue ?? string.Empty;

        await GetEmployees(branchId, searchText);
    }


    private async Task SelectedItemChanged(object? arg)
    {
        if (arg is not null)
        {
            var branchId = ((VBranch)arg).BranchId;

            this.branchId = branchId;

            await GetEmployees(branchId, searchText);
        }
    }
    private void OnRowDoubleClick(DataGridRowMouseEventArgs<VEmployeeList> args)
    {
        AddDetailsTab(args.Data?.EmployeeId);
        
        tabCount++;
    }
}
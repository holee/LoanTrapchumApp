using Applications.Extensions.DateTimes;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.Employees.Commands.ChangeEmployeeState;
using Applications.Features.Employees.Queries.GetEmployeeAsPaging;
using Applications.Interfaces.Lists;
using Applications.Responses.Lists.Employees;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Radzen.Blazor;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.Employees
{
    public partial class EmployeesIndex
    {

        private int selectedIndex = 0;
        private readonly INationalityService _nationalityService;
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<EmployeesIndex> _logger;
        private string? _lastEmployeeId = string.Empty;
        List<VBranch> Branches = [];
        int pageSize = 15;
        int counts = 0;
        int pageIndex = 1;
        string sortField = nameof(VEmployeeList.ModifiedDT);
        bool sortDescending = true;
        string? _empId = string.Empty;
        int branchId = 0;
        public EmployeeDto? employee;

        string searchText = string.Empty;

        [PersistentState(AllowUpdates = true)]
        public List<VEmployeeList> Employees { get; set; }

        protected bool includeInActive = false;

        bool isLoading = false;

        private RadzenDataGrid<VEmployeeList> _gridEmployee = default!;
        public EmployeesIndex(INationalityService nationalityService, IDepartmentService departmentService, ILogger<EmployeesIndex> logger)
        {
            _nationalityService = nationalityService;
            _departmentService = departmentService;
            _logger = logger;
        }
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

            if (args.Data!.ModifiedDT.IsRecently())
            {
                args.Attributes.Add("class", "rz-state-highlight rz-data-row recently-edited");

            }

            args.Data.RowIndex = args.Index + 1; // store 1-based index
        }

        async Task PageSizeChanged(int page)
        {
            pageSize = page;

            pageIndex = 0;

            await GetEmployees(branchId, filter);
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

            var foundIndex = Employees.FindIndex(e => e.EmployeeId == rsp.Value.EmployeeId);

            if (foundIndex != -1)
            {
                Employees[foundIndex] = rsp.Value;
            }

            await _gridEmployee.Reload();

        }

        async Task GetBranches()
        {


            var command = new GetBranchAsPagingQuery(0, new QueryParam(0, 0));


            var results = await mediator.Send(command);

            var unionBranches = new List<VBranch>
            {
                new VBranch
                {
                    BranchId = 0,
                    BranchName = "All"
                }
            };

            if (results.IsSuccess)
            {
                var branches = results.Items.Union(unionBranches);

                Branches = branches.OrderBy(x => x.BranchId).ToList();
            }
        }

        void CreateNewEmployeeTab()
        {

            var newTabId = Guid.NewGuid().ToString("N");

            RenderFragment childContent = builer =>
            {
                builer.OpenComponent(0, typeof(CreateEmployee));

                builer.AddAttribute(1, "OnClose", EventCallback.Factory.Create(this, () =>
                {
                    CloseTab(newTabId);

                }));

                builer.AddAttribute(3, "UpdateStateChanged", EventCallback.Factory.Create(this, async (VEmployeeList x) =>
                {
                    _lastEmployeeId = x.EmployeeId;
                    await _gridEmployee.Reload();
                }));


                builer.SetKey(newTabId);
                builer.CloseComponent();
            };

            var newTab = new CustomTab
            {
                Key = newTabId,
                Title = "New Customer",
                TabAction = TabActions.Create,
                Content = childContent,
            };

            _tabs.Add(newTab);

            selectedIndex = _tabs.Count;


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
                builer.OpenComponent(0, typeof(EmployeeEdit));

                builer.AddAttribute(1, "EmployeeId", id);

                builer.AddAttribute(2, "OnClose", EventCallback.Factory.Create(this, () =>
                {
                    CloseTab(editTabId);
                }));

                builer.AddAttribute(4, "UpdateChanged", EventCallback.Factory.Create(this, async (VEmployeeList x) =>
                {

                    var foundIndex = Employees?.FindIndex(c => c.EmployeeId == x.EmployeeId) ?? -1;

                    if (foundIndex != -1)
                    {
                        Employees![foundIndex] = x;
                    }

                    await _gridEmployee.Reload();

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

            await GetBranches();

            await GetEmployees(branchId);

        }

        async Task Delete(string? employeeId)
        {
            ContextMenuService.Close();

            var confitm = await DialogService!.Confirm(
                "Employee deleted will not be able to recover, would you like to continue?",
                "Delete Employee",
                new ConfirmOptions
                {
                    CancelButtonText = "Cancel",
                    OkButtonText = "OK"
                });
            if (confitm == true)
            {
                try
                {
                    var deleteResponse = await _employeeService!.DeleteAsync(employeeId);

                    if (deleteResponse.IsSuccess)
                    {
                        await GetEmployees(branchId, searchText);
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Error, "Can not delete");
                    }
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, ex.InnerException!.Message);
                }
            }
        }


        async Task GetEmployees(int? branchId, string? q = null)
        {
            isLoading = true;

            _logger.LogInformation("try to getting employee information");

            try
            {
                var param = new QueryParam(pageIndex, pageSize, sortField, includeInActive, sortDescending, q);

                var query = new GetEmployeeAsPagingQuery(branchId, null, param);

                var pageDate = await mediator.Send(query);

                counts = pageDate.Count;

                Employees = pageDate.Items;

                isLoading = false;
            }
            catch (Exception ex)
            {
                isLoading = false;

                _logger.LogError(ex.Message, ex);
            }
            finally
            {
                isLoading = false;
            }

            StateHasChanged();
        }

        async Task LoadData(LoadDataArgs e)
        {

            pageIndex = e.Skip ?? 0;

            pageSize = e.Top ?? 15;

            (sortField, sortDescending) = e.PageInfo(nameof(VEmployeeList.ModifiedDT));

            var filterValue = e.GetFilterValue(nameof(VEmployeeList.AltName),
                                               nameof(VEmployeeList.Phone),
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
                var branchId = ((BranchDto)arg).BranchId;

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
}
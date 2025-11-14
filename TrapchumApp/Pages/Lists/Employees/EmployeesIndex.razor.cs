using Applications.Extensions;
using Applications.Features.Addresses.Responses;
using Applications.Features.Employees.Requests;
using Applications.Features.Entities;
using Applications.Interfaces.Lists;
using Applications.Responses.Lists.Employees;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Employees
{
    public partial class EmployeesIndex
    {
        private readonly INationalityService _nationalityService;
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<EmployeesIndex> _logger;

        public EmployeesIndex(INationalityService nationalityService,
            IDepartmentService departmentService,
            ILogger<EmployeesIndex> logger)
        {
            _nationalityService = nationalityService;
            _departmentService = departmentService;
            _logger = logger;
        }

        RadzenTabs tabs = default!;

        List<TabItems> tabItems = [];

        string? filter = string.Empty;

        IList<VEmployeeList> SelectedEmployees = [];

        void OnRowRender(RowRenderEventArgs<VEmployeeList> e)
        {
            if (e.Data.IsActive)
            {
                //e.Attributes.Add("style", "color:red;background-color:yellow;");
            }

            e.Data.RowIndex = e.Index + 1; // store 1-based index
        }

        bool IsEnableEdit = false;
        bool IsEnableNew = false;
        bool IsEnableDetail = false;
        [Parameter] public int selectedIndex { get; set; } = 0;
        EmployeeDto Employee = new();
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<string> Salutations = [];
        List<BranchDto> Branches = [];
        IEnumerable<DepartmentDto> Departments = [];
        IEnumerable<Country> Countries = [];
        IEnumerable<Nationality> Nationalities = [];
        IEnumerable<OcupationResponse>? Occuptions = [];
        IEnumerable<string> Cities = [];
        EmployeeDtoUpdate EmployeeEdit = new();
        int pageSize = 15;
        int counts = 0;
        int pageIndex = 1;
        string sortField = "EmployeeId";
        bool sortDescending = false;
        string? _empId = string.Empty;
        int branchId = 0;

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


            var responseResult = await _employeeService!.UpdateIsActiveAsync(employeeId, state);

            if (!responseResult.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, responseResult.ErrMessage);

                return;
            }

            NotificationService.Notify(NotificationSeverity.Success, responseResult.Data);

            await GetEmployees(branchId, searchText);
        }

        async Task GetBranches()
        {
            var results = await _branchService.GetBranchesAsync<BranchDto>(0, new QueryParam(0, 0));

            var unionBranches = new List<BranchDto>
            {
                new BranchDto
                {
                    BranchId = 0,
                    BranchName = "All"
                }
            };

            if (results.IsSuccess)
            {
                var branches = results!.Data!.Items.Union(unionBranches);

                Branches = branches.OrderBy(x => x.BranchId).ToList();
            }
        }


        void AddNewTab()
        {
            IsEnableNew = true;
            selectedIndex = 1;
            tabs.Reload();
            tabCount++;
            StateHasChanged();
        }


        void AddDetailsTab(string? employeeId)
        {
            ContextMenuService.Close();
            try
            {
                _empId = employeeId;
                IsEnableDetail = true;
                selectedIndex = 3;
                tabs.Reload();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }

        void AddEditTab(string? employeeId)
        {
            ContextMenuService.Close();

            _empId = employeeId;

            IsEnableEdit = true;

            selectedIndex = 2;

            StateHasChanged();
        }

        void OnEditTab(string? employeeId)
        {
            _empId = employeeId;
            IsEnableEdit = true;
            selectedIndex = 2;
            StateHasChanged();
        }

        void TabChanged(int e)
        {
            selectedIndex = e;
            StateHasChanged();
        }

        void OnIsEditClose(bool isEditCloses)
        {
            IsEnableEdit = isEditCloses;
            selectedIndex = 0;
            StateHasChanged();
        }

        void OnIsDetailClose(bool isDetailCloses)
        {
            IsEnableDetail = isDetailCloses;
            selectedIndex = 0;
            StateHasChanged();
        }

        async void GetIncludeInactive()
        {
            await GetEmployees(branchId, searchText);
        }

        public EmployeeDto? employee;

        string searchText = string.Empty;

        List<VEmployeeList> Employees = [];

        protected bool includeInActive = false;

        bool isLoading = false;

        protected async Task PerformSearch(string? searchtext = null)
        {
            searchText = searchtext ?? string.Empty;

            await GetEmployees(branchId, searchText);
        }

        void OnClosing(bool isNewClosed)
        {
            IsEnableNew = isNewClosed;

            selectedIndex = 0;

            StateHasChanged();
        }


        protected async Task Reload()
        {
            await GetEmployees(branchId, searchText);
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetBranches();

                await GetEmployees(branchId);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
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

                var pageDate = await _employeeService.GetEmployeesAsync(branchId, null, param);

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

            if (e.Sorts.Any())
            {
                sortField = e.Sorts.FirstOrDefault()!.Property;

                sortDescending = e.Sorts.FirstOrDefault()?.SortOrder == SortOrder.Descending;
            }

            var filterColumn = e.Filters?.FirstOrDefault(p => p.Property == nameof(VEmployeeList.AltName) ||
                                                            p.Property == nameof(VEmployeeList.Phone) ||
                                                            p.Property == nameof(VEmployeeList.EmailAddress) ||
                                                            p.Property == nameof(VEmployeeList.DepartmentName) ||
                                                            p.Property == nameof(VEmployeeList.Occupation) ||
                                                            p.Property == nameof(VEmployeeList.BranchName))?.FilterValue?.ToString() ?? string.Empty;


            searchText = filterColumn ?? string.Empty;

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
            AddDetailsTab(args.Data.EmployeeId);
            tabCount++;
        }
    }
}
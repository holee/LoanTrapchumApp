
using Applications.Extensions;
using Applications.Extensions.mappers;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Employees;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Lists.Employees;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.Employees
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
                e.Attributes.Add("style", "color:red;background-color:yellow;");
            }
        }
        bool IsEnableEdit = false;
        bool IsEnableNew = false;
        bool IsEnableDetail = false;
        [Parameter] public int selectedIndex { get; set; } = 0;
        EmployeeDto Employee = new();
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<string> Salutations = [];
        IEnumerable<BranchDto> Branches = [];
        IEnumerable<DepartmentDto> Departments = [];
        IEnumerable<Country> Countries = [];
        IEnumerable<Nationality> Nationalities = [];
        IEnumerable<OcupationResponse>? Occuptions = [];
        IEnumerable<string> Cities = [];
        EmployeeDtoUpdate EmployeeEdit = new();
        int pageSize = 10;
        int counts = 0;
        int pageIndex = 0;
        string sortField = "EmployeeId";
        bool sortDescending = false;
        string? _empId = string.Empty;
        void PageSizeChanged(int page)
        {
            pageSize = page;
        }


        async Task OnCheckChanged(VEmployeeList param)
        {
            var responseResult = await _employeeService!.UpdateIsActiveAsync(param.EmployeeId);

            if (!responseResult.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, responseResult.ErrMessage);
                return;
            }

            NotificationService.Notify(NotificationSeverity.Success, responseResult.Data);

            await GetEmployees(searchText);
        }

        async Task GetBranches()
        {
            var results = await _branchService.GetBranchesAsync<BranchDto>(0, new QueryParam(0, 0, string.Empty));

            if (results.IsSuccess)
            {
                Branches = results!.Data!.Items;
            }

        }


        void AddNewTab()
        {

            IsEnableNew = true;
            selectedIndex = 1;
            tabs.Reload();
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
            await GetEmployees(searchText);
        }

        public EmployeeDto? employee;
        string searchText = string.Empty;
        IEnumerable<VEmployeeList> Employees = [];
        protected bool includeInActive = false;
        bool isLoading = false;
        protected async void PerformSearch(string? searchtext = null)
        {
            searchText = searchtext ?? string.Empty;
            await GetEmployees(searchText);
        }

        void OnClosing(bool isNewClosed)
        {
            IsEnableNew = isNewClosed;
            selectedIndex = 0;
            StateHasChanged();
        }


        protected async void Reload()
        {
            await GetEmployees(searchText);
        }
        protected async override Task OnInitializedAsync()
        {
            try
            {
                await GetBranches();
                await GetEmployees();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

            }
        }

        async Task Delete(string? employeeId)
        {
            ContextMenuService.Close();

            var confitm = await DialogService!.Confirm("Employee deleted will not be able to recover, would you like to continue?",
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
                        await GetEmployees(searchText);
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


        async Task GetEmployees(string? q = null)
        {
            isLoading = true;

            _logger.LogInformation("try to getting employee information");

            try
            {
                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInActive, q);

                var pageDate = await _employeeService.GetEmployeesAsync(param);

                var counts = pageDate.Count;

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
            searchText = e.Filter ?? string.Empty;

            pageIndex = e.Skip!.Value;

            if (!string.IsNullOrEmpty(e.OrderBy))
            {
                sortField = e.OrderBy.Split(" ")[0];

                string dir = e.OrderBy.Split(" ")[1];

                sortDescending = dir.Contains("asc") ? false : true;
            }
            await GetEmployees(searchText);
        }


    }


}
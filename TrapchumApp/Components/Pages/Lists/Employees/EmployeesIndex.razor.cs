
using Applications.Extensions.mappers;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Employees;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Paginations;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Employees
{
    public partial class EmployeesIndex
    {
        private readonly INationalityService _nationalityService;
        private readonly IDepartmentService _departmentService;
        public EmployeesIndex(INationalityService nationalityService, IDepartmentService departmentService)
        {
            _nationalityService = nationalityService;
            _departmentService = departmentService;
        }

        public EmployeeDto? employee;
        string searchText = string.Empty;
        IEnumerable<Applications.Responses.ViewModels.Employees.VEmployees> Employees = [];
        protected bool includeInActive = false;
        bool isLoading = false;
        protected async void PerformSearch(string? searchtext = null)
        {
            searchText = searchtext ?? string.Empty;
            await GetEmployees(searchText);
        }
        async Task GetAdditionalInformation()
        {
            var results = await _branchService!.GetBranchesAsync(0, 0, false);

            if (results.IsSuccess)
            {
                //Branches = results!.Data!;
            }
            var deptResponse = await _departmentService!.GetDepartments(0, 0);

            if (deptResponse.IsSuccess)
            {
                Departments = deptResponse.Data!.Maps<DepartmentDto>();
            }

            Nationalities = await _nationalityService!.GetNationalities(12);

            Provinces = await _addressService.GetsProvinces();

            Occuptions = await _employeeService!.GetOccupationsAsync("Position");

            Countries = await _addressService.GetCountries(105);

            Cities = await _addressService.GetCities();

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
                await GetEmployees();
                await GetAdditionalInformation();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

            }
        }

        async Task Delete(Guid? employeeId)
        {

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
                    ContextMenuService.Close();

                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, ex.InnerException!.Message);
                }

            }
            ContextMenuService.Close();


        }


        async Task GetEmployees(string? q = null)
        {
            isLoading = true;
            try
            {
                var param=new QueryParam(pageIndex,pageSize,sortField,sortDescending,includeInActive,q);
                
                var pageDate = await _employeeService.GetEmployeesAsync(param);
               
                var counts = pageDate.Count;
                
                Employees = pageDate.Items;
               
                isLoading = false;

            }
            catch (Exception ex)
            {
                isLoading = false;
                Console.WriteLine(ex.Message);
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
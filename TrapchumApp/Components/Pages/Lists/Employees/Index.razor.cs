using TCLMS.Shared.Entities.Dtos.Lists.Employees;
using TCLMS.Shared.Entities.ViewModels.Employees;
using Radzen;

namespace TCLMS.Front.Pages.Lists.Employees
{
    public partial class Index
    {
        public EmployeeDto? employee;
        string searchText = string.Empty;
        IEnumerable<VEmployees> Employees = [];
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
                Branches = results!.Data!;
            }
            var deptResponse = await _departmentService!.GetListDepartments(0, 0);
            if (deptResponse.IsSuccess)
            {
                Departments = deptResponse.Data!;
            }
            Nationalities = await _nationalityService!.GetAllAsync();
            Provinces = await _addressService.GetAllProvinceAsync();
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
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);

            }
        }

        async Task Delete(long employeeId)
        {

            var confitm = await DialogService!.Confirm("Employee deleted will not be able to recover, would you like to continue?",
                "Delete Employee",
                new Radzen.ConfirmOptions
                {
                    CancelButtonText = "Cancel",
                    OkButtonText = "OK"
                });
            if (confitm == true)
            {
                try
                {
                    var deleteResponse = await _employeeService!.DeleteEmployee(employeeId);

                    if (deleteResponse.IsSuccess)
                    {
                        await GetEmployees(searchText);
                    }
                    else
                    {
                        NotificationService.Notify(Radzen.NotificationSeverity.Error, "Can not delete");
                    }
                    ContextMenuService.Close();

                }
                catch (Exception ex)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.InnerException!.Message);
                }

            }
            ContextMenuService.Close();


        }


        async Task GetEmployees(string? q = null)
        {
            isLoading = true;
            try
            {
                var pageDate = await _employeeService.GetEmployeesAsync(pageIndex, pageSize, sortField, sortDescending, includeInActive, q);
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
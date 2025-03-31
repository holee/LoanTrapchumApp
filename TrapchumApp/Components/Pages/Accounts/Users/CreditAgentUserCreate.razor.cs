using Applications.Extensions.Identity.Enumerations;
using Applications.Interfaces.Employees;
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Requests.Lists.Entities;
using Applications.Responses.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Components.Pages.Accounts.Users
{
    public partial class CreditAgentUserCreate
    {
        [Parameter] public Guid? EmployeeId { get; set; } = Guid.Empty;
        [Parameter] public int BranchId { get; set; } = 0;
        RegisterRequest model = new RegisterRequest();
        EditForm form = default!;
        IEnumerable<BranchDto> Branches = [];
        IEnumerable<string?> roles = [];
        IEnumerable<VEmployees> Employees = [];
        
        private readonly IRoleService _roleService;
        public CreditAgentUserCreate(IRoleService roleService)
        {
            _roleService = roleService;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var employeeResponse = await _employeeService.GetEmployeesAsync(EmployeeId, BranchId);
            var branchResponse = await _branchService.GetBranchesAsync(BranchId, 1, false);
            var getRoles = await _roleService.GetRolesAsync();


            roles = getRoles?.Data?.Where(x => x.RoleName!.Contains(AppRoles.Co)).Select(r => r.RoleName) ?? [];

            model.RoleName = roles.First();

            if (employeeResponse.IsSuccess)
            {
                Employees = employeeResponse.Data!;
                SelectedEmployeeChanged(Employees.First());
            }
            if (branchResponse!.IsSuccess)
            {
                // Branches = branchResponse.Data!;
                model.BranchRefId = Branches.First().BranchId;
            }
            else
            {
                Branches = [];
            }
        }
        protected async void SubmitForm()
        {
            try
            {
                form.EditContext?.Validate();

                var result = await _userService.CreateUserAsync(model);

                if (result.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "New user was created.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error,result.ErrMessage);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }


        }

        void SelectedEmployeeChanged(VEmployees? emp)
        {
            if (emp is not null)
            {
                var username = emp.FullName!.Split(" ");
                model.EmployeeId = emp.EmployeeId;
                model.Username = $"{username[0]}{username[1]}".ToLower();
                model.Email = $"{username[0]}{username[1]}@gmail.com".ToLower();
                model.Password = "Pa$$w0rd";
                model.ConfirmPassword = "Pa$$w0rd";
                StateHasChanged();
            }
            else
            {
                model.Username = "";
                model.Email = "";
                model.EmployeeId = Guid.Empty;
                model.Password = "Pa$$w0rd";
                model.ConfirmPassword = "Pa$$w0rd";
            }
        }



    }
}
using Applications.Extensions.Identity.Enumerations;
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Pages.Identity.Users
{
    public partial class CreditAgentUserCreate
    {
        [Parameter] public string? EmployeeId { get; set; } = string.Empty;
        [Parameter] public int BranchId { get; set; } = 0;

        RegisterRequest model = new RegisterRequest();

        EditForm form = default!;

        IEnumerable<BranchDto> Branches = [];

        IEnumerable<string?> roles = [];

        IEnumerable<VEmployeeList> Employees = [];

        private readonly IRoleService _roleService;
        public CreditAgentUserCreate(IRoleService roleService)
        {
            _roleService = roleService;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var empRsp = await _employeeService.GetEmployeesAsync(BranchId, EmployeeId,new(0,0));

            var getRoles = await _roleService.GetRolesAsync(null);


            roles = getRoles?.Data?.Where(x => x.Contains(AppRoles.Co)).ToList() ?? [];

            model.RoleName = roles.First()!;

            if (empRsp.Items.Any())
            {
                Employees = empRsp.Items ?? [];

                SelectedEmployeeChanged(Employees.First());
            }

            await GetBranches();

        }

        private async Task GetBranches()
        {

            var branchResponse = await _branchService.GetBranchesAsync<BranchDto>(BranchId, new QueryParam(0, 25, string.Empty));

            if (branchResponse!.IsSuccess)
            {

                Branches = branchResponse.Data.Items ?? [];

                model.BranchRefId = BranchId;

            }
            else
            {
                Branches = [];
            }

            await Task.CompletedTask;

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
                    NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }


        }

        void SelectedEmployeeChanged(VEmployeeList? emp)
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
                model.EmployeeId = string.Empty;
                model.Password = "Pa$$w0rd";
                model.ConfirmPassword = "Pa$$w0rd";
            }
        }



    }
}
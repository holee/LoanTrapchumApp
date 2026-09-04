using Applications.Extensions.Identity.Enumerations;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace Web.UI.Pages.Lists.CreditAgents
{
    public partial class CreditAgentUserCreate
    {
        [Parameter] public string? EmployeeId { get; set; } = string.Empty;
        [Parameter] public int BranchId { get; set; } = 0;

        RegisterRequest model = new RegisterRequest();

        EditForm form = default!;

        IEnumerable<VBranch> Branches = [];

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

            var getRoles = await _roleService.GetRolesAsync(null);

            roles = getRoles.IsError ? [] : getRoles.Value.Where(x => x.Contains(AppRoles.Co)).ToList();

            model.RoleName = roles.FirstOrDefault() ?? "Credit Officer";

            await GetBranches();
        }

        private async Task GetBranches()
        {
            var command = new GetBranchAsPagingQuery(0, new QueryParam(0, 0));

            var rsp = await mediator.Send(command);

            if (rsp!.IsSuccess)
            {
                Branches = rsp.Items ?? [];
                model.BranchRefId = BranchId;
            }
            else
            {
                Branches = Enumerable.Empty<VBranch>();
            }

            await Task.CompletedTask;
        }

        protected async void SubmitForm()
        {
            try
            {
                form.EditContext?.Validate();

                var result = await _userService.CreateUserAsync(model);

                if (!result.IsError)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "New user was created.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, result.FirstError.Description);
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

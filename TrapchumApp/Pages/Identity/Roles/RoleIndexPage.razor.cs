using Applications.Constants.Authorizations;
using Applications.Features.Identity.Roles.Queries.GetRolesAsPaging;
using Applications.Features.Identity.Roles.Responses;
using Applications.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using TrapchumApp.Components.Messageboxes;
using Applications.Features.Identity.Roles.Commands.DeleteRole;
using System.Security.Claims;

namespace TrapchumApp.Pages.Identity.Roles
{
    public partial class RoleIndexPage
    {
        private readonly IMediator _mediator;
        private int pageIndex = 0;
        private int pageSize = 15;
        private int counts = 0;
        private bool includeInActive = false;
        protected bool IsLoading = true;
        protected int index = 0;
        private string _filter = string.Empty;
        private string sortField = "Name";
        private bool sortDescending = false;
        protected IEnumerable<RoleDto>? Roles = Enumerable.Empty<RoleDto>();

        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        private bool _canDeleteRole = false;
        private string? _currentUserId;

        public RoleIndexPage(IMediator mediator)
        {
            _mediator = mediator;
        }
        void PageSizeChanged(int size)
        {
            pageSize = size;
            pageIndex = 0;
        }

        void ManageRolePermissions(string roleId)
        {
            NavigationManager.NavigateTo($"/App/Account/Roles/{roleId}/Permissions");
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationState;

            var user = authState.User;

            _currentUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _canDeleteRole = user.HasClaim(AppClaim.Permission, AppPermission.RuleFor(Modules.Roles, Actions.Delete));

            await GetRoles();

            await base.OnInitializedAsync();
        }

        private async Task LoadRoleData(LoadDataArgs args)
        {
            pageIndex = args.Skip ?? 0;

            pageSize = args.Top ?? 15;

            (sortField, sortDescending) = args.PageInfo();

            await GetRoles(_filter);

        }

        async Task SearchChanged(string? filter)
        {
            _filter = filter ?? string.Empty;

            await GetRoles(filter);
        }


        protected async Task GetRoles(string? filter = null)
        {
            var param = new QueryParam(pageIndex, pageSize, sortField, includeInActive, sortDescending, filter: filter);

            var result = await _mediator.Send(new GetRolesAsPagingQuery(param));

            Roles = result.Items;

            counts = result.Count;

        }

        async Task DeleteRole(string? roleId)
        {
            if (string.IsNullOrEmpty(roleId)) return;

            var dia = await MsgBox.ShowAsync("Are you sure you want to delete this role?", "Delete Role", MsgBoxButtons.YesNo, MsgBoxBoxIcon.Question);

            if (dia == MsgBoxResult.No) return;

            var result = await _mediator.Send(new DeleteRoleCommand(roleId));

            if (result.IsError)
            {
                _toastService.ShowError(result.FirstError.Description);

                return;
            }

            _toastService.ShowSuccess("Role deleted successfully.");

            await GetRoles(_filter);
        }
    }
}

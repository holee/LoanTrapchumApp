using Applications.Interfaces.Identity;
using Applications.Requests.Identity;

namespace TrapchumApp.Components.Pages.Accounts.Roles
{
    public partial class Index
    {
        private readonly IRoleService _roleService;

        public Index(IRoleService roleService)
        {
            _roleService = roleService;
        }


        int pageSize = 15;
        void PageSizeChanged(int size)
        {
            pageSize = size;
        }

        async Task RoleRefresh()
        {
            await GetRoles();
        }
        void ManageRolePermissions(string roleId)
        {
            NavigationManager.NavigateTo($"/App/Account/Roles/{roleId}/Permissions");
        }
        protected bool IsLoading = true;

        protected int index = 0;

        protected IEnumerable<RoleDto>? Roles = Enumerable.Empty<RoleDto>();

        protected override async Task OnInitializedAsync()
        {

            await GetRoles();

            await base.OnInitializedAsync();
        }

        protected async Task GetRoles()
        {
            var result = await _roleService.GetRolesAsync(string.Empty,string.Empty);

            if (result.IsSuccess)
            {
                Roles = result.Data;
            }
            else
            {
                Roles = [];
            }


        }
    }
}
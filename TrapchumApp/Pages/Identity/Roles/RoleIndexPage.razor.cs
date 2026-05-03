using Applications.Interfaces.Identity;
using Applications.Features.Identity.Roles.Responses;

namespace TrapchumApp.Pages.Identity.Roles
{
    public partial class RoleIndexPage
    {
        private readonly IRoleService _roleService;
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
        public RoleIndexPage(IRoleService roleService)
        {
            _roleService = roleService;
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

            var result = await _roleService.GetRolesAsync(string.Empty, param);

            Roles = result.Items;

            counts = result.Count;

        }
    }
}
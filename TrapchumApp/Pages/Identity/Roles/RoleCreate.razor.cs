using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Responses.ViewModels.Auths;
using Applications.Services.Identity;

namespace TrapchumApp.Pages.Identity.Roles;

public partial class RoleCreate
{
    private RoleCreateDto model = new RoleCreateDto();

    private EditForm roleCreateForm = default!;
    private List<RolePermissionsDtoCreate>? RoleClaimsGrouped { get; set; } = [];

    private bool visible = false;

    private readonly IRoleService roleService;

    private readonly IRoleServiceImpl roleServiceImpl;
    public RoleCreate(IRoleService roleService, IRoleServiceImpl roleServiceImpl)
    {
        this.roleService = roleService;
        this.roleServiceImpl = roleServiceImpl;
    }

    protected async Task CreateRole()
    {

        if (roleCreateForm.EditContext!.Validate() == false) return;


        var permissions = RoleClaimsGrouped?
                    .SelectMany(x => x.Permissions)
                    .Where(x => x.IsSelected == true)
                    .Select(x => new RolePermissionsViewModel
                    {
                        ClaimType = x.ClaimType,
                        ClaimValue = x.ClaimValue,
                        Description = x.Description,
                        Group = x.Group,
                        IsAssignedToRole = x.IsSelected,
                    }).OrderBy(x=>x.Group)
                    .ToList();

        model.Permissions = permissions ?? [];

        var result = await roleService.CreateAsync(model);

        if (!result.IsSuccess)
        {
            NotificationService.Notify(NotificationSeverity.Success, "New Role was added.");
        }

    }


}

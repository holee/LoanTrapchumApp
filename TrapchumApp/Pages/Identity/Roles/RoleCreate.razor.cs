using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Pages.Identity.Roles;

public partial class RoleCreate
{
    protected RoleCreateDto model = new RoleCreateDto();
    protected EditForm roleCreateForm = default!;
    protected bool visible = false;
    private readonly IRoleService roleService;

    public RoleCreate(IRoleService roleService)
    {
        this.roleService = roleService;
    }

    protected async Task CreateRole()
    {

        roleCreateForm.EditContext!.Validate();

        var result = await roleService.CreateAsync(model);

        if (!result.IsSuccess)
        {
            NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Success, "New Role Created.");
        }

    }


}

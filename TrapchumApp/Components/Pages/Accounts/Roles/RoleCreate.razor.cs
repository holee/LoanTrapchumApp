using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Microsoft.AspNetCore.Components.Forms;

namespace TrapchumApp.Components.Pages.Accounts.Roles;

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
            NotificationService.Notify(Radzen.NotificationSeverity.Error, result.ErrMessage);
        }
        else
        {
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "New Role Created.");
        }

    }


}

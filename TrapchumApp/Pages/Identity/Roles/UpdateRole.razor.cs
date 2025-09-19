using Applications.Features.Identity.Roles.Requests;
using Applications.Interfaces.Identity;
using TrapchumApp.Helpers.Validations;

namespace TrapchumApp.Pages.Identity.Roles;
public partial class UpdateRole
{
    [Parameter] public string? RoleId { get; set; }

    private RoleDtoUpdate model = new RoleDtoUpdate();

    private EditForm roleCreateForm = default!;

    private readonly IRoleService roleService;

    public UpdateRole(IRoleService roleService)
    {
        this.roleService = roleService;
    }


    protected override async Task OnInitializedAsync()
    {
        var rsp = await roleService.EditAsync(RoleId);

        if (rsp.IsSuccess && rsp.Data is not null)
        {
            model = rsp.Data;
        }

        await base.OnInitializedAsync();
    }

    protected async Task CreateRole()
    {

        if (roleCreateForm.EditContext!.Validate() == false) return;

        var rsp = await roleService.UpdateAsync(model);

        if (!rsp.IsSuccess)
        {
            roleCreateForm.EditContext.ApplyValidationSummary<RoleDtoUpdate>(rsp.ErrMessage);
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Success, rsp.Data);

             DialogService.Close(true);
        }

    }

}

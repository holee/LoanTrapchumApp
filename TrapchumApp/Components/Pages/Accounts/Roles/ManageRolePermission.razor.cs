
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Auths;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace TrapchumApp.Components.Pages.Accounts.Roles;

public partial class ManageRolePermission
{

    private readonly IRoleService _roleService;

    public ManageRolePermission(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [Parameter, EditorRequired] public string Id { get; set; } = string.Empty;

    protected UserClaimsVm? UserClaimsVM = new UserClaimsVm();

    protected RolePermissions rolePermissions = new();

    protected EditForm formEdit = default!;

    protected bool IsLoading = true;
    protected Dictionary<string, List<RolePermissionsViewModel>> RoleClaimsGrouped { get; } = new();

    protected List<string> groups = [];
    protected override async Task OnInitializedAsync()
    {
        await GetPermissions();
        await base.OnInitializedAsync();
    }

    private async Task GetPermissions()
    {
        IsLoading = true;

        var resultResponse = await _roleService.GetPermissionsByRoleAsync(Id);

        if (resultResponse.IsSuccess)
        {
            rolePermissions = resultResponse.Data!;

            if (rolePermissions is not null)
            {

                RoleClaimsGrouped.Add("All", rolePermissions.Permissions!);

                foreach (var permission in rolePermissions.Permissions!)
                {
                    if (RoleClaimsGrouped.ContainsKey(permission.Group))
                    {
                        RoleClaimsGrouped[permission.Group].Add(permission);
                    }
                    else
                    {
                        RoleClaimsGrouped.Add(permission.Group, new List<RolePermissionsViewModel> { permission });
                    }
                }

                if (RoleClaimsGrouped.Count > 0)
                {

                    groups = RoleClaimsGrouped.Select(x => x.Key).ToList();

                }
            }
        }
        else
        {
            NotificationService.Notify(Radzen.NotificationSeverity.Error, resultResponse.ErrMessage);
        }

        IsLoading = false;
    }

    protected async void SubmitHandler()
    {
        var request = new RolePermissionRequest(Id, rolePermissions!.Permissions);

        formEdit?.EditContext?.Validate();

        var responseResult = await _roleService.AddRolePermissions(request);//.AssignPermissionAsync(request);

        if (responseResult!.IsSuccess)
        {
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Permissions were assigned.");

            NavigationManager.NavigateTo("/Administrator/Roles");
        }
        else
        {
            NotificationService.Notify(Radzen.NotificationSeverity.Error, responseResult.ErrMessage);
        }
    }



}

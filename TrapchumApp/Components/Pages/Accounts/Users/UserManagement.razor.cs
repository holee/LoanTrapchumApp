using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Auths;
using Microsoft.AspNetCore.Components;
using Radzen;
namespace TrapchumApp.Components.Pages.Accounts.Users;

public partial class UserManagement
{
    [Parameter, EditorRequired] public string Id { get; set; } = string.Empty;
    bool IsLoading = true;
    UserPermissions userPermission = new();
    ChangePasswordDto model = new();
    UserDto userModel = new UserDto();
    Dictionary<string, List<UserPermissionViewModel>> UserClaimsGrouped { get; } = new();
    UserPermissions rolePermissions = new();
    List<string> groups = [];
    IEnumerable<string> roles = [];
    bool isBusy = false;
    UserRoleDtoUpdate formRequest = new();
    string? message = string.Empty;
    private readonly IRoleService _roleService;
    public UserManagement(IRoleService roleService)
    {
        _roleService = roleService;
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            model.UserId = Id;
            await GetPermissions();
            await GetRoles();
            var resp = await _userService.GetUserDetailsById(Id);
            if (resp.IsSuccess)
            {
                userModel = resp.Data!;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
            }
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
        await base.OnInitializedAsync();
    }
    protected async Task SubmitOnValid()
    {
        try
        {
            var response = await _userService.ChangePassword(model);

            if (!response.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
                return;
            }
            else
            {
                //message=response.
                NotificationService.Notify(NotificationSeverity.Success, "");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    async Task HandleSubmitOnValid()
    {
        try
        {
            formRequest.UserId = Id;

            var response = await _userService.UpdateUserRoleAsync(formRequest);

            if (!response.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
                return;
            }
            NotificationService.Notify(NotificationSeverity.Success, "Success");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }


    async Task GetRoles()
    {
        var getRoles = await _roleService.GetRolesAsync();

        roles = getRoles?.Data?.Where(x => !x.RoleName!.Contains(userModel.Role))!.Select(r => r.RoleName!) ?? [];

    }

    private async Task GetPermissions()
    {
        IsLoading = true;

        var resultResponse = await _userService.GetUserPermissionsAsync(Id);

        if (resultResponse.IsSuccess)
        {
            userPermission = resultResponse.Data!;

            if (userPermission is not null)
            {

                UserClaimsGrouped.Add("All", userPermission.Permissions!);

                foreach (var permission in userPermission.Permissions!)
                {
                    if (UserClaimsGrouped.ContainsKey(permission.Group))
                    {
                        UserClaimsGrouped[permission.Group].Add(permission);
                    }
                    else
                    {
                        UserClaimsGrouped.Add(permission.Group, new List<UserPermissionViewModel> { permission });
                    }
                }

                if (UserClaimsGrouped.Count > 0)
                {

                    groups = UserClaimsGrouped.Select(x => x.Key).ToList();

                }
            }
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, resultResponse.ErrMessage);
        }
        IsLoading = false;
        StateHasChanged();
    }



    private async Task AddUserPermissions()
    {
        isBusy = true;

        var request = new UserPermissionRequest(Id, userPermission!.Permissions);

        var resultResponse = await _userService.AddOrRemovedUserPermissionsAsync(Id, request);

        if (!resultResponse.IsSuccess)
        {
            NotificationService.Notify(NotificationSeverity.Success, resultResponse!.ErrMessage);
        }
        else
        {
            Console.WriteLine(resultResponse.Data);
            message = resultResponse.Data;
        }
        isBusy = false;

    }

}

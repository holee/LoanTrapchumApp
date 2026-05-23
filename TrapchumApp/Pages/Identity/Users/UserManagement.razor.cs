using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Auths;
using Microsoft.AspNetCore.Components.Forms;

namespace TrapchumApp.Pages.Identity.Users;

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

        roles = getRoles.IsError ? [] : getRoles.Value.Where(x => !x.Contains(userModel.Role)).Select(r => r);

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

    bool _permSelectAll = false;
    private string _permGroupFilter = string.Empty;
    ProfileDtoCreate _profileUploadModel = new();
    string? _profilePreviewUrl = null;
    bool _profileBusy = false;
    const long _maxProfileFileSize = 10 * 1024 * 1024;

    private IEnumerable<KeyValuePair<string, List<UserPermissionViewModel>>> FilteredUserGroups =>
        UserClaimsGrouped
            .Where(kvp => kvp.Key != "All" &&
                (string.IsNullOrWhiteSpace(_permGroupFilter) ||
                 kvp.Key.Contains(_permGroupFilter, StringComparison.OrdinalIgnoreCase)));

    private void OnSelectAllPermissionsChanged(bool isChecked)
    {
        _permSelectAll = isChecked;
        foreach (var perm in userPermission?.Permissions ?? [])
            perm.IsAssignedToRole = isChecked;
    }

    private void OnUserGroupSelectAllChanged(string groupName, bool isChecked)
    {
        if (UserClaimsGrouped.TryGetValue(groupName, out var perms))
            foreach (var perm in perms)
                perm.IsAssignedToRole = isChecked;

        _permSelectAll = userPermission?.Permissions?.All(p => p.IsAssignedToRole) ?? false;
    }

    private void OnUserPermissionChanged(UserPermissionViewModel item, bool isChecked)
    {
        item.IsAssignedToRole = isChecked;
        _permSelectAll = userPermission?.Permissions?.All(p => p.IsAssignedToRole) ?? false;
    }

    private async Task HandleProfileFileSelected(InputFileChangeEventArgs e)
    {
        if (e.FileCount == 0) return;

        var file = e.File;

        using var stream = new MemoryStream();

        using var fileStream = file.OpenReadStream(_maxProfileFileSize);

        await fileStream.CopyToAsync(stream);

        _profileUploadModel.FileName = file.Name;

        _profileUploadModel.FileSize = file.Size;

        _profileUploadModel.FileContent = stream.ToArray();

        var base64 = Convert.ToBase64String(_profileUploadModel.FileContent);

        _profilePreviewUrl = $"data:{file.ContentType};base64,{base64}";
    }

    private async Task SaveProfilePhoto()
    {
        if (_profileUploadModel.FileContent is null) return;

        _profileBusy = true;

        var rsp = await _userService.ChangeProfile(Id, _profileUploadModel);

        if (rsp.IsSuccess)
        {
            if (userModel.Profile is null)
                userModel.Profile = new();

            userModel.Profile.FileUrl = _profilePreviewUrl;

            NotificationService.Notify(NotificationSeverity.Success, "Profile photo updated.");

            CancelProfileUpload();
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
        }
        _profileBusy = false;
    }

    private void CancelProfileUpload()
    {
        _profileUploadModel = new();
        _profilePreviewUrl = null;
    }

}

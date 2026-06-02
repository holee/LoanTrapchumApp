using Applications.Features.Identity.Command.ChangePassword;
using Applications.Features.Identity.Command.UpdateUserRole;
using Applications.Features.Identity.Queries.GetUserById;
using Applications.Features.Identity.Queries.GetUserPermissions;
using Applications.Features.Identity.Roles.Queries.GetRoles;
using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Auths;
using Microsoft.AspNetCore.Components.Forms;

namespace TrapchumApp.Pages.Identity.Users;

public partial class UserManagement(ILogger<UserManagement> logger)
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

    protected override async Task OnInitializedAsync()
    {
        try
        {
            model.UserId = Id;

            await GetPermissions();

            await GetRoles();

            var resp = await mediator.Send(new GetUserByIdQuery(Id));

            if (resp.IsSuccess())
            {
                userModel = resp.Value;
            }
            else
            {
                _toastService.ShowError(resp.GetMessage());
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
        var comamnd = new ChangePasswordCommand(model);

        var response = await mediator.Send(comamnd);  //_userService.ChangePassword(model);

        if (response.IsError)
        {
            _toastService.ShowError(response.GetMessage());
        }
        else
        {
            _toastService.ShowSuccess("Password was change successfully.");
        }

    }

    async Task HandleSubmitOnValid()
    {
        try
        {
            formRequest.UserId = Id;


            var commmand = new UpdateUserRoleCommand(formRequest);

            var response = await mediator.Send(commmand);  //_userService.UpdateUserRoleAsync(formRequest);

            if (response.IsError)
            {
                _toastService.ShowError(response.GetMessage());

                return;
            }

            _toastService.ShowSuccess("User role was updated successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating user role: {Message}", ex.Message);
        }

    }


    async Task GetRoles()
    {
        logger.LogInformation("Fetching roles for user management with UserId: {UserId}", Id);

        try
        {
            var command = new GetRolesQuery(string.Empty);

            var getRoles = await mediator.Send(command);  //_roleService.GetRolesAsync();

            roles = getRoles.Where(x => !x.Contains(userModel.Role)).Select(r => r);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching roles: {Message}", ex.Message);
        }

    }

    private async Task GetPermissions()
    {
        IsLoading = true;

        var rsp = await mediator.Send(new GetUserPermissionsQuery(Id)); // _userService.GetUserPermissionsAsync(Id);

        if (rsp.IsSuccess())
        {
            userPermission = rsp.Value;

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
            _toastService.ShowError(rsp.GetMessage());
        }
        IsLoading = false;

        StateHasChanged();
    }



    private async Task AddUserPermissions()
    {
        isBusy = true;

        var request = new UserPermissionRequest(Id, userPermission!.Permissions);

        var rsp = await _userService.AddOrRemovedUserPermissionsAsync(Id, request);

        if (rsp.IsError)
        {
            _toastService.ShowError(rsp.GetMessage());
        }
        else
        {
            message = rsp.Value;
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

        if (!rsp.IsError)
        {
            if (userModel.Profile is null)
                userModel.Profile = new();

            userModel.Profile.FileUrl = _profilePreviewUrl;

            _toastService.ShowSuccess("Profile photo updated successfully!");


            CancelProfileUpload();
        }
        else
        {
            _toastService.ShowError(rsp.GetMessage());
        }
        _profileBusy = false;
    }

    private void CancelProfileUpload()
    {
        _profileUploadModel = new();

        _profilePreviewUrl = null;
    }

}

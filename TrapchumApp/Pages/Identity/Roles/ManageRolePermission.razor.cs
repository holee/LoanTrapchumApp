using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Auths;

namespace TrapchumApp.Pages.Identity.Roles;

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

    bool _selectAll = false;

    private List<RolePermissionsDto>? _roleClaimsGrouped = [];

    private Dictionary<string, bool> _originalPermissionStates = [];

    private List<string> groups = [];
    protected override async Task OnInitializedAsync()
    {
        await GetPermissions();

        await base.OnInitializedAsync();
    }

    private async Task GetPermissions()
    {
       Tracker.Show();
        var rsp = await _roleService.GetPermissionsByRoleByIdAsync(Id);

        if (rsp.IsSuccess && rsp.Data is not null)
        {
            rolePermissions = rsp.Data;

            _roleClaimsGrouped = rolePermissions.Permissions?
                                    .GroupBy(g => g.Group)
                                    .Select(x => new RolePermissionsDto
                                    {
                                        Group = x.Key,
                                        Permissions = x.ToList(),
                                        GroupCheck = x.All(y => y.IsAssignedToRole)
                                    }).ToList();
            UpdateGlobalSelectAll();

            _originalPermissionStates = rolePermissions.Permissions!
                .ToDictionary(p => $"{p.Group}_{p.ClaimValue}", p => p.IsAssignedToRole);

            groups = _roleClaimsGrouped?.Select(x => x.Group!).ToList() ?? [];

            groups.Add("All");
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
        }

        Tracker.Hide();
    }


    private void UpdateGlobalSelectAll()
    {
        _selectAll = _roleClaimsGrouped!.All(g => g.Permissions.All(p => p.IsAssignedToRole));
    }


    private void OnSelectAllChanged(bool isChecked)
    {
        _selectAll = isChecked;

        foreach (var group in _roleClaimsGrouped ?? [])
        {
            group.GroupCheck = isChecked;

            foreach (var permission in group.Permissions)
            {
                permission.IsAssignedToRole = isChecked;
            }
        }
    }
    private void OnGroupSelectAllChanged(RolePermissionsDto group, bool isChecked)
    {
        group.GroupCheck = isChecked;

        foreach (var permission in group.Permissions)
        {
            permission.IsAssignedToRole = isChecked;
        }

        // Update global select all
        UpdateGlobalSelectAll();
    }

    void OnCheckChanged(RolePermissionsViewModel arg)
    {
        var gr = _roleClaimsGrouped?.Where(x => x.Group == arg.Group).FirstOrDefault();

        gr!.Permissions.Where(x => x.ClaimValue == arg.ClaimValue && x.ClaimType == arg.ClaimType)
                      .First().IsAssignedToRole = arg.IsAssignedToRole;

        gr!.GroupCheck = gr.Permissions.All(y => y.IsAssignedToRole == true);

        UpdateGlobalSelectAll();

    }

    private void RestoreDefaults()
    {
        foreach (var group in _roleClaimsGrouped ?? [])
        {
            foreach (var permission in group.Permissions)
            {
                var key = $"{permission.Group}_{permission.ClaimValue}";
                if (_originalPermissionStates.TryGetValue(key, out var original))
                    permission.IsAssignedToRole = original;
            }
            group.GroupCheck = group.Permissions.All(p => p.IsAssignedToRole);
        }
        UpdateGlobalSelectAll();
    }

    private void RestoreGroupDefaults(RolePermissionsDto group)
    {
        foreach (var permission in group.Permissions)
        {
            var key = $"{permission.Group}_{permission.ClaimValue}";
            if (_originalPermissionStates.TryGetValue(key, out var original))
                permission.IsAssignedToRole = original;
        }
        group.GroupCheck = group.Permissions.All(p => p.IsAssignedToRole);
        UpdateGlobalSelectAll();
    }

    private bool HasChanges()
    {
        if (_roleClaimsGrouped is null) return false;
        foreach (var group in _roleClaimsGrouped)
        {
            foreach (var permission in group.Permissions)
            {
                var key = $"{permission.Group}_{permission.ClaimValue}";
                if (_originalPermissionStates.TryGetValue(key, out var original) && permission.IsAssignedToRole != original)
                    return true;
            }
        }
        return false;
    }

    protected async Task SubmitHandler()
    {
        var request = new RolePermissionRequest(Id, rolePermissions!.Permissions);

        formEdit?.EditContext?.Validate();

        var rsp = await _roleService.AddRolePermissions(request);

        if (rsp!.IsSuccess)
        {
            NotificationService.Notify(NotificationSeverity.Success, "Permissions were assigned.");

            NavigationManager.NavigateTo("/App/Account/Roles");
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
        }
    }



}

//protected List<string> groups = [];
//protected Dictionary<string, List<RolePermissionsViewModel>> RoleClaimsGrouped { get; } = new();
//foreach (var permission in rolePermissions.Permissions!)
//{
//    if (RoleClaimsGrouped.ContainsKey(permission.Group))
//    {
//        RoleClaimsGrouped[permission.Group].Add(permission);
//    }
//    else
//    {
//        RoleClaimsGrouped.Add(permission.Group, new List<RolePermissionsViewModel> { permission });
//    }
//}

//if (RoleClaimsGrouped.Count > 0)
//{

//    groups = RoleClaimsGrouped.Select(x => x.Key).ToList();

//}
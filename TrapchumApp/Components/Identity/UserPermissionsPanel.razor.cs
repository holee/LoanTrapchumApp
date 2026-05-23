using Applications.Responses.ViewModels.Auths;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Identity;

public partial class UserPermissionsPanel
{
    [Parameter, EditorRequired] public bool IsLoading { get; set; }
    [Parameter, EditorRequired] public bool IsBusy { get; set; }
    [Parameter, EditorRequired] public bool PermSelectAll { get; set; }
    [Parameter, EditorRequired] public string GroupFilter { get; set; } = string.Empty;
    [Parameter, EditorRequired] public IEnumerable<KeyValuePair<string, List<UserPermissionViewModel>>> FilteredGroups { get; set; } = [];

    [Parameter] public EventCallback<bool> SelectAllChanged { get; set; }
    [Parameter] public EventCallback<string> GroupFilterChanged { get; set; }
    [Parameter] public EventCallback<GroupToggleArgs> GroupSelectAllChanged { get; set; }
    [Parameter] public EventCallback<PermissionChangeArgs> PermissionChanged { get; set; }
    [Parameter] public EventCallback SavePermissions { get; set; }

    private Task OnSelectAllClicked(bool value) => SelectAllChanged.InvokeAsync(value);
    private Task OnFilterInput(string value) => GroupFilterChanged.InvokeAsync(value);
    private Task OnGroupToggle(string groupName, bool isChecked) =>
        GroupSelectAllChanged.InvokeAsync(new GroupToggleArgs(groupName, isChecked));
    private Task OnPermCheck(UserPermissionViewModel item, bool isChecked) =>
        PermissionChanged.InvokeAsync(new PermissionChangeArgs(item, isChecked));

    public record GroupToggleArgs(string GroupName, bool IsChecked);
    public record PermissionChangeArgs(UserPermissionViewModel Item, bool IsChecked);
}

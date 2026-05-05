using Applications.Features.Identity.Roles.Commands.AddRolePermissions;
using Applications.Features.Identity.Roles.Commands.UpdateRole;
using Applications.Features.Identity.Roles.Queries.EditRole;
using Applications.Features.Identity.Roles.Queries.GetAllPermissions;
using Applications.Features.Identity.Roles.Queries.GetPermissionsByRole;
using Applications.Features.Identity.Roles.Requests;
using Applications.Features.Identity.Users;
using Applications.Responses.ViewModels.Auths;
using Applications.Utils;
using TrapchumApp.Helpers.Validations;

namespace TrapchumApp.Pages.Identity.Roles;
public partial class UpdateRole
{
    [Parameter] public string? RoleId { get; set; }

    private RoleDtoUpdate model = new RoleDtoUpdate();
    private List<RolePermissionsDtoCreate> RoleClaimsGrouped { get; set; } = [];

    private EditForm roleCreateForm = default!;

    private readonly IMediator _mediator;

    public UpdateRole(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected override async Task OnInitializedAsync()
    {
        var roleTask = _mediator.Send(new EditRoleQuery(RoleId!));
        var allPermissionsTask = _mediator.Send(new GetAllPermissionsQuery());
        var assignedTask = _mediator.Send(new GetPermissionsByRoleQuery(RoleId!));

        await Task.WhenAll(roleTask, allPermissionsTask, assignedTask);

        var roleRsp = await roleTask;
        if (!roleRsp.IsError && roleRsp.Value is not null)
            model = roleRsp.Value;

        var allPermissions = await allPermissionsTask;
        var assignedRsp = await assignedTask;

        var assignedValues = assignedRsp.IsError
            ? []
            : assignedRsp.Value.Permissions?
                .Where(p => p.IsAssignedToRole)
                .Select(p => p.ClaimValue)
                .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];

        foreach (var p in allPermissions)
            p.IsSelected = assignedValues.Contains(p.ClaimValue);

        RoleClaimsGrouped = allPermissions
            .GroupBy(p => p.Group)
            .Select(g => new RolePermissionsDtoCreate
            {
                Group = g.Key,
                Permissions = g.ToList(),
                GroupCheck = g.All(p => p.IsSelected)
            }).ToList();

        await base.OnInitializedAsync();
    }

    protected async Task CreateRole()
    {
        if (roleCreateForm.EditContext!.Validate() == false) return;

        var updateRsp = await _mediator.Send(new UpdateRoleCommand(model));

        if (updateRsp.IsError)
        {
            roleCreateForm.EditContext.ApplyValidationSummary<RoleDtoUpdate>(updateRsp.FirstError.Description);
            return;
        }

        var permissions = RoleClaimsGrouped
            .SelectMany(g => g.Permissions)
            .Select(p => new RolePermissionsViewModel
            {
                RoleId = model.RoleId,
                ClaimType = p.ClaimType,
                ClaimValue = p.ClaimValue,
                Description = p.Description,
                Group = p.Group,
                IsAssignedToRole = p.IsSelected
            }).ToList();

        var permRsp = await _mediator.Send(new AddRolePermissionsCommand(new RolePermissionRequest(model.RoleId, permissions)));

        if (permRsp.IsError)
        {
            NotificationService.Notify(NotificationSeverity.Warning, permRsp.FirstError.Description);
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, updateRsp.Value);
        DialogService.Close(true);
    }
}

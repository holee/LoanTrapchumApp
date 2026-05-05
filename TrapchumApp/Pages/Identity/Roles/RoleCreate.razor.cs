using Applications.Features.Identity.Roles.Commands.CreateRole;
using Applications.Features.Identity.Roles.Queries.GetAllPermissions;
using Applications.Features.Identity.Roles.Requests;
using Applications.Responses.ViewModels.Auths;
using Applications.Utils;

namespace TrapchumApp.Pages.Identity.Roles;

public partial class RoleCreate
{
    private RoleCreateDto model = new RoleCreateDto();

    private EditForm roleCreateForm = default!;
    private List<RolePermissionsDtoCreate>? RoleClaimsGrouped { get; set; } = [];

    private bool visible = false;

    private readonly IMediator _mediator;

    public RoleCreate(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected override async Task OnInitializedAsync()
    {
        var permissions = await _mediator.Send(new GetAllPermissionsQuery());

        RoleClaimsGrouped = permissions
            .GroupBy(p => p.Group)
            .Select(g => new RolePermissionsDtoCreate
            {
                Group = g.Key,
                Permissions = g.ToList()
            }).ToList();

        await base.OnInitializedAsync();
    }

    protected async Task CreateRole()
    {

        if (roleCreateForm.EditContext!.Validate() == false) return;

        var permissions = RoleClaimsGrouped?
                    .SelectMany(x => x.Permissions)
                    .Where(x => x.IsSelected == true)
                    .Select(x => new RolePermissionsViewModel
                    {
                        ClaimType = x.ClaimType,
                        ClaimValue = x.ClaimValue,
                        Description = x.Description,
                        Group = x.Group,
                        IsAssignedToRole = x.IsSelected,
                    }).OrderBy(x=>x.Group)
                    .ToList();

        model.Permissions = permissions ?? [];

        var result = await _mediator.Send(new CreateRoleCommand(model));

        if (result.IsError)
        {
            NotificationService.Notify(NotificationSeverity.Error, result.FirstError.Description);
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "New Role was added.");

    }


}

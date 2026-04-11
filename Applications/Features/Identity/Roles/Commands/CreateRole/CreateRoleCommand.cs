using Applications.Features.Identity.Roles.Requests;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Identity.Roles.Commands.CreateRole;

public sealed record CreateRoleCommand(RoleCreateDto Role) : IRequest<ErrorOr<RoleClaims>>;

using Applications.Constants.Authorizations;
using Applications.Interfaces.Identity;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Domains.Models.Identity;
using FluentValidation;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Applications.Features.Identity.Roles.Requests;
using Applications.Features.Identity.Roles.Responses;
using Applications.Features.Identity.Users;
using Applications.Commons.ExceptionHandlers;
using ErrorOr;
using Infrastructures.Repository.Extensions;
namespace Infrastructures.Repository;

public sealed class RoleRepository(
    RoleManager<ApplicationRole> roleManager,
    UserDataContext contextFactory,
    IValidator<RoleCreateDto> validator) : IRoleService
{
    public async Task<ErrorOr<Success>> CreateAsync(RoleCreateDto role)
    {
        var validationResult = await validator.ValidateAsync(role);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
                .ToList();
            return errors;
        }

        var existingRole = await roleManager.FindByNameAsync(role.RoleName!);

        if (existingRole is not null)
            return Error.Conflict("Role.Conflict", $"{role.RoleName} Role already existing!");

        var newRole = new ApplicationRole
        {
            Name = role.RoleName,
            Description = role.Description,
            IsActive = true
        };

        var result = await roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();

        var addedRole = await roleManager.FindByNameAsync(role.RoleName!);

        if (role.Permissions.Any() && addedRole is not null)
        {
            var claims = role.Permissions.Select(x => new ApplicationRoleClaim
            {
                RoleId = addedRole.Id,
                ClaimType = x.ClaimType,
                ClaimValue = x.ClaimValue,
                Description = x.Description,
            });

            var context = contextFactory;
            context.ApplicationRoleClaim.AddRange(claims);
            await context.SaveChangesAsync();
        }

        return Result.Success;
    }

    public async Task<ErrorOr<RoleClaims>> CreateAsync(RoleCreateDto role, CancellationToken cancellationToken = default)
    {
        var existingRole = await roleManager.FindByNameAsync(role.RoleName!);

        if (existingRole is not null)
            return Error.Conflict("Role.Conflict", $"Role '{role.RoleName}' already exists.");

        var newRole = new ApplicationRole
        {
            Name = role.RoleName,
            Description = role.Description,
            IsActive = true,
        };

        var result = await roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();

        if (role.Permissions.Count != 0)
        {
            var claimEntities = role.Permissions.Select(x => new ApplicationRoleClaim
            {
                RoleId = newRole.Id,
                ClaimType = x.ClaimType,
                ClaimValue = x.ClaimValue,
                Description = x.Description,
            });

            var context = contextFactory;
            context.ApplicationRoleClaim.AddRange(claimEntities);
            await context.SaveChangesAsync(cancellationToken);
        }

        var roleClaims = new RoleClaims(
            newRole.Id,
            newRole.Name!,
            role.Permissions
                .Select(x => new RoleClaimDto
                {
                    ClaimType = x.ClaimType,
                    IsSelected = true,
                })
                .ToList()
        );

        return roleClaims;
    }

    public async Task<ErrorOr<string>> UpdateAsync(RoleDtoUpdate role)
    {
        var existingRole = await roleManager.FindByIdAsync(role.RoleId);

        if (existingRole is null)
            return Error.NotFound("Role.NotFound", $"{role.RoleId} Not Found.");

        existingRole.Name = role.RoleName;
        existingRole.Description = role.Description;

        var result = await roleManager.UpdateAsync(existingRole);

        if (!result.Succeeded)
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();

        return "Role was updated.";
    }

    public async Task<ErrorOr<Deleted>> DeleteRoleAsync(string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);

        if (role is null)
            return Error.NotFound("Role.NotFound", $"Role with Id '{roleId}' not found.");

        var context = contextFactory;
        var isInUse = await context.UserRoles.AnyAsync(ur => ur.RoleId == roleId);

        if (isInUse)
            return Error.Conflict("Role.Conflict", "Cannot delete this role because it is assigned to one or more users.");

        role.IsActive = false;

        var result = await roleManager.UpdateAsync(role);

        if (!result.Succeeded)
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();

        return Result.Deleted;
    }

    public async Task<ErrorOr<RoleDtoUpdate>> EditAsync(string? roleId)
    {
        if (roleId is null)
            return Error.Validation("Role.Validation", "RoleId Is Required.");

        var existingRole = await roleManager.FindByIdAsync(roleId);

        if (existingRole is null)
            return Error.NotFound("Role.NotFound", $"{roleId} Not Found.");

        return new RoleDtoUpdate
        {
            RoleId = roleId,
            RoleName = existingRole.Name!,
            Description = existingRole.Description!
        };
    }

    public async Task<ErrorOr<Success>> AddRolePermissions(RolePermissionRequest permission)
    {
        var roleInDb = await roleManager.FindByIdAsync(permission.RoleId.ToString());

        if (roleInDb is null)
            return Error.NotFound("Role.NotFound", "No Role Found.");

        var permissionsToBeAssigned = permission.RoleClaims!
            .Where(rc => rc.IsAssignedToRole == true)
            .ToList();

        var currentlyAssignedClaims = await roleManager.GetClaimsAsync(roleInDb);

        foreach (var claim in currentlyAssignedClaims)
            await roleManager.RemoveClaimAsync(roleInDb, claim);

        var context = contextFactory;
        foreach (var claim in permissionsToBeAssigned)
        {
            var mappedRoleClaim = claim.Adapt<ApplicationRoleClaim>();
            await context.RoleClaims.AddAsync(mappedRoleClaim);
        }

        await context.SaveChangesAsync();

        return Result.Success;
    }

    public async Task AddRolePermissions(string? roleId, RolePermissionRequest permission)
    {
        if (string.IsNullOrEmpty(roleId))
            throw new ArgumentException("RoleId is required.", nameof(roleId));

        var roleInDb = await roleManager.FindByIdAsync(roleId);

        if (roleInDb is null)
            throw new NotFoundException($"No Role Found with Id: {roleId}");

        var permissionsToBeAssigned = permission.RoleClaims!
            .Where(rc => rc.IsAssignedToRole == true)
            .ToList();

        var currentlyAssignedClaims = await roleManager.GetClaimsAsync(roleInDb);

        foreach (var claim in currentlyAssignedClaims)
            await roleManager.RemoveClaimAsync(roleInDb, claim);

        var context = contextFactory;
        foreach (var claim in permissionsToBeAssigned)
        {
            var mappedRoleClaim = claim.Adapt<ApplicationRoleClaim>();
            await context.RoleClaims.AddAsync(mappedRoleClaim);
        }

        await context.SaveChangesAsync();
    }

    public async Task<ErrorOr<IEnumerable<RoleDto>>> GetRolesAsync(string? roleId = null, string? name = null)
    {
        var context = contextFactory;
        var queries = context.Roles
            .OrderBy(r => r.Name)
            .Where(x => x.IsActive)
            .Select(x => new RoleDto
            {
                Id = x.Id,
                RoleName = x.Name,
                IsActive = x.IsActive,
                Description = x.Description
            });

        if (!string.IsNullOrEmpty(roleId))
            queries = queries.Where(x => x.Id == roleId);

        if (!string.IsNullOrEmpty(name))
            queries = queries.Where(x => EF.Functions.Like(x.RoleName, $"%{name}%"));

        return (await queries.AsNoTracking().ToListAsync()).AsEnumerable().ToErrorOr();
    }

    public async Task<Pager<RoleDto>> GetRolesAsync(string? roleId, QueryParam param, CancellationToken cancellation = default)
    {
        var context = contextFactory;
        var queries = context.Roles
            .OrderBy(r => r.Name)
            .Where(x => x.IsActive)
            .Select(x => new RoleDto
            {
                Id = x.Id,
                RoleName = x.Name,
                IsActive = x.IsActive,
                Description = x.Description
            });

        if (!string.IsNullOrEmpty(roleId))
            queries = queries.Where(x => x.Id == roleId);

        if (!string.IsNullOrEmpty(param.Filter))
            queries = queries.Where(x => EF.Functions.Like(x.RoleName, $"%{param.Filter}%"));

        if (param.SortDescending)
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));

        var counts = await queries.CountAsync(cancellation);

        if (param.Take > 0)
            queries = queries.Skip(param.Page).Take(param.Take);

        var items = await queries.AsNoTracking().ToListAsync(cancellation);

        return new Pager<RoleDto>(items, counts);
    }

    public async Task<ErrorOr<IEnumerable<string>>> GetRolesAsync(string? name = null)
    {
        var context = contextFactory;
        var queries = context.Roles.Where(x => x.IsActive);

        if (!string.IsNullOrEmpty(name))
            queries = queries.Where(x => EF.Functions.Like(x.Name!, $"%{name}%"));

        var roles = await queries
            .AsNoTracking()
            .Select(x => x.Name)
            .ToListAsync();

        return roles.OfType<string>().AsEnumerable().ToErrorOr();
    }

    public async Task<Pager<RoleDto>> GetRolesAsync(QueryParam param, CancellationToken cancellation = default)
    {
        var context = contextFactory;

        var queries = context.Roles.Where(x => x.IsActive);

        if (!string.IsNullOrEmpty(param.Filter))
        {
            var filter = param.Filter.Trim();

            string[] columns = [nameof(ApplicationRole.Name), nameof(ApplicationRole.Description)];

            queries = queries.StartWiths(filter, columns);

        }


        var counts = await queries.CountAsync(cancellation);

        if (param.Take > 0)
            queries = queries.Skip(param.Page).Take(param.Take);

        var roles = await queries
            .AsNoTracking()
            .Select(x => ProjectTo(x)).ToListAsync();

        return PagerExtensions.Create(roles, counts);
    }

    private static RoleDto ProjectTo(ApplicationRole x) => new()
    {
        Id = x.Id,
        RoleName = x.Name,
        IsActive = x.IsActive,
        Description = x.Description
    };

    public async Task<ErrorOr<RoleDto>> GetRoleByIdAsync(string id)
    {
        var context = contextFactory;
        var role = await context.Roles
            .Where(x => x.IsActive)
            .Select(x => new RoleDto
            {
                Id = x.Id,
                RoleName = x.Name,
                IsActive = x.IsActive
            }).AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (role is null)
            return Error.NotFound("Role.NotFound", "No role found.");

        return role;
    }

    public async Task<ErrorOr<RolePermissions>> GetPermissionsByRoleAsync(string roleId)
    {
        return await GetPermissionsByRoleInternalAsync(roleId, ApplicationPermissions.AllPermissions, p => p.Group);
    }

    public async Task<ErrorOr<RolePermissions>> GetPermissionsByRoleByIdAsync(string roleId)
    {
        return await GetPermissionsByRoleInternalAsync(roleId, AppPermissions.AllPermissions, p => p.Module);
    }

    private async Task<ErrorOr<RolePermissions>> GetPermissionsByRoleInternalAsync(string roleId,
        IEnumerable<dynamic> allPermissions,
        Func<dynamic, string> groupSelector)
    {
        var context = contextFactory;
        var roleInDb = await context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        if (roleInDb is null)
            return Error.NotFound("Role.NotFound", "Role not found in the system.");

        var roleClaimResponse = new RolePermissions(
            new(roleId, roleInDb.Name!, roleInDb.Description!),
            new());

        var currentRoleClaims = await GetAllClaimsForRoleAsync(roleId);

        var assignedClaimValues = new HashSet<string>(
            currentRoleClaims.Select(crc => crc.ClaimValue),
            StringComparer.OrdinalIgnoreCase);

        foreach (var permission in allPermissions)
        {
            roleClaimResponse.Permissions!.Add(new RolePermissionsViewModel
            {
                RoleId = roleId,
                ClaimType = AppClaim.Permission,
                ClaimValue = permission.For,
                Description = permission.Description,
                Group = groupSelector(permission),
                IsAssignedToRole = assignedClaimValues.Contains(permission.For)
            });
        }

        return roleClaimResponse;
    }

    public List<RolePermissionsViewModel> GetAllPermisses()
    {
        var allPermissions = ApplicationPermissions.AllPermissions;

        List<RolePermissionsViewModel> permissionsViewModels = new();

        foreach (var permission in allPermissions)
        {
            permissionsViewModels.Add(new RolePermissionsViewModel
            {
                RoleId = string.Empty,
                ClaimType = AppClaim.Permission,
                ClaimValue = permission.For,
                Description = permission.Description,
                Group = permission.Group,
                IsAssignedToRole = false
            });
        }

        return permissionsViewModels;
    }

    public List<PermissionsDtoCreate> GetAllPermissions()
    {
        var allPermissions = AppPermissions.AllPermissions;

        List<PermissionsDtoCreate> permissionsViewModels = new();

        foreach (var permission in allPermissions)
        {
            permissionsViewModels.Add(new PermissionsDtoCreate
            {
                ClaimType = AppClaim.Permission,
                ClaimValue = permission.For,
                Description = permission.Description,
                Group = permission.Module,
                IsSelected = false
            });
        }

        return permissionsViewModels;
    }

    private async Task<List<RolePermissionsViewModel>> GetAllClaimsForRoleAsync(string roleId)
    {
        var context = contextFactory;
        var roleClaims = await context.RoleClaims
            .Where(rc => rc.RoleId == roleId)
            .ToListAsync();

        if (roleClaims.Any())
            return roleClaims.Adapt<List<RolePermissionsViewModel>>();

        return [];
    }
}
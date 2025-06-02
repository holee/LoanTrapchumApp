using Applications.Constants.Authorizations;
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Auths;
using Domains.Models.Identity;
using FluentValidation;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public class RoleService(
    RoleManager<ApplicationRole> roleManager,
    UserDataContext context,
    IValidator<RoleCreateDto> validator) : IRoleService
{
    public async Task<Result> CreateAsync(RoleCreateDto role)
    {
        var validationResult = await validator.ValidateAsync(role);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(err => err.ErrorMessage).ToList();
            var errString = string.Join(Environment.NewLine, errors);
            return Result.Failure(ErrorResponse.Validation(errString));
        }

        var existingRole = await roleManager.FindByNameAsync(role.RoleName!);

        if (existingRole is not null)
        {
            return Result.Failure(ErrorResponse.Conflict($"{role.RoleName} Role already existing!"));
        }

        var newRole = new ApplicationRole
        {
            Name = role.RoleName,
            IsActive = true
        };

        var result = await roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();
            var errString = string.Join(Environment.NewLine, errors);
            return Result.Failure(ErrorResponse.Validation(errString));
        }

        return Result.Success();
    }

    public async Task<Result> AddRolePermissions(RolePermissionRequest permission)
    {
        var roleInDb = await roleManager.FindByIdAsync(permission.RoleId.ToString());

        if (roleInDb is null)
        {
            return Result.Failure(ErrorResponse.NotFound("No Role Found."));
        }

        var permissionsToBeAssigned = permission!.RoleClaims!
            .Where(rc => rc.IsAssignedToRole == true)
            .ToList();
        var currentlyAssignedClaims = await roleManager.GetClaimsAsync(roleInDb);

        // Drop
        foreach (var claim in currentlyAssignedClaims)
        {
            await roleManager.RemoveClaimAsync(roleInDb, claim);
        }

        // Add
        foreach (var claim in permissionsToBeAssigned)
        {
            var mappedRoleClaim = claim.Adapt<ApplicationRoleClaim>();

            await context.RoleClaims.AddAsync(mappedRoleClaim);
        }

        await context.SaveChangesAsync();

        return Result.Success("Role permissions updated successfully.");
    }

    public async Task<Result<IEnumerable<RoleDto>>> GetRolesAsync(string? roleId = null, string? name = null)
    {
        var queries = roleManager.Roles
            .Where(x => x.IsActive)
            .Select(x => new RoleDto
            {
                Id = x.Id,
                RoleName = x.Name,
                IsActive = x.IsActive,
                Description = x.Description
            });

        if (!string.IsNullOrEmpty(roleId))
        {
            queries = queries.Where(x => x.Id == roleId);
        }

        if (!string.IsNullOrEmpty(name))
        {
            queries = queries.Where(x => EF.Functions.Like(x.RoleName, $"%{name}%"));
        }

        return Result.Success<IEnumerable<RoleDto>>(await queries.AsNoTracking().ToListAsync());
    }


    public async Task<Result<IEnumerable<string>>> GetRolesAsync(string? name = null)
    {
        var queries = roleManager.Roles.Where(x => x.IsActive);

        if (!string.IsNullOrEmpty(name))
        {
            queries = queries.Where(x => EF.Functions.Like(x.Name!, $"%{name}%"));
        }

        var roles = await queries
                            .AsNoTracking()
                            .Select(x=>x.Name)
                            .ToListAsync();

        if (!roles.Any())
        {
            return Result.Success<IEnumerable<string>>(Enumerable.Empty<string>());
        }

        return Result.Success<IEnumerable<string>>(roles!);
    }


    public async Task<Result<RoleDto>> GetRoleByIdAsync(string id)
    {
        var role = await roleManager.Roles
            .Where(x => x.IsActive)
            .Select(x => new RoleDto
            {
                Id = x.Id,
                RoleName = x.Name,
                IsActive = x.IsActive
            }).AsNoTracking()
            .FirstOrDefaultAsync();

        if (role is null) return Result.Failure<RoleDto>(ErrorResponse.NotFound("No role found."));

        return Result.Success<RoleDto>(role);
    }


    public async Task<Result<RolePermissions>> GetPermissionsByRoleAsync(string roleId)
    {
        var roleInDb = await roleManager.FindByIdAsync(roleId.ToString());

        if (roleInDb is null)
        {
            return Result.Failure<RolePermissions>(ErrorResponse.NotFound("There is any roles in the system."));
        }

        var allPermissions = ApplicationPermissions.AllPermissions;

        var roleClaimResponse = new RolePermissions(new(roleId, roleInDb.Name!, roleInDb.Description!), new());

        var currentRoleClaims = await GetAllClaimsForRoleAsync(roleId);

        var allPermissionsNames = allPermissions.Select(p => p.For).ToList();

        var currentRoleClaimsValues = currentRoleClaims.Select(crc => crc.ClaimValue).ToList();

        var currentlyAssignedRoleClaimsNames = allPermissionsNames
            .Intersect(currentRoleClaimsValues)
            .ToList();

        foreach (var permission in allPermissions)
        {
            if (currentlyAssignedRoleClaimsNames.Any(carc => carc == permission.For))
            {
                roleClaimResponse.Permissions!.Add(new RolePermissionsViewModel
                {
                    RoleId = roleId,
                    ClaimType = AppClaim.Permission,
                    ClaimValue = permission.For,
                    Description = permission.Description,
                    Group = permission.Group,
                    IsAssignedToRole = true
                });
            }
            else
            {
                roleClaimResponse!.Permissions!.Add(new RolePermissionsViewModel
                {
                    RoleId = roleId,
                    ClaimType = AppClaim.Permission,
                    ClaimValue = permission.For,
                    Description = permission.Description,
                    Group = permission.Group,
                    IsAssignedToRole = false
                });
            }
        }

        return Result.Success(roleClaimResponse);
    }

    private async Task<List<RolePermissionsViewModel>> GetAllClaimsForRoleAsync(string roleId)
    {
        var roleClaims = await context.RoleClaims
            .Where(rc => rc.RoleId == roleId)
            .ToListAsync();
        if (roleClaims.Any())
        {
            var mappedRolePermissions = roleClaims.Adapt<List<RolePermissionsViewModel>>();
            return mappedRolePermissions;
        }

        return new List<RolePermissionsViewModel>();
    }
}
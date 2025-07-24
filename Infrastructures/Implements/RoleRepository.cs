using Applications.Constants.Authorizations;
using Applications.Interfaces.Identity;
using Applications.Requests.Identity;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Domains.Models.Identity;
using FluentValidation;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace Infrastructures.Implements;

public sealed class RoleRepository(
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
            Description = role.Description,
            IsActive = true
        };

        var result = await roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();

            var errString = string.Join(Environment.NewLine, errors);

            return Result.Failure(ErrorResponse.Validation(errString));
        }

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

            context.ApplicationRoleClaim.AddRange(claims);

            await context.SaveChangesAsync();
        }

        return Result.Success();
    }


    public async Task<Result<string>> UpdateAsync(RoleDtoUpdate role)
    {

        var existingRole = await roleManager.FindByIdAsync(role.RoleId);

        if (existingRole is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"{role.RoleId} Not Found."));
        }

        existingRole.Name = role.RoleName;

        existingRole.Description = role.Description;

        var result = await roleManager.UpdateAsync(existingRole);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();

            var errString = string.Join(Environment.NewLine, errors);

            return Result.Failure<string>(ErrorResponse.Validation(errString));
        }

        return Result.Success("Role was updated.");
    }

    public async Task<Result<RoleDtoUpdate>> EditAsync(string? roleId)
    {

        if (roleId == null)
        {
            return Result.Failure<RoleDtoUpdate>(ErrorResponse.Validation($"{roleId} Is Required."));
        }

        var existingRole = await roleManager.FindByIdAsync(roleId);

        if (existingRole is null)
        {
            return Result.Failure<RoleDtoUpdate>(ErrorResponse.NotFound($"{roleId} Not Found."));
        }

        var editRole = new RoleDtoUpdate
        {
            RoleId = roleId,
            RoleName = existingRole.Name!,
            Description = existingRole.Description!
        };

        return Result.Success<RoleDtoUpdate>(editRole);

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
        {
            queries = queries.Where(x => x.Id == roleId);
        }

        if (!string.IsNullOrEmpty(name))
        {
            queries = queries.Where(x => EF.Functions.Like(x.RoleName, $"%{name}%"));
        }

        return Result.Success<IEnumerable<RoleDto>>(await queries.AsNoTracking().ToListAsync());
    }


    public async Task<Pager<RoleDto>> GetRolesAsync(string? roleId, QueryParam param, CancellationToken cancellation = default)
    {
        var queries = roleManager.Roles
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
        {
            queries = queries.Where(x => x.Id == roleId);
        }

        if (!string.IsNullOrEmpty(param.Filter))
        {
            queries = queries.Where(x => EF.Functions.Like(x.RoleName, $"%{param.Filter}%"));
        }

        if (param.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        var counts = await queries.CountAsync(cancellation);


        if (param.PageSize > 0)
        {
            queries = queries.Skip(param.Skip).Take(param.PageSize);
        }
        var items = await queries.AsNoTracking().ToListAsync(cancellation);

        return new Pager<RoleDto>(items, counts);
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
                            .Select(x => x.Name)
                            .ToListAsync();

        if (!roles.Any())
        {
            return Result.Success<IEnumerable<string>>([]);
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





    public async Task<Result<RolePermissions>> GetPermissionsByRoleByIdAsync(string roleId)
    {
        var roleInDb = await roleManager.FindByIdAsync(roleId.ToString());

        if (roleInDb is null)
        {
            return Result.Failure<RolePermissions>(ErrorResponse.NotFound("There is any roles in the system."));
        }

        var allPermissions = AppPermissions.AllPermissions;

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
                    Group = permission.Module,
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
                    Group = permission.Module,
                    IsAssignedToRole = false
                });
            }
        }

        return Result.Success(roleClaimResponse);
    }


    public List<RolePermissionsViewModel> GetAllPermisses()
    {
        var allPermissions = ApplicationPermissions.AllPermissions;

        List<RolePermissionsViewModel> permissionsViewModels = new();

        if (allPermissions.Any())
        {
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
        }

        return permissionsViewModels;
    }



    public List<PermissionsDtoCreate> GetAllPermissions()
    {
        var allPermissions = AppPermissions.AllPermissions;

        List<PermissionsDtoCreate> permissionsViewModels = new();

        if (allPermissions.Any())
        {
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
        }

        return permissionsViewModels;
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
using Applications.Bases;
using Applications.Constants.Authorizations;
using Applications.Extensions;
using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses.Auths;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Dapper;
using Domains.Models.Files;
using Domains.Models.Identity;
using Domains.Shared;
using ErrorOr;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Infrastructures.Implements;

public sealed class UserRepository : IUserService
{
    private readonly IDbContextFactory<UserDataContext> _userContextFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signinManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IHostingEnvironment _env;
    private readonly IDbContextFactory<DataContext> _cxtFactory;
    private readonly IDapperConnectionFactory connectionFactory;
    public UserRepository(UserManager<ApplicationUser> userManager,
         RoleManager<ApplicationRole> roleManager,
         IDbContextFactory<UserDataContext> userContextFactory,
         IHostingEnvironment env,
         SignInManager<ApplicationUser> signinManager,
         IDbContextFactory<DataContext> dbContextFactory,
         IDapperConnectionFactory connectionFactory)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userContextFactory = userContextFactory;
        _env = env;
        _signinManager = signinManager;
        _cxtFactory = dbContextFactory;
        this.connectionFactory = connectionFactory;
    }
    public async Task<ErrorOr<string>> CreateAsync(RegisterRequest model, CancellationToken cancellation = default)
    {
        if (string.IsNullOrWhiteSpace(model.EmployeeId))
            return Error.Validation(description: "Employee is required.");

        if (await IsExistEmployeeHasAccount(model.EmployeeId))
            return Error.Conflict(description: "Employee already registered.");

        var existingUser = await GetUserByName(model.Username);
        if (existingUser is not null)
            return Error.Conflict(description: $"{model.Username} already exists.");

        var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
        if (userWithSameEmail is not null)
            return Error.Conflict(description: $"{model.Email} email already exists.");

        var user = new ApplicationUser
        {
            UserName = model.Username ?? model.Email,
            Email = model.Email,
            BranchRefId = model.BranchRefId,
            NameRefId = model.EmployeeId,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return result.Errors
                .Select(e => Error.Validation(code: e.Code, description: e.Description))
                .ToList();
        }

        if (!string.IsNullOrEmpty(model.RoleName))
        {

            if (!await _userManager.IsInRoleAsync(user, model.RoleName))
            {
                var assignRole = await _userManager.AddToRoleAsync(user, model.RoleName);

                if (!assignRole.Succeeded)
                {
                    return assignRole.Errors
                        .Select(e => Error.Validation(code: e.Code, description: e.Description))
                        .ToList();
                }
            }
        }
        return "New User was added.";
    }


    public async Task<ErrorOr<DeleteResult>> DeleteAsync(string userId, CancellationToken cancellation = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Error.Validation(description: "UserId is required.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Error.NotFound(description: "No user found with the provided userId.");

        if (await IsUserHasLoanRequest(userId, cancellation) ||
           await IsUserHasLoanDisbursment(userId, cancellation))
        {
            return DeleteResult.InUse("User cannot be deleted because it is associated with existing loan requests, disbursements, or profiles.");
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return result.Errors
                .Select(e => Error.Validation(code: e.Code, description: e.Description))
                .ToList();
        }

        return DeleteResult.Success("User was deleted successfully.");
    }

    public async Task<ErrorOr<Updated>> ToggleUserActiveAsync(string userId, CancellationToken cancellation = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Error.Validation(description: "UserId is required.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Error.NotFound(description: "No user found with the provided userId.");

        user.IsActive = !user.IsActive;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return result.Errors
                .Select(e => Error.Validation(code: e.Code, description: e.Description))
                .ToList();
        }

        return Result.Updated;
    }


    public async Task<ErrorOr<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default)
    {
        if (userId.IsNullOrEmpty())
            return Error.NotFound(description: "There is no user selected.");

        using var db = await connectionFactory.CreateConnectionAsync(cancellation);

        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId);

        var item = await db.QueryFirstOrDefaultAsync<VUser>("SELECT * FROM vUsers WHERE Id=@UserId", parameters);

        return item is null
            ? Error.NotFound(description: Messages.DataNotFound)
            : item;
    }



    public async Task<ErrorOr<Pager<VUser>>> GetUsersAsync(QueryParam param, string? userId = null, CancellationToken cancellation = default)
    {
        await using var context = await _cxtFactory.CreateDbContextAsync(cancellation);

        var queries = context.Database
                            .SqlQuery<VUser>($"SELECT * FROM vUsers")
                            .WhereIf(!param.IncludeInActive, x => x.IsActive == true)
                            .WhereIf(!string.IsNullOrWhiteSpace(param.Filter), e => EF.Functions.Like(e.UserName!, $"%{param.Filter}%") ||
                                                                                    EF.Functions.Like(e.Email!, $"%{param.Filter}%"))
                            .WhereIf(userId.IsNotNullOrEmpty(), u => u.Id == userId);

        if (!string.IsNullOrEmpty(param.SortColumn))
        {
            queries = queries.OrderByDescendingIf(param.SortDescending, p => EF.Property<object>(p, param.SortColumn!));
        }

        var counts = await queries.CountAsync(cancellation);

        if (param.Take > 0)
        {
            queries = queries.Skip(param.Page).Take(param.Take);
        }

        var users = await queries.AsNoTracking().ToListAsync(cancellation);

        return new Pager<VUser>(users, counts);
    }

    public async Task<ErrorOr<Pager<VUser>>> GetUsersAsync(int? branchId, string? userId, QueryParam param, CancellationToken cancellation = default)
    {
        await using var context = await _cxtFactory.CreateDbContextAsync(cancellation);

        var queries = context.Database
                            .SqlQuery<VUser>($"SELECT * FROM vUsers")
                            .WhereIf(branchId > 0, x => x.BranchId == branchId)
                            .WhereIf(!param.IncludeInActive, x => x.IsActive == true)
                            .WhereIf(!string.IsNullOrWhiteSpace(param.Filter), e => EF.Functions.Like(e.UserName!, $"%{param.Filter}%") ||
                                                                                    EF.Functions.Like(e.Email!, $"%{param.Filter}%"))
                            .WhereIf(userId.IsNotNullOrEmpty(), u => u.Id == userId)
                            .OrderByDescendingIf(param.SortDescending, p => EF.Property<object>(p, param.SortColumn!));

        var counts = await queries.CountAsync(cancellation);

        if (param.Take > 0)
            queries = queries.Skip(param.Page).Take(param.Take);

        var users = await queries.AsNoTracking().ToListAsync(cancellation);

        return new Pager<VUser>(users, counts);
    }


    public async Task<ErrorOr<string>> ChangePassword(ChangePasswordDto req, CancellationToken cancellation = default)
    {
        var existingUser = await _userManager.FindByIdAsync(req.UserId);

        if (existingUser is null)
            return Error.NotFound(description: "There is no any user in the system.");

        var changePasswordResult = await _userManager.ChangePasswordAsync(existingUser, req.CurrentPassword, req.NewPassword);

        if (!changePasswordResult.Succeeded)
        {
            return changePasswordResult.Errors
                .Select(e => Error.Validation(code: e.Code, description: e.Description))
                .ToList();
        }

        return "Password was changed successfully.";
    }


    public async Task<ErrorOr<Updated>> UpdateUserRoleAsync(UserRoleDtoUpdate req, CancellationToken cancellation = default)
    {
        var existingUser = await _userManager.FindByIdAsync(req.UserId);
        if (existingUser is null)
            return Error.NotFound(description: "There is no any user in the system.");

        var existingRoles = await _userManager.GetRolesAsync(existingUser);
        if (existingRoles.Any())
            await _userManager.RemoveFromRolesAsync(existingUser, existingRoles);

        var result = await _userManager.AddToRoleAsync(existingUser, req.RoleName);
        if (!result.Succeeded)
        {
            return result.Errors
                .Select(e => Error.Validation(code: e.Code, description: e.Description))
                .ToList();
        }

        return Result.Updated;
    }
    public async Task<ErrorOr<FileModel>> ChangeProfile(string? userId, FileModel? model, CancellationToken cancellation = default)
    {
        if (userId.IsNullOrEmpty())
            return Error.NotFound(description: "There is no any user selected.");

        if (model is null)
            return Error.Validation(description: "No file was provided.");

        await using var userContext = await _userContextFactory.CreateDbContextAsync(cancellation);
        var existingProfile = await userContext.UserProfiles
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.UserRefId == userId, cancellation);

        var uploadResult = await UploadFile(model);

        if (!uploadResult.Success || uploadResult.Model is null)
            return Error.Validation(description: "Error during file upload.");

        var fileResult = uploadResult.Model;

        var userProfile = new UserProfiles
        {
            FileName = fileResult.FileName,
            FileUrl = fileResult.FileUrl,
            IsActive = true,
            UserRefId = userId
        };

        if (existingProfile is null)
            userContext.UserProfiles.Add(userProfile);
        else
            userContext.UserProfiles.Update(userProfile);

        DeleteOldProfileFile(existingProfile?.FileName);

        await userContext.SaveChangesAsync(cancellation);

        return fileResult;
    }

    public async Task<ErrorOr<UserProfileResponse>> GetUserProfile(string? userId, CancellationToken cancellation = default)
    {
        using var db = await connectionFactory.CreateConnectionAsync(cancellation);

        var sql = @"SELECT TOP(1)
				u.Id AS UserId,
				u.BranchName,
				u.Email,
				CONCAT(u.FirstName,' ',u.LastName) AS FullName,
				u.RoleName,
				p.FileUrl
					FROM Security.UserProfiles p
					RIGHT JOIN vUsers u ON p.UserRefId=u.Id
					WHERE u.id=@UserId
				";

        var item = await db.QueryFirstOrDefaultAsync<UserProfileResponse>(sql, new { UserId = userId });

        return item is null
            ? Error.NotFound(description: "No user found.")
            : item;
    }

    public async Task<ErrorOr<Updated>> EditUserAsync(string userId, EditUserDto model, CancellationToken cancellation = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Error.NotFound(description: "User not found.");

        user.Email = model.Email;
        user.UserName = model.UserName;
        user.IsActive = model.IsActive;

        var editResult = await _userManager.UpdateAsync(user);
        if (!editResult.Succeeded)
        {
            return editResult.Errors
                .Select(e => Error.Validation(code: e.Code, description: e.Description))
                .ToList();
        }

        return Result.Updated;
    }
    public async Task<ErrorOr<UserPermissions>> GetUserPermissionsAsync(string? userId, CancellationToken cancellation = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        var userInDb = await _userManager.FindByIdAsync(userId);
        if (userInDb is null)
            return Error.NotFound(description: "No user found in the system.");

        var allPermissions = ApplicationPermissions.UserPermissions;
        var userPermissions = new UserPermissions(userId, []);
        var currentUserClaims = await GetUserClaimssAsync(userId);
        var currentUserClaimsValues = currentUserClaims.Select(c => c.ClaimValue).ToHashSet();

        foreach (var permission in allPermissions)
        {
            userPermissions.Permissions!.Add(new UserPermissionViewModel
            {
                UserId = userId,
                ClaimType = AppClaim.Permission,
                ClaimValue = permission.For,
                Group = permission.Group,
                IsAssignedToRole = currentUserClaimsValues.Contains(permission.For)
            });
        }

        return userPermissions;
    }
    public async Task<ErrorOr<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission, CancellationToken cancellation = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        var userInDb = await _userManager.FindByIdAsync(userId);
        if (userInDb is null)
            return Error.NotFound(description: "No user found.");

        var permissionsToBeAssigned = permission.Permissions!
                                                .Where(rc => rc.IsAssignedToRole)
                                                .ToList();

        var currentlyAssignedClaims = await _userManager.GetClaimsAsync(userInDb);

        foreach (var claim in currentlyAssignedClaims)
            await _userManager.RemoveClaimAsync(userInDb, claim);

        await using var userContext = await _userContextFactory.CreateDbContextAsync(cancellation);
        foreach (var claim in permissionsToBeAssigned)
        {
            var mappedUserClaim = claim.Adapt<ApplicationUserClaim>();
            await userContext.UserClaims.AddAsync(mappedUserClaim, cancellation);
        }

        await userContext.SaveChangesAsync(cancellation);

        return "Permissions were updated successfully.";
    }
    public async Task<ErrorOr<UserViewModel>> GetUserRolesAsync(string? userId, CancellationToken cancellation = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        var userInDb = await _userManager.FindByIdAsync(userId);
        if (userInDb is null)
            return Error.NotFound(description: "No user found.");

        var useViewModel = new UserViewModel
        {
            UserId = userInDb.Id.ToString(),
            UserName = userInDb.UserName!
        };

        var roleViewmodels = new List<UserRolesViewModel>();
        foreach (var role in _roleManager.Roles)
        {
            roleViewmodels.Add(new UserRolesViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsSelected = await _userManager.IsInRoleAsync(userInDb, role.Name!)
            });
        }

        if (roleViewmodels.Any())
            useViewModel.UserRoles.AddRange(roleViewmodels);

        return useViewModel;
    }
    public async Task<ErrorOr<string>> AddOrRemovedUserRolesAsync(List<UserRolesViewModel> model, string userId, CancellationToken cancellation = default)
    {
        var userInDb = await _userManager.FindByIdAsync(userId);
        if (userInDb is null)
            return Error.NotFound(description: "User not found.");

        var roles = await _userManager.GetRolesAsync(userInDb);
        var removedRoleResult = await _userManager.RemoveFromRolesAsync(userInDb, roles);
        if (!removedRoleResult.Succeeded)
        {
            return removedRoleResult.Errors
                .Select(e => Error.Validation(code: e.Code, description: e.Description))
                .ToList();
        }

        var newSelectedRoles = model.Where(x => x.IsSelected).Select(x => x.RoleName).ToList();
        if (!newSelectedRoles.Any())
            return Error.Validation(description: "No roles selected.");

        var applyNewRoleResult = await _userManager.AddToRolesAsync(userInDb, newSelectedRoles!);
        if (!applyNewRoleResult.Succeeded)
        {
            return applyNewRoleResult.Errors
                .Select(e => Error.Validation(code: e.Code, description: e.Description))
                .ToList();
        }

        return "User role was updated.";
    }
    private async Task<IList<UserPermissionViewModel>> GetUserClaimssAsync(string userId)
    {
        await using var userContext = await _userContextFactory.CreateDbContextAsync();
        var userClaims = await userContext.UserClaims
                                     .Where(x => x.UserId == userId)
                                     .AsNoTracking()
                                     .ToListAsync();

        if (userClaims.Any())
        {
            var mappUserPermissions = userClaims.Adapt<IList<UserPermissionViewModel>>();

            return mappUserPermissions;
        }
        return [];
    }
    private async Task<UploadResult> UploadFile(FileModel? fileUpload)
    {

        if (fileUpload is null)
        {
            return new UploadResult(false, "Invalid File Upload", null);
        }
        if (string.IsNullOrEmpty(fileUpload.FileName) && fileUpload?.FileContent?.Length == 0)
        {
            return new UploadResult(false, "No File Name to upload", null);
        }

        if (!AllowedFileSize(fileUpload!.FileSize))
        {
            return new UploadResult(false, "File Size is bigger than allow size.", null);
        }

        if (!AllowedExtension(fileUpload.FileName!))
        {
            return new UploadResult(false, "File Extension is valid.", null);
        }

        var destination = Path.Combine(_env.WebRootPath, "assets/profiles");

        if (!Directory.Exists(destination))
        {
            Directory.CreateDirectory(destination);
        }

        var extension = Path.GetExtension(fileUpload.FileName);

        string newFileName = $"{Guid.CreateVersion7()}{extension}";

        var uploadPath = Path.Combine("assets/profiles", newFileName);

        var filePath = Path.Combine(destination, newFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);

        await fileStream.WriteAsync(fileUpload.FileContent!, 0, fileUpload.FileContent!.Length);

        var uploadResult = new FileModel(fileUpload.FileName!, newFileName, uploadPath);

        return new(true, "Success", uploadResult);
    }


    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".pdf"];
    private bool AllowedExtension(string fileName)
    {

        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return AllowedExtensions.Contains(extension);

    }
    private bool AllowedFileSize(long fileSize = 30_000_000)
    {
        return fileSize > 0 && fileSize < 30_000_000;
    }

    private void DeleteOldProfileFile(string? oldFileName)
    {
        if (string.IsNullOrWhiteSpace(oldFileName)) return;

        var oldFilePath = Path.Combine(_env.WebRootPath, "Profiles", oldFileName);

        if (File.Exists(oldFilePath))
        {
            File.Delete(oldFilePath);
        }
    }




    public async Task<ApplicationUser?> GetUserByName(string name) => await _userManager.FindByNameAsync(name);

    public async Task<ErrorOr<UserDto>> GetUserDetailsById(string? userId, CancellationToken cancellation = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Error.Validation(description: "UserId is required.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Error.NotFound(description: Messages.DataNotFound);

        var userResponse = user.Adapt<UserDto>();
        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Any())
        {
            userResponse.Role = roles.First();
            userResponse.Roles.AddRange(roles);
        }

        await using var userContext = await _userContextFactory.CreateDbContextAsync(cancellation);
        var profile = await userContext.UserProfiles
                                  .Where(x => x.UserRefId == userId && x.IsActive == true)
                                  .FirstOrDefaultAsync(cancellation);

        if (profile is not null)
            userResponse.Profile = profile;

        return userResponse;
    }

    public async Task<ErrorOr<VUser>> GetUserDetailById(string? userId, CancellationToken cancellation = default)
    {
        await using var context = await _cxtFactory.CreateDbContextAsync(cancellation);

        var item = await context.Set<VUser>()
                            .AsNoTracking()
                            .Select(x => new VUser
                            {
                                Id = x.Id,
                                UserName = x.UserName,
                                BranchId = x.BranchId,
                                EmployeeRefId = x.EmployeeRefId,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                BranchName = x.BranchName,
                            })
                            .FirstOrDefaultAsync(x => x.Id == userId, cancellation);

        return item is null
            ? Error.NotFound("NotFound", Messages.DataNotFound)
            : item;
    }






    public async Task<Pager<PermissionsVm>> GetPermissionsAsync(QueryParam q, CancellationToken cancellation = default)
    {
        await using var context = await _cxtFactory.CreateDbContextAsync(cancellation);

        var queries = context.Database
                            .SqlQuery<PermissionsVm>($"SELECT * FROM [Security].[VPermessions]")
                            .WhereIf(!string.IsNullOrEmpty(q.Filter),
                                     x => EF.Functions.Like(x.Module, $"%{q.Filter}%") ||
                                        EF.Functions.Like(x.Action, $"%{q.Filter}%") ||
                                        EF.Functions.Like(x.Group, $"%{q.Filter}%"));
        var counts = await queries.CountAsync();

        if (q.Take > 0)
        {
            queries = queries
                    .Skip(q.Page)
                    .Take(q.Take)
                    .OrderBy(x => x.Module);
        }
        var permissions = await queries
                            .AsNoTracking()
                            .ToListAsync();
        return new Pager<PermissionsVm>(permissions, counts);
    }


    public async Task<bool> IsExistEmployeeHasAccount(string employeeId)
    {
        await using var context = await _cxtFactory.CreateDbContextAsync();

        return await context.Database
                            .SqlQuery<VUser>($@"SELECT * FROM vUsers WHERE EmployeeRefId={employeeId}")
                            .AnyAsync();

    }


    private async Task<bool> IsUserHasLoanRequest(string userId, CancellationToken cancellation = default)
    {
        await using var context = await _cxtFactory.CreateDbContextAsync();

        return await context.LoanRequests
                            .Where(lr => lr.RequestUserRefId == userId || lr.AprovedUserId == userId)
                            .Select(_ => 1)
                            .AnyAsync(cancellation);

    }


    private async Task<bool> IsUserHasLoanDisbursment(string userId, CancellationToken cancellation = default)
    {
        await using var context = await _cxtFactory.CreateDbContextAsync(cancellation);

        return await context.LoanDisbursments
                            .Where(ld => ld.UserRefId == userId)
                            .Select(_ => 1)
                            .AnyAsync(cancellation);
    }

    private async Task<bool> IsUserHasProfile(string userId, CancellationToken cancellation = default)
    {
        await using var context = await _userContextFactory.CreateDbContextAsync(cancellation);

        return await context.UserProfiles
                            .Where(up => up.UserRefId == userId)
                            .Select(_ => 1)
                            .AnyAsync(cancellation);

    }


}

//public async Task<ErrorOr<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default, params IEnumerable<string> columns)
//{
//    if (userId.IsNullOrEmpty())
//        return Error.NotFound(description: "There is no user selected.");

//    var strColumns = columns.Any() ? string.Join(",", columns) : "*";

//    using var db = await connectionFactory.CreateConnectionAsync(cancellation);

//    var parameters = new DynamicParameters();
//    parameters.Add("@UserId", userId);

//    var item = await db.QueryFirstOrDefaultAsync<VUser>($"SELECT {strColumns} FROM vUsers WHERE Id=@UserId", parameters);

//    return item is null
//        ? Error.NotFound(description: Messages.DataNotFound)
//        : item;
//}

//public async Task<ErrorOr<List<VUser>>> GetUsersAsync(string? userId, string? filter = null, CancellationToken cancellation = default)
//{
//    await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

//    var queries = await context.Database
//                            .SqlQuery<VUser>($"SELECT * FROM vUsers")
//                            .Where(x => x.IsActive == true)
//                            .WhereIf(!string.IsNullOrWhiteSpace(filter),
//                                                x => EF.Functions.Like(x.UserName!, $"%{filter}%") ||
//                                                     EF.Functions.Like(x.Email!, $"%{filter}%"))
//                            .WhereIf(!string.IsNullOrEmpty(userId), u => u.Id == userId)
//                            .AsNoTracking()
//                            .ToListAsync(cancellation);

//    return queries;
//}


//public async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
//{
//    var userClaims = await _userManager.GetClaimsAsync(user);

//    var roles = await _userManager.GetRolesAsync(user);

//    var roleClaims = new List<Claim>();
//    var permissionClaims = new List<Claim>();

//    //if (user.BranchRefId > 0 || !roles.Contains(SystemRole.SuperAdmin))
//    //{
//    //    var branchResponse = await _branchService.GetBrachStatus(user.BranchRefId!.Value);
//    //    var branchState = branchResponse.IsSuccess ? branchResponse.Data : new();
//    //    var status = branchState!.IsOpen ? "1" : "0";
//    //    roleClaims.Add(new("Status", status));
//    //    roleClaims.Add(new("BranchId", $"{user.BranchRefId}"));
//    //}

//    foreach (var role in roles)
//    {
//        roleClaims.Add(new Claim(ClaimTypes.Role, role));
//        var currentRole = await _roleManager.FindByNameAsync(role);
//        var allPermissionsForCurrentRole = await _roleManager.GetClaimsAsync(currentRole!);
//        permissionClaims.AddRange(allPermissionsForCurrentRole);
//    }
//    var claims = new List<Claim>
//        {
//            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
//            new(ClaimTypes.Email, user.Email!),
//            new(ClaimTypes.Name, user.UserName!),
//            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
//            new Claim(JwtRegisteredClaimNames.Sub, user.Email!)
//        }
//    .Union(userClaims)
//    .Union(roleClaims)
//    .Union(permissionClaims);
//    return claims;
//}
//public async Task<ApplicationUser?> GetUserById(string userId) => await _userManager.FindByIdAsync(userId);
//public async Task<ApplicationUser?> GetUserByEmail(string email) => await _userManager.FindByEmailAsync(email);

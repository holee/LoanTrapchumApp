using Applications.Constants.Authorizations;
using Applications.Extensions;
using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Dapper;
using Domains.Models.Files;
using Domains.Models.Identity;
using Domains.Shared;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
namespace Infrastructures.Implements;

public sealed class UserService : IUserService
{
    private readonly UserDataContext _userContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signinManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IHostingEnvironment _env;
    private readonly IDbContextFactory<DataContext> _dbContextFactory;
    private readonly IDapperConnectionFactory connectionFactory;
    public UserService(UserManager<ApplicationUser> userManager,
         RoleManager<ApplicationRole> roleManager,
         UserDataContext userContext,
         IHostingEnvironment env,
         SignInManager<ApplicationUser> signinManager,
         IDbContextFactory<DataContext> dbContextFactory,
         IDapperConnectionFactory connectionFactory)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userContext = userContext;
        _env = env;
        _signinManager = signinManager;
        _dbContextFactory = dbContextFactory;
        this.connectionFactory = connectionFactory;
    }
    public async Task<Result<string>> CreateUserAsync(RegisterRequest model)
    {

        if (string.IsNullOrWhiteSpace(model.EmployeeId))
        {

            return Result.Failure<string>(ErrorResponse.NotFound("Employee is required."));
        }

        if (await IsExistEmployeeHasAccount(model.EmployeeId))
        {
            return Result.Failure<string>(ErrorResponse.Failure($"Employee already register."));
        }
        var existingUser = await GetUserByName(model.Username);

        if (existingUser is not null)
        {
            return Result.Failure<string>(ErrorResponse.Failure($"{model.Username} Already Exist."));
        }
        var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

        if (userWithSameEmail is not null)
        {
            return Result.Failure<string>(ErrorResponse.Failure($"{model.Email} Email Already Exist."));
        }

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

            var errors = result.Errors.Select(x => x.Description).ToArray();

            var errorDict = new Dictionary<string, string[]>
                {
                    { "errors",errors }
                };

            return Result.Failure<string>(ErrorResponse.Validation(errorDict));
        }

        if (!await _userManager.IsInRoleAsync(user, model.RoleName!))
        {
            var assignRole = await _userManager.AddToRoleAsync(user, model.RoleName!);

            if (!assignRole.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToArray();

                var errorDict = new Dictionary<string, string[]>
                {
                    { "errors",errors }
                };

                return Result.Failure<string>(ErrorResponse.Validation(errorDict));
            }
        }

        return Result.Success("New User was added.");

    }

    public async Task<Result<IEnumerable<VUser>>> GetUsersAsync(string? userId, string? filter = null)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var queries = await context.Database
                                .SqlQuery<VUser>($"SELECT * FROM vUsers")
                                .Where(x => x.IsActive == true)
                                .WhereIf(!string.IsNullOrWhiteSpace(filter),
                                                    x => EF.Functions.Like(x.UserName!, $"%{filter}%") ||
                                                         EF.Functions.Like(x.Email!, $"%{filter}%"))
                                .WhereIf(!string.IsNullOrEmpty(userId), u => u.Id == userId)
                                .AsNoTracking()
                                .ToListAsync();
        return Result.Success<IEnumerable<VUser>>(queries);

    }

    public async Task<Result<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default)
    {

        if (userId.IsNullOrEmpty())
        {
            return Result.Failure<VUser>(ErrorResponse.NotFound("There is no user selected"));
        }

        using var db = await connectionFactory.CreateConnectionAsync(cancellation);

        var parameters = new DynamicParameters();

        parameters.Add("@UserId", userId);

        var sql = "SELECT * FROM vUsers WHERE Id=@UserId";

        var item = await db.QueryFirstOrDefaultAsync<VUser>(sql, parameters);

        if (item is null)
        {
            return Result.Failure<VUser>(ErrorResponse.NotFound(Messages.DataNotFound));
        }

        return Result.Success<VUser>(item);

    }

    public async Task<Result<VUser>> GetUserByIdAsync(string? userId, CancellationToken cancellation = default, params IEnumerable<string> columns)
    {

        if (userId.IsNullOrEmpty())
        {
            return Result.Failure<VUser>(ErrorResponse.NotFound("There is no user selected"));
        }

        var strColumns = columns.Any() ? string.Join(",", columns) : "*";

        using var db = await connectionFactory.CreateConnectionAsync(cancellation);

        var parameters = new DynamicParameters();

        parameters.Add("@UserId", userId);

        var sql = $"SELECT {strColumns} FROM vUsers WHERE Id=@UserId";

        var item = await db.QueryFirstOrDefaultAsync<VUser>(sql, parameters);

        if (item is null)
        {
            return Result.Failure<VUser>(ErrorResponse.NotFound(Messages.DataNotFound));
        }

        return Result.Success<VUser>(item);

    }

    public async Task<Result<Pager<VUser>>> GetUsersAsync(QueryParam param, string? userId)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var queries = context.Database
                            .SqlQuery<VUser>($"SELECT * FROM vUsers")
                            .WhereIf(!param.IncludeInActive, x => x.IsActive == true)
                            .WhereIf(!string.IsNullOrWhiteSpace(param.Filter), e => EF.Functions.Like(e.UserName!, $"%{param.Filter}%") ||
                                                                                    EF.Functions.Like(e.Email!, $"%{param.Filter}%"))
                            .WhereIf(userId.IsNotNullOrEmpty(), u => u.Id == userId)
                            .OrderByDescendingIf(param.SortDescending, p => EF.Property<object>(p, param.SortColumn));


        var counts = await queries.CountAsync();

        if (param.PageSize > 0)
        {
            queries = queries.Skip(param.Page).Take(param.PageSize);
        }


        var users = await queries
                            .AsNoTracking()
                            .ToListAsync();

        return Result.Success<Pager<VUser>>(new(users, counts));
    }

    public async Task<Result<Pager<VUser>>> GetUsersAsync(int? branchId, string? userId, QueryParam param)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var queries = context.Database
                            .SqlQuery<VUser>($"SELECT * FROM vUsers")
                            .WhereIf(branchId > 0, x => x.BranchId == branchId)
                            .WhereIf(!param.IncludeInActive, x => x.IsActive == true)
                            .WhereIf(!string.IsNullOrWhiteSpace(param.Filter), e => EF.Functions.Like(e.UserName!, $"%{param.Filter}%") ||
                                                                                    EF.Functions.Like(e.Email!, $"%{param.Filter}%"))
                            .WhereIf(userId.IsNotNullOrEmpty(), u => u.Id == userId)
                            .OrderByDescendingIf(param.SortDescending, p => EF.Property<object>(p, param.SortColumn));


        var counts = await queries.CountAsync();


        if (param.PageSize > 0)
        {
            queries = queries.Skip(param.Page).Take(param.PageSize);
        }

        var users = await queries.AsNoTracking().ToListAsync();

        return Result.Success<Pager<VUser>>(new(users, counts));
    }


    public async Task<Result<string>> ChangePassword(ChangePasswordDto req, CancellationToken cancellation = default)
    {

        var existingUser = await _userManager.FindByIdAsync(req.UserId);

        if (existingUser is null)
        {
            return Result.Failure<string>(ErrorResponse.Failure("There is no any user in the system."));
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(existingUser, req.CurrentPassword, req.NewPassword);

        if (!changePasswordResult.Succeeded)
        {
            var errors = changePasswordResult.Errors.Select(x => x.Description);

            var errString = string.Join(Environment.NewLine, errors);

            return Result.Failure<string>(ErrorResponse.Failure(errString));
        }
        return Result.Success<string>("Password was changed successful.");
    }


    public async Task<Result> UpdateUserRoleAsync(UserRoleDtoUpdate req)
    {

        var existingUser = await _userManager.FindByIdAsync(req.UserId);

        if (existingUser is null)
        {
            return Result.Failure(ErrorResponse.NotFound("There is no any user in the system."));
        }
        var existingRoles = await _userManager.GetRolesAsync(existingUser);

        if (existingRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(existingUser, existingRoles);
        }

        var result = await _userManager.AddToRoleAsync(existingUser, req.RoleName);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();

            var errString = string.Join(Environment.NewLine, errors);

            return Result.Failure(ErrorResponse.Failure(errString));

        }

        return Result.Success("Role was changed.");

    }
    public async Task<Result<FileModel>> ChangeProfile(string? userId, FileModel? model, CancellationToken cancellation = default)
    {

        if (userId.IsNullOrEmpty())
        {
            return Result.Failure<FileModel>(ErrorResponse.NotFound("There is no any user selected."));
        }

        if (model is null)
        {
            return Result.Failure<FileModel>(ErrorResponse.Validation("No file was provided."));
        }

        var existingProfile = await _userContext.UserProfiles
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.UserRefId == userId);


        var uploadResult = await UploadFile(model);

        if (!uploadResult.Success || uploadResult.Model is null)
        {
            return Result.Failure<FileModel>(ErrorResponse.Validation("Error file upload."));
        }

        var fileResult = uploadResult.Model;

        var userProfile = new UserProfiles
        {
            FileName = fileResult.FileName,
            FileUrl = fileResult.FileUrl,
            IsActive = true,
            UserRefId = userId
        };

        if (existingProfile is null)
        {
            _userContext.UserProfiles.Add(userProfile);
        }
        else
        {
            _userContext.UserProfiles.Update(userProfile);
        }


        DeleteOldProfileFile(existingProfile?.FileName);

        await _userContext.SaveChangesAsync();

        return Result.Success<FileModel>(fileResult);

    }

    public async Task<Result<string>> ChangeProfile(UserProfiles profile, CancellationToken cancellation = default)
    {


        if (string.IsNullOrEmpty(profile.UserRefId))
        {
            _userContext.UserProfiles.Add(profile);
        }
        else
        {
            _userContext.UserProfiles.Update(profile);
        }


        await _userContext.SaveChangesAsync();

        return Result.Success<string>("Profile was changed.");

    }


    public async Task<Result<UserProfileResponse>> GetUserProfile(string? userId, CancellationToken cancellation = default)
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

        var item = await db.QueryFirstAsync<UserProfileResponse>(sql, new
        {
            UserId = userId,
        });

        if (item is null)
        {
            return Result.Failure<UserProfileResponse>(ErrorResponse.NotFound("NO User found."));
        }

        return Result.Success(item);

    }


    public async Task<Result> EditUserAsync(string userId, EditUserDto model)
    {

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result.Failure(ErrorResponse.NotFound("User Not Found"));
        }
        else
        {
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.IsActive = model.IsActive;
            var editResult = await _userManager.UpdateAsync(user);
            if (!editResult.Succeeded)
            {
                var errors = editResult.Errors.Select(x => x.Description).ToList();
                var errString = string.Join(Environment.NewLine, errors);
                return Result.Failure(ErrorResponse.Failure(errString));
            }

        }
        return Result.Success("User Was Edited Success fully.");
    }
    public async Task<Result<UserPermissions>> GetUserPermissionsAsync(string? userId)
    {

        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        var userInDb = await _userManager.FindByIdAsync(userId);

        if (userInDb is null)
        {
            return Result.Failure<UserPermissions>(ErrorResponse.NotFound("There is any users in the system."));
        }

        var allPermissions = ApplicationPermissions.UserPermissions;//  UserLevelPermissions.AllUserPermissions;
        var userPermissions = new UserPermissions(userId, []);
        var currentUserClaims = await GetUserClaimssAsync(userId);
        var userPermissionsNames = allPermissions.Select(p => p.For).ToList();
        var currentUserClaimsValues = currentUserClaims.Select(crc => crc.ClaimValue).ToList();
        var currentlyAssignedUserClaimsNames = userPermissionsNames
                                                    .Intersect(currentUserClaimsValues)
                                                    .ToList();
        foreach (var permission in allPermissions)
        {
            if (currentlyAssignedUserClaimsNames.Any(carc => carc == permission.For))
            {
                userPermissions.Permissions!.Add(new UserPermissionViewModel
                {
                    UserId = userId,
                    ClaimType = AppClaim.Permission,
                    ClaimValue = permission.For,
                    Group = permission.Group,
                    IsAssignedToRole = true
                });
            }
            else
            {
                userPermissions.Permissions!.Add(new UserPermissionViewModel
                {
                    UserId = userId,
                    ClaimType = AppClaim.Permission,
                    ClaimValue = permission.For,
                    Group = permission.Group,
                    IsAssignedToRole = false
                });
            }
        }
        return await Result.SuccessAsync(userPermissions);
    }
    public async Task<Result<string>> AddOrRemovedUserPermissionsAsync(string? userId, UserPermissionRequest permission)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        var userInDb = await _userManager.FindByIdAsync(userId);

        if (userInDb is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound("No Role Found."));
        }

        var permissionsToBeAssigned = permission!.Permissions!
                                                 .Where(rc => rc.IsAssignedToRole == true)
                                                    .ToList();

        var currentlyAssignedClaims = await _userManager.GetClaimsAsync(userInDb);

        // Drop
        foreach (var claim in currentlyAssignedClaims)
        {
            await _userManager.RemoveClaimAsync(userInDb, claim);
        }
        // Add
        foreach (var claim in permissionsToBeAssigned)
        {
            var mappedUserClaim = claim.Adapt<ApplicationUserClaim>();

            await _userContext.UserClaims.AddAsync(mappedUserClaim);
        }

        await _userContext.SaveChangesAsync();

        return Result.Success("Permissions were addedd successfully.");
    }
    public async Task<Result<UserViewModel>> GetUserRolesAsync(string? userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        var userInDb = await _userManager.FindByIdAsync(userId);

        if (userInDb is null)
        {
            return Result.Failure<UserViewModel>(ErrorResponse.NotFound("No Role Found."));
        }
        var roleViewmodels = new List<UserRolesViewModel>();
        var useViewModel = new UserViewModel
        {
            UserId = userInDb.Id.ToString(),
            UserName = userInDb.UserName!
        };
        foreach (var role in _roleManager.Roles)
        {
            var roleViewmodel = new UserRolesViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
            };

            if (await _userManager.IsInRoleAsync(userInDb, role.Name!))
            {
                roleViewmodel.IsSelected = true;
            }
            else
            {
                roleViewmodel.IsSelected = false;
            }
            roleViewmodels.Add(roleViewmodel);
        }
        if (roleViewmodels.Any())
        {
            useViewModel.UserRoles.AddRange(roleViewmodels);
        }
        return Result.Success(useViewModel);
    }
    public async Task<Result<string>> AddOrRemovedUserRolesAsync(List<UserRolesViewModel> model, string userId)
    {
        var userInDb = await _userManager.FindByIdAsync(userId);

        if (userInDb is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound("user not found."));
        }

        var roles = await _userManager.GetRolesAsync(userInDb);

        var removedRoleResult = await _userManager.RemoveFromRolesAsync(userInDb, roles);

        if (!removedRoleResult.Succeeded)
        {
            var error = removedRoleResult.Errors.Select(x => x.Description);

            var errString = string.Join(Environment.NewLine, error);

            return Result.Failure<string>(ErrorResponse.ServerError(errString));
        }

        var newSelectedRoles = model.Where(x => x.IsSelected == true)
                                    .Select(x => x.RoleName).ToList();

        if (!newSelectedRoles.Any())
        {
            return Result.Failure<string>(ErrorResponse.NotFound("no any roles selected."));
        }

        var applyNewRoleResult = await _userManager.AddToRolesAsync(userInDb, newSelectedRoles!);

        if (!applyNewRoleResult.Succeeded)
        {
            var error = removedRoleResult.Errors.Select(x => x.Description);

            var errString = string.Join(Environment.NewLine, error);

            return Result.Failure<string>(ErrorResponse.ServerError(errString));
        }

        return Result.Success("user role was updated.");
    }
    private async Task<IList<UserPermissionViewModel>> GetUserClaimssAsync(string userId)
    {
        var userClaims = await _userContext.UserClaims
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

        var destination = Path.Combine(_env.WebRootPath, "Profiles");

        if (!Directory.Exists(destination))
        {
            Directory.CreateDirectory(destination);
        }

        var extension = Path.GetExtension(fileUpload.FileName);

        string newFileName = $"{Guid.CreateVersion7()}{extension}";

        var uploadPath = Path.Combine("Profiles", newFileName);

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

    public async Task<Result<UserDto>> GetUserDetailsById(string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentNullException(nameof(userId));
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return Result.Failure<UserDto>(ErrorResponse.NotFound(Messages.DataNotFound));
        }

        var userResponse = user.Adapt<UserDto>();

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Any())
        {
            userResponse.Role = roles.First();

            userResponse.Roles.AddRange(roles);
        }

        var profile = await _userContext.UserProfiles
                                  .Where(x => x.UserRefId == userId && x.IsActive == true)
                                  .FirstOrDefaultAsync();
        if (profile is not null)
        {
            userResponse.Profile = profile;
        }


        return Result.Success(userResponse);
    }

    public async Task<ErrorOr.ErrorOr<VUser>> GetUserDetailById(string? userId, CancellationToken cancellation = default)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

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

        if (item is null) return ErrorOr.Error.NotFound("NotFound", Messages.DataNotFound);

        return item;
    }






    public async Task<Pager<PermissionsVm>> GetPermissionsAsync(QueryParam q)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var queries = context.Database
                            .SqlQuery<PermissionsVm>($"SELECT * FROM [Security].[VPermessions]")
                            .WhereIf(!string.IsNullOrEmpty(q.Filter),
                                     x => EF.Functions.Like(x.Module, $"%{q.Filter}%") ||
                                        EF.Functions.Like(x.Action, $"%{q.Filter}%") ||
                                        EF.Functions.Like(x.Group, $"%{q.Filter}%"));
        var counts = await queries.CountAsync();

        if (q.PageSize > 0)
        {
            queries = queries
                    .Skip(q.Page)
                    .Take(q.PageSize)
                    .OrderBy(x => x.Module);
        }
        var permissions = await queries
                            .AsNoTracking()
                            .ToListAsync();
        return new Pager<PermissionsVm>(permissions, counts);
    }


    public async Task<bool> IsExistEmployeeHasAccount(string employeeId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.Database
                            .SqlQuery<VUser>($@"SELECT * FROM vUsers WHERE EmployeeRefId={employeeId}")
                            .AnyAsync();

    }


}


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

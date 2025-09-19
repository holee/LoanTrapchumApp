using Applications.Extensions;
using Applications.Features.Entities;
using Applications.Interfaces.Branches;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Branches;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public class BranchService(IDbContextFactory<DataContext> contextFactory) : IBranchService
{

    public async Task<Result<VBranchResponse>> CreateAsync(Branch branch, CancellationToken cancellation = default)
    {
        try
        {
            if (await IsBranchExist(null, branch.BranchName)) 
            {
                return Result.Failure<VBranchResponse>(ErrorResponse.Conflict("Branch Name is already exist."));
            }

            await using var context = await contextFactory.CreateDbContextAsync();

            context.Branches.Add(branch);

            await context.SaveChangesAsync();

           var created=await context.VBranchs
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.BranchId == branch.BranchId);

            if(created is null)
            {
                return Result.Failure<VBranchResponse>(ErrorResponse.Validation("Could not created branch...!"));
            }

            return Result.Success(created);
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure<VBranchResponse>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }
     
    public async Task<Result> DeleteAsync(int? id)
    {
        try
        {

            if(await IsBranchUsed(id))
            {
                return Result.Failure<string>(ErrorResponse.Conflict("This branch is used in the system."));
            }

            await using var context = await contextFactory.CreateDbContextAsync();

            var branch = await context.Branches.FindAsync(id);

            if (branch is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no branch found."));
            }

            context.Branches.Remove(branch);

            await context.SaveChangesAsync();

            return Result.Success("One branch was delete");
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }

    public async Task<Result> UpdateAsync(Branch branch)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var existingBranch = await context.Branches
                .FirstOrDefaultAsync(x => x.BranchId == branch.BranchId &&
                                          x.EditSeq == branch.EditSeq);

            if (existingBranch is null)
            {
                return Result.Failure<string>(
                    ErrorResponse.Failure("One branch was updated Or Branch was chnaged by someone"));
            }

            branch.EditSeq = existingBranch.EditSeq + 1;

            context.Entry(existingBranch).CurrentValues.SetValues(branch);

            await context.SaveChangesAsync();

            return Result.Success("One branch was updated");
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }

    public async Task<Result> UpdateStatusAsync(int? branchId)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var existingBranch = await context.Branches.FindAsync(branchId);

            if (existingBranch is null)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"No any branch found."));
            }

            existingBranch.IsActive = !existingBranch.IsActive;

            existingBranch.EditSeq = existingBranch.EditSeq + 1;

            context.Branches.Update(existingBranch);

            await context.SaveChangesAsync();

            return Result.Success("Branch was updated successfully!");
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }

    public async Task<Result<string>> UpdateStatusAsync(int? branchId, bool state)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var existingBranch = await context.Branches.FindAsync(branchId);

            if (existingBranch is null)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(Messages.DataNotFound));
            }

            existingBranch.IsActive = state;

            existingBranch.ModifiedDt = DateTime.Now;

            await context.SaveChangesAsync();

            return Result.Success("Branch was updated successfully!");
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }


    public async Task<Result<Branch>> GetBranchById(int? id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var branch = await context.Branches.FindAsync(id);

        if (branch is null)
        {
            return Result.Failure<Branch>(ErrorResponse.NotFound("There is no any branch found."));
        }

        return Result.Success(branch);
    }

    public async Task<Result<T>> GetBranchById<T>(int? id)
        where T : class
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var branch = await context.Database
            .SqlQuery<VBranchResponse>($"SELECT TOP(1) * FROM VBranches WHERE BranchId={id}")
            .AsNoTracking()
            .ProjectToType<T>()
            .FirstOrDefaultAsync();
        if (branch is null)
        {
            return Result.Failure<T>(ErrorResponse.NotFound("There is no any branch found."));
        }

        return Result.Success(branch);
    }

    public async Task<Pager<VBranchResponse>> GetBranchesAsync(QueryParam param)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var queries = context.VBranchs
            .WhereIf(!string.IsNullOrEmpty(param.Filter),
                x => EF.Functions.Like(x.BranchName, $"%{param.Filter}%") ||
                     EF.Functions.Like(x.BranchId.ToString(), $"%{param.Filter}%"));

        if (param.IsIncludeInActive)
        {
            queries = queries.IgnoreQueryFilters();
        }

        if (param.SortDescending)
        {
            queries = queries.OrderByDescending(f => EF.Property<object>(f, param.SortColumn));
        }

        int counts = queries.Count();

        if (param.PageSize > 0)
        {
            queries = queries.Skip(param.Skip).Take(param.PageSize);
        }

        var items = await queries.AsNoTracking().ToListAsync();

        return new Pager<VBranchResponse>
        {
            Count = counts,
            Items = items
        };
    }

    public async Task<Result<Pager<T>>> GetBranchesAsync<T>(int? branchId, QueryParam param)
        where T : class
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        IQueryable<VBranchResponse> queries = context.VBranchs;

        if (branchId > 0)
        {
            queries = queries.Where(x => x.BranchId == branchId);
        }

        if (param.IsIncludeInActive) queries = queries.IgnoreQueryFilters();

        if (!string.IsNullOrEmpty(param.Filter))
        {
            queries = queries.Where(x => EF.Functions.Like(x.BranchName, $"%{param.Filter}%"));
        }

        if (param.SortDescending)
        {
            queries = queries.OrderByDescending(f => EF.Property<object>(f, param.SortColumn));
        }

        int counts = await queries.CountAsync();

        if (param.PageSize > 0)
        {
            queries = queries.Skip(param.Skip).Take(param.PageSize);
        }

        var items = await queries
            .ProjectToType<T>()
            .AsNoTracking()
            .ToListAsync();
        return Result.Success(new Pager<T>
        {
            Count = counts,
            Items = items
        });
    }

    public async Task<Result<IEnumerable<VBranchResponse>>> GetBranchesByUser(string? userId, int? branchId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure<IEnumerable<VBranchResponse>>(ErrorResponse.NotFound("No User found in the branch."));
        }

        await using var context = await contextFactory.CreateDbContextAsync();


        var user = await context.Database.SqlQuery<VUser>($"SELECT * FROM VUsers WHERE id={userId}")
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return Result.Failure<IEnumerable<VBranchResponse>>(ErrorResponse.NotFound("No User found in the branch."));
        }


        if (user.EmployeeRefId.IsNullOrEmpty())
        {
            return Result.Failure<IEnumerable<VBranchResponse>>(ErrorResponse.NotFound("No User found in the branch."));
        }

        var queries = context.VBranchs.Where(x => x.BranchId == user.BranchId);

        if (branchId > 0)
        {
            queries = queries.Where(x => x.BranchId == branchId);
        }

        return Result.Success<IEnumerable<VBranchResponse>>(await queries.AsNoTracking().ToListAsync());
    }

    public async Task<Result<IEnumerable<T>>> GetBranchesByUser<T>(string? userId, int? branchId = 0)
        where T : class
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("No User found in the branch."));
        }

        await using var context = await contextFactory.CreateDbContextAsync();

        var user = await context.Database.SqlQuery<VUser>($"SELECT * FROM VUsers WHERE id={userId}")
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("No User found in the branch."));
        }


        if (user.EmployeeRefId.IsNullOrEmpty())
        {
            return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("No User found in the branch."));
        }

        var queries = context.VBranchs.Where(x => x.BranchId == user.BranchId);

        if (branchId > 0)
        {
            queries = queries.Where(x => x.BranchId == branchId);
        }

        var items = await queries
            .AsNoTracking()
            .ProjectToType<T>()
            .ToListAsync();

        return Result.Success<IEnumerable<T>>(items);
    }

    public async Task<Result<ResultStatus>> GetBrachStatus(int? branchId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var branch = await context.VBranchs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.BranchId == branchId && x.IsActive == true);

        if (branch is null)
        {
            return Result.Failure<ResultStatus>(ErrorResponse.NotFound("There is no any branch found."));
        }

        return branch.OpenStatus == "1"
            ? Result.Success(new ResultStatus(true, "Open"))
            : Result.Success(new ResultStatus(false, "Closed"));
    }


    public async Task<Result<Branch>> GetBranchForVault(int? id)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var branch = await context.Branches.IgnoreAutoIncludes()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BranchId == id);
            if (branch is null)
            {
                return Result.Failure<Branch>(ErrorResponse.NotFound("There is no any branch found."));
            }

            return Result.Success(branch);
        }
        catch (Exception e)
        {
            {
                return Result.Failure<Branch>(ErrorResponse.ServerError($"{e.Message}"));
            }
        }
    }

    public async Task<Result<BranchDtoUpdate>> GetUpdateBranchById(int? branchId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var existingBranch = await context.Branches.FindAsync(branchId);

        if (existingBranch is null)
        {
            return Result.Failure<BranchDtoUpdate>(ErrorResponse.NotFound("There is no any branch found."));
        }

        var item = existingBranch.Adapt<BranchDtoUpdate>();

        return Result.Success(item);
    }

    public async Task<Result<UserBranchesResponse>> GetBranchesByUser(string? userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure<UserBranchesResponse>(ErrorResponse.NotFound("There is no user is selected"));
        }

        await using var context = await contextFactory.CreateDbContextAsync();

        var user = await context.VUsers
                                .Where(x=>x.Id==userId)
                                .ProjectToType<UserBranchesResponse>()
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

        if (user is null)
        {
            return Result.Failure<UserBranchesResponse>(ErrorResponse.NotFound("There is no user in the system"));
        }

        return Result.Success(user);
    }

    public async Task<Result<IEnumerable<ApprovedUser>>> GetApprovedUser(int? branchId, string? filter)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var user = await context.VUsers
            .Where(x=>x.BranchId==branchId && x.CanApproved==true)
            .WhereIf(!string.IsNullOrEmpty(filter), x => EF.Functions.Like(x.UserName!, $"%{filter}%") ||
                                                         EF.Functions.Like(x.FirstName!, $"%{filter}%") ||
                                                         EF.Functions.Like(x.LastName!, $"%{filter}%"))
            .ProjectToType<ApprovedUser>()
            .AsNoTracking()
            .ToListAsync();

        if (!user.Any())
        {
            return Result.Failure<IEnumerable<ApprovedUser>>(ErrorResponse.NotFound("There is no user in the system"));
        }

        return Result.Success<IEnumerable<ApprovedUser>>(user);
    }



    private async Task<bool> IsBranchExist(int? branchId,string branchName,CancellationToken cancellation=default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var existings=await context.VBranchs
                                    .Where(x=>x.BranchName == branchName && 
                                            (!branchId.HasValue || x.BranchId !=branchId))
                                    .AsNoTracking()
                                    .ToListAsync(cancellation);

        return existings.Any();
    }

    private async Task<bool> IsBranchUsed(int? branchId, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var existings = await context.VBranchs
                                    .Where(x =>x.BranchId==branchId && x.Used==true)
                                    .AsNoTracking()
                                    .ToListAsync(cancellation);

        return existings.Any();
    }

}
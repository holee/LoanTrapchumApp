using Applications.Constants.Tables;
using Applications.Extensions;
using Applications.Interfaces.Branches;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Branches;
using Dapper;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public class BranchService(ApplicationDbContext context, IDapperConnectionFactory dapperConnection) : IBranchService
{
    public async Task<Result> Create(Branch branch)
    {
        try
        {
            context.Branches.Add(branch);

            await context.SaveChangesAsync();

            return Result.Success("New branch was created.");
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }

    public async Task<Result> DeleteAsync(int? id)
    {
        try
        {
            var branch = await context.Branches.AsNoTracking().Where(x => x.BranchId == id).FirstOrDefaultAsync();

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

    public async Task<Result> Update(Branch branch)
    {
        try
        {
            var existingBranch = context.Branches.FirstOrDefault(x => x.BranchId == branch.BranchId);


            if (existingBranch is null)
            {
                return Result.Failure<string>(ErrorResponse.Failure("One branch was updated"));
            }

            if (existingBranch.EditSeq != branch.EditSeq)
            {
                return Result.Failure<string>(ErrorResponse.Failure("Branch was chnaged by someone"));
            }


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
            var existingBranch = await context.Branches.AsNoTracking().FirstOrDefaultAsync(x => x.BranchId == branchId);

            if (existingBranch is null)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"No any branch found."));
            }

            existingBranch!.IsActive = existingBranch.IsActive ? false : true;

            context.Branches.Update(existingBranch);

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
        var branch = await context.Branches
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.BranchId == id);
        if (branch is null)
        {
            return Result.Failure<Branch>(ErrorResponse.NotFound("There is no any branch found."));
        }

        return Result.Success(branch);
    }

    public async Task<Result<T>> GetBranchById<T>(int? id)
    {
        var branch = await context.Branches
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.BranchId == id);
        if (branch is null)
        {
            return Result.Failure<T>(ErrorResponse.NotFound("There is no any branch found."));
        }

        var item = branch.Adapt<T>();
        
        return Result.Success(item);
    }
    
    
    
    public async Task<Result<IEnumerable<VBranchResponse>>> GetBranchesAsync(int branchId = 0,
        int pageSize = 0,
        bool includeInactive = false,
        string? filter = null)
    {
        IQueryable<VBranchResponse> query = context.VBranchs.OrderByDescending(x => x.BranchName);

        if (includeInactive)
        {
            query = query.IgnoreQueryFilters();
        }

        if (branchId > 0)
        {
            query = query.Where(x => x.BranchId == branchId);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(x => EF.Functions.Like(x.BranchName, $"%{filter}%"));
        }

        if (pageSize > 0)
        {
            query = query.Take(pageSize).OrderByDescending(x => x.BranchName);
        }


        return Result.Success<IEnumerable<VBranchResponse>>(await query.AsNoTracking().ToListAsync());
    }

    public async Task<Pager<VBranchResponse>> GetBranchesAsync(QueryParam param)
    {
        IQueryable<VBranchResponse> query = context.VBranchs;

        if (param.IsIncludeInActive == true)
            query = query.IgnoreQueryFilters();

        if (!string.IsNullOrEmpty(param.Filter))
        {
            query = query.Where(x => EF.Functions.Like(x.BranchName, $"%{param.Filter}%"));
        }

        if (param.SortDescending)
        {
            query = query.OrderByDescending(f => EF.Property<object>(f, param.SortColumn));
        }

        int counts = await query.CountAsync();

        var items = query.Skip(param.PageIndex).Take(param.PageSize);

        return new Pager<VBranchResponse>
        {
            Count = counts,
            Items = await items.AsNoTracking().ToListAsync()
        };
    }

    public async Task<Result<IEnumerable<VBranchResponse>>> GetBranchesAsync(int branchId = 0, int take = 0,
        string? filter = null)
    {
        IQueryable<VBranchResponse> queries = context.VBranchs.OrderBy(x => x.BranchName);

        if (!queries.Any())
        {
            return Result.Failure<IEnumerable<VBranchResponse>>(ErrorResponse.NotFound("no data found."));
        }

        if (!string.IsNullOrEmpty(filter))
        {
            queries = queries.Where(x => EF.Functions.Like(x.BranchName, $"%{filter}%"));
        }

        if (branchId > 0)
        {
            queries = queries.Where(x => x.BranchId == branchId);
        }

        if (take > 0)
        {
            queries = queries.Take(take).OrderBy(x => x.BranchName);
        }

        var items = await queries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<VBranchResponse>>(items);
    }

    public async Task<Result<IEnumerable<VBranchResponse>>> GetBranchesByUser(string userId, int? branchId = 0)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure<IEnumerable<VBranchResponse>>(ErrorResponse.NotFound("No User found in the branch."));
        }

        using var db = dapperConnection.CreateConnection();

        var user = await db.QueryFirstAsync<VUser>($"SELECT * FROM {TableConstant.VUsers} WHERE id=@userId", new
        {
            UserId = userId
        });

        IQueryable<VBranchResponse> queries = context.VBranchs;

        if (user.EmployeeRefId.IsNotNullOrEmpty())
        {
            queries = queries.Where(x => x.BranchId == user.BranchId);
        }

        if (branchId > 0)
        {
            queries = queries.Where(x => x.BranchId == branchId);
        }


        return Result.Success<IEnumerable<VBranchResponse>>(await queries.AsNoTracking().ToListAsync());
    }

    public async Task<Result<ResultStatus>> GetBrachStatus(int? branchRefId)
    {
        var branch = await context.VBranchs.Where(x => x.BranchId == branchRefId).FirstOrDefaultAsync();
        if (branch is null)
        {
            return Result.Failure<ResultStatus>(ErrorResponse.NotFound("There is no any branch found."));
        }

        return branch!.OpenStatus == "1"
            ? Result.Success(new ResultStatus(true, "Open"))
            : Result.Success(new ResultStatus(false, "Closed"));
    }


    public async Task<Result<Branch>> GetBranchForVault(int? id)
    {
        try
        {
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
        var existingBranch = await context.Branches
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.BranchId == branchId);
        if (existingBranch is null)
        {
            return Result.Failure<BranchDtoUpdate>(ErrorResponse.NotFound("There is no any branch found."));
        }

        var item = existingBranch.Adapt<BranchDtoUpdate>();

        return Result.Success(item);
    }

    public async Task<Result<IEnumerable<VBranchResponse>>> GetBranchesAsync(int? branchId = 0, int? pageSize = 0,
        bool includeInactive = false, string? filter = null)
    {
        // using var db = dapperConnection.CreateConnection();
        //
        // var branches =await db.QueryAsync<VBranchResponse>("spGetBranches", new
        // {
        //     branchId,
        //     pageSize,
        //     includeInactive,
        //     filter
        // },commandType:CommandType.StoredProcedure);
        //
        // return Result.Success(branches);

        IQueryable<VBranchResponse> queries =
            context.Database.SqlQuery<VBranchResponse>($"SELECT * FROM VBranches");

        if (branchId is not null && branchId > 0)
        {
            queries = queries.Where(x => x.BranchId == branchId);
        }

        if (pageSize > 0)
        {
            queries = queries.Take(pageSize.Value);
        }

        if (!includeInactive)
        {
            queries = queries.Where(b => b.IsActive == true);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            queries = queries.Where(x => x.BranchName.Contains(filter));
        }

        var items = await queries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<VBranchResponse>>(items);
    }

    public async Task<Result<IEnumerable<T>>> GetBranchesAsync<T>(int? branchId, int? pageSize,
        bool includeInactive = false, string? filter = null)
        where T : class
    {
        IQueryable<VBranchResponse> queries = context.Database.SqlQuery<VBranchResponse>($"SELECT * FROM VBranches");

        if (branchId is not null && branchId > 0)
        {
            queries = queries.Where(x => x.BranchId == branchId);
        }

        if (pageSize > 0)
        {
            queries = queries.Take(pageSize.Value);
        }

        if (!includeInactive)
        {
            queries = queries.Where(b => b.IsActive == true);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            queries = queries.Where(x => x.BranchName.Contains(filter));
        }

        var items = await queries
            .AsNoTracking()
            .ProjectToType<T>()
            .ToListAsync();

        return Result.Success<IEnumerable<T>>(items);
    }
}
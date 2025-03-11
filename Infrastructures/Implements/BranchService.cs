using Applications.Extensions.Identity;
using Applications.Interfaces.Branches;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Domains.Models.Lists;
using Infrastructures.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public class BranchService : IBranchService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    public BranchService(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task<Result> Create(Branch branch)
    {
        try
        {
            _context.Branches.Add(branch);

            await _context.SaveChangesAsync();

            return Result.Success("New branch was created.");
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
        }

    }

    public async Task<Result> Delete(int id)
    {
        try
        {
            var branch = await _context.Branches.AsNoTracking().Where(x => x.BranchId == id).FirstOrDefaultAsync();

            if (branch is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no branch found."));
            }

            _context.Branches.Remove(branch);

            await _context.SaveChangesAsync();

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
            var existingBranch = _context.Branches.FirstOrDefault(x => x.BranchId == branch.BranchId);


            if (existingBranch is null)
            {
                return Result.Failure<string>(ErrorResponse.Failure("One branch was updated"));
            }
            if (existingBranch.EditSeq != branch.EditSeq)
            {
                return Result.Failure<string>(ErrorResponse.Failure("Branch was chnaged by someone"));
            }


            _context.Entry(existingBranch).CurrentValues.SetValues(branch);

            await _context.SaveChangesAsync();

            return Result.Success("One branch was updated");
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }


    public async Task<Result> UpdateStatusAsync(int branchId)
    {
        try
        {
            var existingBranch = await _context.Branches.AsNoTracking().FirstOrDefaultAsync(x => x.BranchId == branchId);

            if (existingBranch is null)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"No any branch found."));
            }

            existingBranch!.IsActive = existingBranch.IsActive ? false : true;

            _context.Branches.Update(existingBranch);

            await _context.SaveChangesAsync();

            return Result.Success("Branch was updated successfully!");

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }
    public async Task<Result<Branch>> GetBranchById(int id)
    {
        var branch = await _context.
                    Branches
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.BranchId == id);
        if (branch is null)
        {
            return Result.Failure<Branch>(ErrorResponse.NotFound("There is no any branch found."));
        }

        return Result.Success(branch);
    }

    public async Task<Result<IEnumerable<VBranch>>> GetBranchesAsync(int branchId = 0,
        int pageSize = 0,
        bool includeInactive = false,
        string? filter = null)
    {
        IQueryable<VBranch> query = _context.VBranchs.OrderByDescending(x => x.BranchName);

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


        return Result.Success<IEnumerable<VBranch>>(await query.AsNoTracking().ToListAsync());
    }

    public async Task<Pager<VBranch>> GetBranchesAsync(QueryParam param)
    {
        IQueryable<VBranch> query = _context.VBranchs;

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

        return new Pager<VBranch>
        {
            Count = counts,
            Items = await items.AsNoTracking().ToListAsync()
        };
    }

    public async Task<Result<IEnumerable<VBranch>>> GetBranchesAsync(int branchId = 0, int take = 0, string? filter = null)
    {
        IQueryable<VBranch> queries = _context.VBranchs.OrderBy(x => x.BranchName);

        if (!queries.Any())
        {
            return Result.Failure<IEnumerable<VBranch>>(ErrorResponse.NotFound("no data found."));
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

        return Result.Success<IEnumerable<VBranch>>(items);
    }
    public async Task<Result<IEnumerable<VBranch>>> GetBranchesByUser(long userId, int branchId = 0)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result.Failure<IEnumerable<VBranch>>(ErrorResponse.NotFound("No User found in the branch."));
        }

        IQueryable<VBranch> queries = _context.VBranchs;

        if (user.NameRefId > 0)
        {
            queries = queries.Where(x => x.BranchId == user.BranchRefId);
        }

        if (branchId > 0)
        {

            queries = queries.Where(x => x.BranchId == user.BranchRefId);

        }


        return Result.Success<IEnumerable<VBranch>>(await queries.AsNoTracking().ToListAsync());
    }

    public async Task<Result<ResultStatus>> GetBrachStatus(int branchRefId)
    {
        var branch = await _context.VBranchs.Where(x => x.BranchId == branchRefId).FirstOrDefaultAsync();
        if (branch is null)
        {
            return Result.Failure<ResultStatus>(ErrorResponse.NotFound("There is no any branch found."));
        }
        return branch!.OpenStatus == "1"
        ? Result.Success(new ResultStatus(true, "Open"))
        : Result.Success(new ResultStatus(false, "Closed"));
    }


    public async Task<Result<Branch>> GetBranchForVault(int id)
    {
        try
        {
            var branch = await _context.
                    Branches.IgnoreAutoIncludes()
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


}

using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Domains.Models.Collaterals;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public class CollateralsService : ICollateralsService
{
    private readonly ApplicationDbContext _context;
    public CollateralsService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id, string? searchText = null)
    {
        IQueryable<CollateralSubType> queries = _context.CollateralSubTypes;

        if (id > 0)
        {
            queries = queries.Where(x => x.CollateralTypeRefid == id);
        }

        if (!string.IsNullOrEmpty(searchText))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CollateralSubTypeName!, $"%{searchText}%") ||
                                     EF.Functions.Like(x.Id.ToString()!, $"%{searchText}$"));
        }
        return await queries.ProjectToType<CollateralSubTypeResponse>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(bool includeInactive = false)
    {
        IQueryable<CollateralType> query = _context.CollateralTypes;

        if (!includeInactive)
        {
            query = query.Where(x => x.IsActive == true);
        }
        return await query.ProjectToType<CollateralTypeResponse>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? collateralTypeId = 0, string? q = null)
    {
        IQueryable<CollateralType> query = _context.CollateralTypes;
        if (collateralTypeId > 0)
        {
            query = query.Where(x => x.CollateralTypeId == collateralTypeId);
        }

        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(x => EF.Functions.Like(x.CollateralTypeCode!, $"%{q}%"));
        }
        return await query.ProjectToType<CollateralTypeResponse>().AsNoTracking().ToListAsync();
    }


    public async Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null)
    {
        IQueryable<VCollateral> queries = _context
                                            .VCollaterals;

        if (!string.IsNullOrEmpty(q))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CustomerId.ToString(), $"%{q}%") ||
                                         EF.Functions.Like(x.CustomerName!, $"%{q}%") ||
                                         EF.Functions.Like(x.RegisterDate!.ToString(), $"%{q}%"));
        }

        return await queries.AsNoTracking().ToListAsync();

    }

    public async Task<Pager<VCollateral>> GetCollateralsAysnc(int pageIndex, int pageSize,
        string sortColumn, bool sortByAscending, string? q = null)
    {
        IQueryable<VCollateral> queries = _context.VCollaterals;

        if (!string.IsNullOrEmpty(q))
        {
            queries = queries.Where(x => (EF.Functions.Like(x.CustomerId.ToString(), $"%{q}%") ||
                                          EF.Functions.Like(x.RegisterDate!.ToString(), $"%{q}%") ||
                                          EF.Functions.Like(x.CustomerName!, "%" + q + "%")));
        }

        if (sortByAscending)
        {
            queries = queries.OrderBy(x => EF.Property<object>(x, sortColumn));
        }
        else
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, sortColumn));
        }

        var count = queries.Count();
        var collaterals = queries.AsNoTracking().Skip(pageIndex).Take(pageSize);
        return new Pager<VCollateral>
        {
            Items = await collaterals.ToListAsync(),
            Count = count,
        };
    }



    public async Task<Result<CollateralResponse>> GetCollateralByIdAsync(Guid? collateralId)
    {
        try
        {
            var response = _context.Collaterals
                        .Where(x => x.CollateralId == collateralId)
                        .Include(x => x.CollateralLines!.OrderBy(x => x.TypeName));

            if (!response.Any())
            {
                return Result.Failure<CollateralResponse>(ErrorResponse.NotFound("there is no any data found"));
            }

            var item = await response.ProjectToType<CollateralResponse>()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();
            if (item is null)
            {
                return Result.Failure<CollateralResponse>(ErrorResponse.NotFound("there is no any data found"));

            }

            return Result.Success(item);

        }
        catch (Exception ex)
        {
            return Result.Failure<CollateralResponse>(ErrorResponse.NotFound($"{ex.Message}"));
        }

    }

    public async Task<Result<CollateralResponse>> GetCollateralByCustomerAsync(Guid? customerId)
    {
        var response = _context.Collaterals
                        .Include(x => x.CollateralLines)
                        .Where(x => x.CollateralId == customerId);

        var collateral = await response.AsNoTracking()
                                        .ProjectToType<CollateralResponse>()
                                        .FirstOrDefaultAsync();

        if (collateral is null)
        {
            return Result.Failure<CollateralResponse>(ErrorResponse.NotFound("no collateral found"));
        }


        return Result.Success(collateral);
    }
    public async Task<IEnumerable<CollateralLineResponse>> GetCollateralLinesAsync(Guid? collateralId)
    {
        var results = _context.CollateralLines.Where(x => x.CollateralRefId == collateralId);

        var lines = await results.ProjectToType<CollateralLineResponse>()
                            .AsNoTracking().ToListAsync();

        return lines;
    }

    public async Task<Result<string>> InsertOrUpdate(Collateral req)
    {
        try
        {
            var existingCollateral = await _context.Collaterals
                               .Include(c => c.CollateralLines)
                               .FirstOrDefaultAsync(x => x.CollateralId == req.CollateralId);

            if (existingCollateral is null)
            {
                _context.Collaterals.Add(req);
            }
            else
            {
                _context.Entry(existingCollateral).CurrentValues.SetValues(req);

                foreach (var collateralLine in req.CollateralLines)
                {
                    var existingLine = existingCollateral.CollateralLines
                                                .FirstOrDefault(x => x.Id == collateralLine.Id);

                    if (existingLine is null || existingLine.Id == 0)
                    {
                        existingCollateral.CollateralLines.Add(collateralLine);
                    }
                    else
                    {
                        _context.Entry(existingLine).CurrentValues.SetValues(collateralLine);
                    }
                }

                foreach (var item in existingCollateral.CollateralLines!)
                {
                    if (!req.CollateralLines.Any(x => x.Id == item.Id))
                    {
                        _context.Remove(item);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Result.Success("Collateral was changed.");

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }



    }


}

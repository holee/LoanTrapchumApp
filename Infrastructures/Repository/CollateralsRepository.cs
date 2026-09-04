using Applications.Features.Collaterals.Dtos;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructures.Repository;

public sealed class CollateralsRepository(ILogger<CollateralsRepository> logger, DataContext dbContextFactory) : ICollateralsService
{
    public async Task<IEnumerable<CollateralSubTypeDto>> GetCollateralSubTypes(int? id, string? searchText = null)
    {


        logger.LogInformation("GetCollateralSubTypes called with id: {Id} and searchText: {SearchText}", id, searchText);

        var context = dbContextFactory;

        IQueryable<CollateralSubType> queries = context.CollateralSubTypes;

        if (id > 0)
        {
            queries = queries.Where(x => x.CollateralTypeRefId == id);
        }

        if (!string.IsNullOrEmpty(searchText))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CollateralSubTypeName!, $"%{searchText}%") ||
                                     EF.Functions.Like(x.Id.ToString()!, $"%{searchText}$"));
        }
        return await queries.ProjectToType<CollateralSubTypeDto>().AsNoTracking().ToListAsync();
    }


    public async Task<IEnumerable<CollateralSubTypeDto>> GetCollateralSubTypes(int? id, string? searchText = null, CancellationToken cancellationToken = default)
    {


        logger.LogInformation("GetCollateralSubTypes called with id: {Id} and searchText: {SearchText}", id, searchText);

        var context = dbContextFactory;

        IQueryable<CollateralSubType> queries = context.CollateralSubTypes;

        if (id > 0)
        {
            queries = queries.Where(x => x.CollateralTypeRefId == id);
        }

        if (!string.IsNullOrEmpty(searchText))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CollateralSubTypeName!, $"%{searchText}%") ||
                                     EF.Functions.Like(x.Id.ToString()!, $"%{searchText}$"));
        }
        return await queries.ProjectToType<CollateralSubTypeDto>().AsNoTracking().ToListAsync(cancellationToken);
    }


    public async Task<IEnumerable<CollateralTypeDto>> GetCollateralTypes(int? collateralTypeId = 0, string? q = null)
    {

        logger.LogInformation("GetCollateralTypes called with collateralTypeId: {CollateralTypeId} and q: {Q}", collateralTypeId, q);

        var context = dbContextFactory;

        IQueryable<CollateralType> query = context.CollateralTypes;

        if (collateralTypeId > 0)
        {
            query = query.Where(x => x.CollateralTypeId == collateralTypeId);
        }

        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(x => EF.Functions.Like(x.CollateralTypeCode!, $"%{q}%"));
        }
        return await query.ProjectToType<CollateralTypeDto>().AsNoTracking().ToListAsync();
    }
    public async Task<IEnumerable<VCollateral>> GetsCollateralAysnc(string? q = null)
    {

        var context = dbContextFactory;


        IQueryable<VCollateral> queries = context.VCollaterals;

        if (!string.IsNullOrEmpty(q))
        {
            queries = queries.Where(x => EF.Functions.Like(x.FullName, $"%{q}%") ||
                                         EF.Functions.Like(x.CustomerName!, $"%{q}%") ||
                                         EF.Functions.Like(x.RegisterDate!.ToString(), $"%{q}%"));
        }

        return await queries.AsNoTracking().ToListAsync();

    }
    public async Task<Pager<VCollateral>> GetCollateralsAysnc(QueryParam param, CancellationToken cancellation = default)
    {

        logger.LogInformation("GetCollateralsAysnc called with QueryParam: {QueryParam}", param);

        var context = dbContextFactory;

        IQueryable<VCollateral> queries = context.VCollaterals;

        if (!string.IsNullOrEmpty(param.Filter))
        {
            queries = queries.Where(x => (EF.Functions.Like(x.FullName, $"%{param.Filter}%") ||
                                          EF.Functions.Like(x.RegisterDate.ToString(), $"%{param.Filter}%") ||
                                          EF.Functions.Like(x.CustomerName!, $"%{param.Filter}%")));
        }

        if (param.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        var count = queries.Count();

        if (param.Take > 0)
        {
            queries = queries.Skip(param.Page).Take(param.Take);
        }

        var collaterals = await queries.AsNoTracking().ToListAsync(cancellation);

        return new Pager<VCollateral>
        {
            Items = collaterals,
            Count = count,
        };
    }
    public async Task<Result<string>> Upsert(CollateralSubType req, CancellationToken cancellation = default)
    {
        try
        {
            var dataContext = dbContextFactory;

            if (req.Id == 0)
            {
                dataContext.Add(req);
            }
            else
            {
                dataContext.Update(req);
            }

            await dataContext.SaveChangesAsync(cancellation);

            return Result.Success("Collateral was changed.");

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }



    }
    public async Task<Result<string>> CreateAsync(CollateralLine req, CancellationToken cancellation = default)
    {
        try
        {
            var context = dbContextFactory;

            context.Add(req);

            await context.SaveChangesAsync(cancellation);

            return Result.Success("New collateral was added.");

        }
        catch (DbUpdateException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }
    public async Task<Result<string>> UpdateAsync(CollateralLine req, CancellationToken cancellation = default)
    {
        try
        {
            var context = dbContextFactory;

            context.Update(req);

            await context.SaveChangesAsync(cancellation);

            return Result.Success("Collateraline was changed.");

        }
        catch (DbUpdateException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }
    public async Task<Result<string>> UpSertAsync(CollateralLine req, CancellationToken cancellation = default)
    {
        try
        {
            var context = dbContextFactory;

            if (req.Id == 0)
            {
                context.Add(req);
            }
            else
            {
                context.Update(req);
            }

            await context.SaveChangesAsync(cancellation);

            return Result.Success("Collateraline was changed.");

        }
        catch (DbUpdateException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }



    }
    public async Task<Result<string>> InsertOrUpdateAsync(CollateralSubType req, CancellationToken cancellation = default)
    {
        try
        {
            var _context = dbContextFactory;

            if (req.Id == 0)
            {
                _context.CollateralSubTypes.Add(req);
            }
            else
            {
                _context.CollateralSubTypes.Update(req);
            }

            await _context.SaveChangesAsync(cancellation);

            return Result.Success("Collateral was changed.");

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }



    }


}


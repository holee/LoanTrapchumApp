using Applications.Extensions;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Extensions;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Dapper;
using Domains.Models.Collaterals;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructures.Implements;

public sealed class CollateralsRepository(IDapperConnectionFactory connectionFactory, IDbContextFactory<DataContext> dbContextFactory) : ICollateralsService
{
    public async Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id, string? searchText = null)
    {

        await using var context = dbContextFactory.CreateDbContext();


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
        return await queries.ProjectToType<CollateralSubTypeResponse>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(bool includeInactive = false)
    {

        await using var context = dbContextFactory.CreateDbContext();


        IQueryable<CollateralType> query = context.CollateralTypes;

        if (!includeInactive)
        {
            query = query.Where(x => x.IsActive == true);
        }
        return await query.ProjectToType<CollateralTypeResponse>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? collateralTypeId = 0, string? q = null)
    {

        await using var context = dbContextFactory.CreateDbContext();


        IQueryable<CollateralType> query = context.CollateralTypes;

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

        await using var context = dbContextFactory.CreateDbContext();


        IQueryable<VCollateral> queries = context.VCollaterals;

        if (!string.IsNullOrEmpty(q))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CustomerId, $"%{q}%") ||
                                         EF.Functions.Like(x.CustomerName!, $"%{q}%") ||
                                         EF.Functions.Like(x.RegisterDate!.ToString(), $"%{q}%"));
        }

        return await queries.AsNoTracking().ToListAsync();

    }

    public async Task<Pager<VCollateral>> GetCollateralsAysnc(QueryParam param)
    {

        await using var context = dbContextFactory.CreateDbContext();

        IQueryable<VCollateral> queries = context.VCollaterals;

        if (!string.IsNullOrEmpty(param.Filter))
        {
            queries = queries.Where(x => (EF.Functions.Like(x.CustomerId, $"%{param.Filter}%") ||
                                          EF.Functions.Like(x.RegisterDate.ToString(), $"%{param.Filter}%") ||
                                          EF.Functions.Like(x.CustomerName!, "%" + param.Filter + "%")));
        }

        if (param.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        var count = queries.Count();

        if (param.PageSize > 0)
        {
            queries = queries.Skip(param.Skip).Take(param.PageSize);
        }

        var collaterals = await queries.AsNoTracking().ToListAsync();

        return new Pager<VCollateral>
        {
            Items = collaterals,
            Count = count,
        };
    }



    public async Task<Result<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId)
    {
        try
        {

            await using var context = dbContextFactory.CreateDbContext();

            var response = context.Collaterals
                        .Where(x => x.CollateralId == collateralId)
                        .Include(x => x.CollateralLines!.OrderBy(x => x.TypeName));

            if (!response.Any())
            {
                return Result.Failure<VCustomerCollaterals>(ErrorResponse.NotFound("there is no any data found"));
            }

            var item = await response.ProjectToType<VCustomerCollaterals>()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();
            if (item is null)
            {
                return Result.Failure<VCustomerCollaterals>(ErrorResponse.NotFound("there is no any data found"));

            }

            return Result.Success(item);

        }
        catch (Exception ex)
        {
            return Result.Failure<VCustomerCollaterals>(ErrorResponse.NotFound($"{ex.Message}"));
        }

    }

    public async Task<Result<VCustomerCollaterals>> GetCustomerCollateralAsync(string? customerId)
    {

        await using var context = dbContextFactory.CreateDbContext();

        var response = context.Collaterals
                            .Include(x => x.CollateralLines)
                            .Where(x => x.CollateralId == customerId);

        var collateral = await response.AsNoTracking()
                                        .ProjectToType<VCustomerCollaterals>()
                                        .FirstOrDefaultAsync();

        if (collateral is null)
        {
            return Result.Failure<VCustomerCollaterals>(ErrorResponse.NotFound("no collateral found"));
        }


        return Result.Success(collateral);
    }

    public async Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId)
    {

        await using var context = dbContextFactory.CreateDbContext();

        var collateral = await context.EntityNames
                             .Where(x => x.Id!.Equals(customerId))
                             .Include(x => x!.Customer)
                                .ThenInclude(x => x!.Collateral)
                                .ThenInclude(x => x!.CollateralLines)
                        .Select(x => x.ToCollateralResponse())
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

        if (collateral is null)
        {
            return Result.Failure<VCustomerCollaterals>(ErrorResponse.NotFound("No collateral found"));
        }


        return Result.Success(collateral);
    }


    public async Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLinesAsync(string? collateralId)
    {


        using var _connection = connectionFactory.CreateConnection();

        var sql = "SELECT * FROM VCollateralLines WHERE CollateralId=@CollateralId";

        var items = await _connection.QueryAsync<VCollateralLines>(sql, new { collateralId });

        if (!items.Any())
        {
            return Result.Failure<IEnumerable<VCollateralLines>>(ErrorResponse.NotFound("No collateral found"));
        }


        return Result.Success(items);
    }


    public async Task<Result<VCollateralLines>> GetCustomerCollateralLineAsync(long? id, string? collateralId)
    {


        using var _connection = connectionFactory.CreateConnection();

        var sql = "SELECT * FROM VCollateralLines WHERE Id=@Id AND CollateralId=@CollateralId";

        var item = await _connection.QueryFirstAsync<VCollateralLines>(sql, new { id, collateralId });

        if (item is null)
        {
            return Result.Failure<VCollateralLines>(ErrorResponse.NotFound("No collateral found"));
        }


        return Result.Success(item);
    }


    public async Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId, bool includeInUsed)
    {

        if (customerId.IsNullOrEmpty())
        {
            return Result.Failure<VCustomerCollaterals>(ErrorResponse.NotFound("No collateral found"));
        }

        using var _connection = connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@CustomerId", customerId);

        parameters.Add("@IncludeInUsed", includeInUsed);

        var lookup = new Dictionary<string, VCustomerCollaterals>();

        var items = await _connection.QueryAsync<VCustomerCollaterals, VCollateralLines, VCustomerCollaterals>("spGetCustomerCollaterals", (collateral, collateralLine) =>
        {
            if (lookup.TryGetValue(collateral.CollateralId!, out var existingCollateral))
            {
                collateral = existingCollateral!;
            }
            else
            {
                lookup.Add(collateral.CollateralId!, collateral!);
            }
            if (collateralLine is not null)
            {
                collateral.CollateralLines.Add(collateralLine);
            }

            return collateral;

        }, parameters,
        commandType: CommandType.StoredProcedure,
        splitOn: "Id");

        var col = items.FirstOrDefault(x => x.CollateralId!.Equals(customerId));

        //var col = lookup[customerId!];

        if (col is null)
        {
            return Result.Failure<VCustomerCollaterals>(ErrorResponse.NotFound("No collateral found"));
        }

        return Result.Success(col);
    }





    public async Task<IEnumerable<VCollateralLines>> GetCollateralLinesAsync(string? collateralId)
    {

        await using var context = dbContextFactory.CreateDbContext();

        var results = context.CollateralLines.Where(x => x.CollateralRefId == collateralId);

        var lines = await results.ProjectToType<VCollateralLines>()
                            .AsNoTracking().ToListAsync();

        return lines;
    }


    public async Task<Result<string>> InsertOrUpdate(Collateral req, bool detarching, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            var existingCollateral = await context.Collaterals
                               .Include(c => c.CollateralLines)
                               .FirstOrDefaultAsync(x => x.CollateralId == req.CollateralId);

            if (existingCollateral is null)
            {
                context.Collaterals.Add(req);
            }
            else
            {
                context.Entry(existingCollateral).CurrentValues.SetValues(req);

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
                        context.Entry(existingLine).CurrentValues.SetValues(collateralLine);
                    }
                }

                if (detarching)
                {

                    var linesToRemove = existingCollateral.CollateralLines
                                                    .Where(existing => req.CollateralLines.All(incoming => incoming.Id != existing.Id))
                                                    .ToList();
                    context.RemoveRange(linesToRemove);
                }
            }

            await context.SaveChangesAsync();

            return Result.Success("Collateral was changed.");

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }



    }






    public async Task<Result<string>> Upsert(CollateralSubType req, CancellationToken cancellation = default)
    {
        try
        {
            await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellation);

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
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

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
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

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
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

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
            await using var _context = await dbContextFactory.CreateDbContextAsync();

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



//public async Task<Result<string>> InsertOrUpdate(Collateral req, CancellationToken cancellation = default)
//{
//    try
//    {
//        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

//        var existingCollateral = await context.Collaterals
//                           .Include(c => c.CollateralLines)
//                           .FirstOrDefaultAsync(x => x.CollateralId == req.CollateralId);

//        if (existingCollateral is null)
//        {
//            context.Collaterals.Add(req);
//        }
//        else
//        {
//            context.Entry(existingCollateral).CurrentValues.SetValues(req);

//            foreach (var collateralLine in req.CollateralLines)
//            {
//                var existingLine = existingCollateral.CollateralLines
//                                            .FirstOrDefault(x => x.Id == collateralLine.Id);

//                if (existingLine is null || existingLine.Id == 0)
//                {
//                    existingCollateral.CollateralLines.Add(collateralLine);
//                }
//                else
//                {
//                    context.Entry(existingLine).CurrentValues.SetValues(collateralLine);
//                }
//            }

//            foreach (var item in existingCollateral.CollateralLines!)
//            {
//                if (!req.CollateralLines.Any(x => x.Id == item.Id))
//                {
//                    context.Remove(item);
//                }
//            }
//        }

//        await context.SaveChangesAsync();

//        return Result.Success("Collateral was changed.");

//    }
//    catch (Exception ex)
//    {
//        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
//    }



//}

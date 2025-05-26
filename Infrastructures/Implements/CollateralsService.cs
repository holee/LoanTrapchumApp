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
using System.Runtime.CompilerServices;

namespace Infrastructures.Implements;

public class CollateralsService(DataContext context, IDapperConnectionFactory connectionFactory) : ICollateralsService
{
    public async Task<IEnumerable<CollateralSubTypeResponse>> GetCollateralSubTypes(int? id, string? searchText = null)
    {
        IQueryable<CollateralSubType> queries = context.CollateralSubTypes;

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
        IQueryable<CollateralType> query = context.CollateralTypes;

        if (!includeInactive)
        {
            query = query.Where(x => x.IsActive == true);
        }
        return await query.ProjectToType<CollateralTypeResponse>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<CollateralTypeResponse>> GetCollateralTypes(int? collateralTypeId = 0, string? q = null)
    {
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
        IQueryable<VCollateral> queries = context
                                            .VCollaterals;

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
            queries = queries.Skip(param.PageNumber).Take(param.PageSize);
        }

        var collaterals = await queries.ToListAsync();

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


    public async Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId, bool includeInUsed)
    {

        if (customerId.IsNullOrEmpty())
        {
            return Result.Failure<VCustomerCollaterals>(ErrorResponse.NotFound("No collateral found"));
        }


        using var db = connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@CustomerId", customerId);

        parameters.Add("@IncludeInUsed", includeInUsed);

        var lookup = new Dictionary<string, VCustomerCollaterals>();

        var items = await db.QueryAsync<VCustomerCollaterals, VCollateralLines, VCustomerCollaterals>("spGetCustomerCollaterals", (collateral, collateralLine) =>
        {
            if (lookup.TryGetValue(collateral.CollateralId!, out var existingCollateral))
            {
                collateral = existingCollateral!;
            }
            else
            {
                lookup.Add(collateral.CollateralId!, collateral!);
            }
            if(collateralLine is not null)
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





    public async Task<IEnumerable<Applications.Responses.Lists.Collaterals.VCollateralLines>> GetCollateralLinesAsync(string? collateralId)
    {
        var results = context.CollateralLines.Where(x => x.CollateralRefId == collateralId);

        var lines = await results.ProjectToType<VCollateralLines>()
                            .AsNoTracking().ToListAsync();

        return lines;
    }

    public async Task<Result<string>> InsertOrUpdate(Collateral req)
    {
        try
        {
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

                foreach (var item in existingCollateral.CollateralLines!)
                {
                    if (!req.CollateralLines.Any(x => x.Id == item.Id))
                    {
                        context.Remove(item);
                    }
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


}

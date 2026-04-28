using Applications.Bases;
using Applications.Commons.Messages.Collaterals;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Response;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces.Customers;
using Applications.Responses.Paginations;
using Dapper;
using Domains.Models.Collaterals;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;

namespace Infrastructures.Implements;

public sealed class CollateralRepository : ICollateralService
{
    private readonly ILogger<CollateralRepository> logger;
    private readonly IDbContextFactory<DataContext> dbContextFactory;
    private readonly IDapperConnectionFactory connectionFactory;

    public CollateralRepository(ILogger<CollateralRepository> logger, IDbContextFactory<DataContext> dbContextFactory,
        IDapperConnectionFactory connectionFactory)
    {
        this.logger = logger;
        this.dbContextFactory = dbContextFactory;
        this.connectionFactory = connectionFactory;
    }


    public async Task<ErrorOr<CollateralLine>> GetCollateralLineByIdAsync(long? id, string? collateralId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var rsp = await context.CollateralLines
            .AsNoTracking()
            .Where(x => x.Id == id && x.CollateralRefId == collateralId)
            .FirstOrDefaultAsync();
        if (rsp is null)
        {
            return Error.NotFound(CollateralMessages.NotFound);
        }

        return rsp;
    }



    public async Task<ErrorOr<VCollateralLines>> GetCollateralLinesAsync(long? id, string? collateralId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var rsp = await context.CollateralLines
            .AsNoTracking()
            .Where(x => x.Id == id && x.CollateralRefId == collateralId)
            .Select(ProjectToCollateral())
            .FirstOrDefaultAsync();
        if (rsp is null)
        {
            return Error.NotFound(CollateralMessages.NotFound);
        }

        return rsp;
    }

    public async Task<ErrorOr<Collateral>> GetCollateralByIdAsync(string? customerId, CancellationToken cancellation = default)
    {
        await using var context = dbContextFactory.CreateDbContext();

        var collateral = await context.Collaterals
            .Where(x => x.CollateralId == customerId)
            .Include(x => x.CollateralLines)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellation);

        return collateral is null ? Error.NotFound(CollateralMessages.NotFound) : collateral;
    }

    public async Task<ErrorOr<Collateral>> GetEditCollateralByIdAsync(long lineId, string? customerId, CancellationToken cancellation = default)
    {
        await using var context = dbContextFactory.CreateDbContext();

        var collateral = await context.Collaterals
            .Include(c => c.CollateralLines)
            .AsNoTracking()
            .Where(c => c.CollateralLines.Any(line =>
                line.Id == lineId && line.CollateralRefId == customerId))
            .FirstOrDefaultAsync(cancellation);

        if (collateral is null)
            return Error.NotFound(CollateralMessages.NotFound);

        return collateral;
    }




    public async Task<ErrorOr<VCollateralLines>> GetCollateralLineDetailAsync(long? id, string? collateralId,
        CancellationToken cancellation = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

        var sql = @"
                SELECT * FROM VCollateralLines WHERE Id=@Id AND CollateralId=@CollateralId;
                SELECT 
                    d.LoanId,
                    d.InstallmentType,
                    d.Principle,
                    d.PeriodName,
                    d.InterestRate,
                    d.PaidPrinciple,
                    d.PaidInterest,
                    d.OutStanding,
                    d.BranchName,
                    d.LoanStatus
                    FROM VLoansDisbursments d
                    INNER JOIN VLoanRequestCollaterals r
                    ON d.LoanRequestId=r.LoanRequestId
                    WHERE r.CollateralLineId=@Id AND r.CollateralId=@CollateralId;
                ";

        var items = await connection.QueryMultipleAsync(sql, new { id, collateralId });

        var item = await items.ReadFirstOrDefaultAsync<VCollateralLines>();

        if (item is null)
        {
            return Error.NotFound(CollateralMessages.NotFound);
        }

        var loanDisbursements = (await items.ReadAsync<VLoanDisbursments>()).ToList();

        if (loanDisbursements.Count > 0)
        {
            item.LoanDisbursements = loanDisbursements;
        }

        return item;
    }

    public async Task<ErrorOr<IEnumerable<VCollateralLines>>> GetCollateralLinesAsync(string? collateralId)
    {
        using var connection = connectionFactory.CreateConnection();

        var sql = "SELECT * FROM VCollateralLines WHERE CollateralId=@CollateralId";

        var items = await connection.QueryAsync<VCollateralLines>(sql, new { collateralId });

        return !items.Any() ? Error.NotFound(CollateralMessages.NotFound) : ErrorOrFactory.From(items);
    }

    public async Task<ErrorOr<VCustomerCollaterals>> GetCollateralDetailAsync(string? customerId,
        bool includeInUsed = true, CancellationToken cancellation = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

        var parameters = new DynamicParameters();

        parameters.Add("@CustomerId", customerId);

        parameters.Add("@IncludeInUsed", includeInUsed);

        var lookup = new Dictionary<string, VCustomerCollaterals>();

        var items = await connection.QueryAsync<VCustomerCollaterals, VCollateralLines, VCustomerCollaterals>(
            "spGetCustomerCollaterals", (collateral, collateralLine) =>
            {
                if (lookup.TryGetValue(collateral.CollateralId!, out var existingCollateral))
                {
                    collateral = existingCollateral!;
                }
                else
                {
                    lookup.Add(collateral.CollateralId!, collateral!);
                }

                collateral.CollateralLines.Add(collateralLine);

                return collateral;
            }, parameters, commandType: CommandType.StoredProcedure, splitOn: "Id");

        var col = items.FirstOrDefault(x => x.CollateralId!.Equals(customerId));

        //var col = lookup[customerId!];

        if (col is null)
        {
            return Error.NotFound(CollateralMessages.NotFound);
        }

        return col;
    }

    public async Task<ErrorOr<string>> AddOrUpdateAsync(Collateral req, bool detarching, CancellationToken cancellation = default)
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

            return CollateralMessages.Created;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error while inserting or updating collateral with ID: {req.CollateralId}");
            return Error.Failure($"{ex.Message}");
        }
    }

    public async Task<ErrorOr<string>> InsertOrUpdateCollateralLineAsync(CollateralLine model, CancellationToken cancellation = default)
    {
        logger.LogInformation("Upserting collateral line with ID: {Id}", model.Id);
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            if (model.Id == 0)
            {
                context.CollateralLines.Add(model);
            }
            else
            {
                context.CollateralLines.Update(model);
            }

            await context.SaveChangesAsync(cancellation);

            return CollateralMessages.Created;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, $"Concurrency error while upserting collateral line with {ex!.Message}");

            return Error.Failure($"{ex.Message}");
        }
    }

    public async Task<ErrorOr<DeleteResult>> DeleteCollateralLineAsync(long? id, string? collateralId, CancellationToken cancellation = default)
    {
        logger.LogInformation("try to delete collateral line with ID: {Id} & Collateral:{collateralId}", id, collateralId);
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);


            //check collateral line is in use

            if (await IsCollateralInUsed(id, collateralId, context))
            {
                return DeleteResult.InUse("Collateral line is in use and cannot be deleted.");
            }

            var model = await context.CollateralLines
                .FirstOrDefaultAsync(x => x.Id == id && x.CollateralRefId == collateralId, cancellation);

            if (model is null)
            {
                return DeleteResult.NotFound("No Collateral Not Found");
            }

            context.CollateralLines.Remove(model);

            await context.SaveChangesAsync(cancellation);

            return DeleteResult.Success("Collateral line was deleted.");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, $"Concurrency error while upserting collateral line with {ex!.Message}");

            return Error.Failure($"{ex.Message}");
        }
    }

    private async Task<bool> IsCollateralInUsed(long? id, string? collateralId, DataContext context)
    {
        var query = context.VCustomerCollateralLines.AsNoTracking().AsQueryable();

        if (id.HasValue)
            query = query.Where(x => x.Id == id.Value);

        if (!string.IsNullOrEmpty(collateralId))
            query = query.Where(x => x.CollateralId == collateralId);

        return await query.AnyAsync(x => x.IsUsed == true);
    }

    private static Expression<Func<CollateralLine, VCollateralLines>> ProjectToCollateral()
    {
        return x => new VCollateralLines
        {
            Id = x.Id,
            ReferenceNo = x.ReferenceNo,
            CollateralId = x.CollateralRefId,
            OwnerName = x.OwnerName,
            CollateralSubTypeName = x.SubTypeName,
            CollateralNo = x.CollateralNo,
            TypeName = x.TypeName,
            Size = x.Size,
            HotPrice = x.HotPrice,
            PawnPrice = x.PawnPrice,
            MarketPrice = x.MarketPrice,
            Location = x.Location,
            Description = x.Description,
            Quantity = x.Quantity,
            IsUsed = x.IsUsed,
            WareHouse = x.WareHouse
        };
    }

    public async Task<Pager<VCollateralLines>> GetCollateralLinesAsPaging(string? customerId, QueryParam query, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var baseQuery = context.VCollateralLines.AsQueryable();

        if (!string.IsNullOrEmpty(customerId))
        {
            baseQuery = baseQuery.Where(x => x.CollateralId == customerId);
        }

        // Apply filtering
        baseQuery = ApplyFilter(query, baseQuery);

        // Continue with pagination logic here

        baseQuery = ApplyOrdering(baseQuery, query);

        var totalItems = await baseQuery.CountAsync(cancellation);

        var items = await baseQuery
                                .Skip(query.Page)
                                .Take(query.PageSize)
                                .AsNoTracking()
                                .ToListAsync(cancellation);
        return new Pager<VCollateralLines>(items, totalItems);
    }


    private IQueryable<VCollateralLines> ApplyOrdering(IQueryable<VCollateralLines> query, QueryParam param)
    {
        if (!string.IsNullOrWhiteSpace(param.SortColumn))
        {
            var property = typeof(VCollateralLines).GetProperty(param.SortColumn);

            if (property != null)
            {
                query = query.ApplyOrdering(param);
            }
        }
        else
        {
            query = query.OrderBy(x => x.LoanStatus)
                          .ThenBy(x => x.TypeName);
        }

        return query;
    }



    private static IQueryable<VCollateralLines> ApplyFilter(QueryParam query, IQueryable<VCollateralLines> baseQuery)
    {
        // No filter supplied -> return original query

        var filterValue = query.Filter ?? string.Empty;

        var pattern = $"%{filterValue}%";

        // If a specific column is targeted, apply a narrowed predicate

        if (!string.IsNullOrEmpty(pattern) && string.IsNullOrWhiteSpace(query.FilterColumn))
        {
            string[] filterColumns = [nameof(VCollateralLines.TypeName), nameof(VCollateralLines.CollateralSubTypeName), nameof(VCollateralLines.OwnerName), nameof(VCollateralLines.CollateralNo)];

            baseQuery = baseQuery.WhereMatchesFilter(pattern, filterColumns);

            return baseQuery;

        }


        if (!string.IsNullOrEmpty(pattern) && !string.IsNullOrEmpty(query.FilterColumn))
        {
            var property = typeof(VCollateralLines).GetProperty(query.FilterColumn);

            if (property != null)
            {
                baseQuery = baseQuery.Where(e => EF.Functions.Like(EF.Property<string>(e, query.FilterColumn), $"{pattern}%"));
            }
        }

        return baseQuery;
    }
}



//public async Task<ErrorOr<VCustomerCollaterals>> GetCollateralByIdAsync(string? collateralId)
//{
//    try
//    {
//        await using var context = dbContextFactory.CreateDbContext();

//        var colateral = await context.Collaterals
//            .Where(x => x.CollateralId == collateralId)
//            .Include(x => x.CollateralLines!.OrderBy(x => x.TypeName))
//            .ProjectToType<VCustomerCollaterals>()
//            .AsNoTracking()
//            .FirstOrDefaultAsync();

//        if (colateral is null)
//        {
//            return Error.NotFound(CollateralMessages.NotFound);
//        }

//        return colateral;
//    }
//    catch (Exception ex)
//    {
//        return Error.Unexpected($"An error occurred while retrieving collateral: {ex.Message}");
//    }
//}


//public async Task<ErrorOr<VCustomerCollaterals>> GetCollateralsAsync(string? customerId)
//  {

//      await using var context = dbContextFactory.CreateDbContext();

//      var collateral = await context.EntityNames
//                           .Where(x => x.Id!.Equals(customerId))
//                           .Include(x => x!.Customer)
//                              .ThenInclude(x => x!.Collateral)
//                              .ThenInclude(x => x!.CollateralLines)
//                       .AsNoTracking()
//                      .Select(x => x.ToCollateralResponse())
//                      .FirstOrDefaultAsync();

//      if (collateral is null)
//      {
//          return Result.Failure<VCustomerCollaterals>(ErrorResponse.NotFound("No collateral found"));
//      }
using Applications.Bases;
using Applications.Extensions;
using Applications.Features.Collaterals.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Dapper;
using Domains.Models.Collaterals;
using Infrastructures.DataAccess;
using Infrastructures.DataAccess.Factories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;

namespace Infrastructures.Implements;

public sealed class CustomerCollateralRepository : ICustomerCollateralService
{
    private readonly ILogger<CustomerCollateralRepository> logger;
    private readonly IDbContextFactory<DataContext> dbContextFactory;
    private readonly IDapperConnectionFactory connectionFactory;

    public CustomerCollateralRepository(ILogger<CustomerCollateralRepository> logger, IDbContextFactory<DataContext> dbContextFactory, IDapperConnectionFactory connectionFactory)
    {
        this.logger = logger;
        this.dbContextFactory = dbContextFactory;
        this.connectionFactory = connectionFactory;
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

    public async Task<IEnumerable<VCollateralLines>> GetCollateralLinesAsync(string? collateralId)
    {

        await using var context = dbContextFactory.CreateDbContext();

        var results = context.CollateralLines.Where(x => x.CollateralRefId == collateralId);

        var lines = await results.ProjectToType<VCollateralLines>()
                            .AsNoTracking().ToListAsync();

        return lines;
    }

    public async Task<VCollateralLines?> GetCollateralLinesAsync(long? id, string? collateralId)
    {
        if (id is null || string.IsNullOrWhiteSpace(collateralId))

            return null;

        await using var context = await dbContextFactory.CreateDbContextAsync();

        return await context.CollateralLines
            .AsNoTracking()
            .Where(x => x.Id == id && x.CollateralRefId == collateralId)
            .Select(ProjectToView())
            .FirstOrDefaultAsync();
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
    public async Task<Result<VCustomerCollaterals>> GetCustomerCollateralsAsync(string? customerId)
    {

        await using var context = dbContextFactory.CreateDbContext();

        var collateral = await context.EntityNames
                             .Where(x => x.Id!.Equals(customerId))
                             .Include(x => x!.Customer)
                                .ThenInclude(x => x!.Collateral)
                                .ThenInclude(x => x!.CollateralLines)
                         .AsNoTracking()
                        .Select(x => x.ToCollateralResponse())
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
    public async Task<Result<string>> UpsertCollateralLineAsync(CollateralLine model, CancellationToken cancellation = default)
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

            return Result.Success("Collateral was updated.");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, $"Concurrency error while upserting collateral line with {ex!.Message}");

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }

    }

    public async Task<Result<DeleteResult>> DeleteCollateralLineAsync(long? id, string? collateralId, CancellationToken cancellation = default)
    {
        logger.LogInformation("Upserting collateral line with ID: {Id} & Collateral:{collateralId}", id, collateralId);
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);


            //check collateral line is in use

            if (await IsCollateralInUsed(id, collateralId, context))
            {
                return Result.Success(DeleteResult.InUse("Collateral line is in use and cannot be deleted."));
            }

            var model = await context.CollateralLines
                                    .FirstOrDefaultAsync(x => x.Id == id &&
                                                         x.CollateralRefId == collateralId, cancellation);

            if (model is null)
            {
                return Result.Success(DeleteResult.NotFound("No Collateral Not Found"));
            }

            context.CollateralLines.Remove(model);

            await context.SaveChangesAsync(cancellation);

            return Result.Success(new DeleteResult(DeleteStatus.Success, "Collateral line was deleted."));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, $"Concurrency error while upserting collateral line with {ex!.Message}");

            return Result.Failure<DeleteResult>(ErrorResponse.ServerError($"{ex.Message}"));
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

    private static Expression<Func<CollateralLine, VCollateralLines>> ProjectToView()
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

}
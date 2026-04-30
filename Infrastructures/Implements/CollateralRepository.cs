using Applications.Bases;
using Applications.Commons.Messages.Collaterals;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Response;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces.Customers;
using Applications.Responses.Paginations;
using Dapper;
using Domains.Models.Collaterals;
using Domains.Models.Lists;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
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

    public CollateralRepository(
        ILogger<CollateralRepository> logger,
        IDbContextFactory<DataContext> dbContextFactory,
        IDapperConnectionFactory connectionFactory)
    {
        this.logger = logger;
        this.dbContextFactory = dbContextFactory;
        this.connectionFactory = connectionFactory;
    }

    public async Task<ErrorOr<CollateralLine>> GetCollateralLineByIdAsync(long? id, string? collateralId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var line = await context.CollateralLines
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.CollateralRefId == collateralId);

        return line is null ? Error.NotFound(CollateralMessages.NotFound) : line;
    }

    public async Task<ErrorOr<VCollateralLines>> GetCollateralLinesAsync(long? id, string? collateralId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var line = await context.CollateralLines
            .AsNoTracking()
            .Where(x => x.Id == id && x.CollateralRefId == collateralId)
            .Select(ProjectToCollateral())
            .FirstOrDefaultAsync();

        return line is null ? Error.NotFound(CollateralMessages.NotFound) : line;
    }

    public async Task<ErrorOr<Collateral>> GetCollateralByIdAsync(string? customerId, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var collateral = await context.Collaterals
            .AsNoTracking()
            .Include(x => x.CollateralLines)
            .FirstOrDefaultAsync(x => x.CollateralId == customerId, cancellation);

        return collateral is null ? Error.NotFound(CollateralMessages.NotFound) : collateral;
    }

    public async Task<ErrorOr<Collateral>> GetEditCollateralByIdAsync(long lineId, string? customerId, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var collateral = await context.Collaterals
            .AsNoTracking()
            .Include(c => c.CollateralLines)
            .FirstOrDefaultAsync(c => c.CollateralLines.Any(
                line => line.Id == lineId && line.CollateralRefId == customerId), cancellation);

        return collateral is null ? Error.NotFound(CollateralMessages.NotFound) : collateral;
    }

    public async Task<ErrorOr<VCollateralLines>> GetCollateralLineDetailAsync(long? id, string? collateralId,
        CancellationToken cancellation = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

        const string sql = @"
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
            INNER JOIN VLoanRequestCollaterals r ON d.LoanRequestId = r.LoanRequestId
            WHERE r.CollateralLineId = @Id AND r.CollateralId = @CollateralId;";

        using var multi = await connection.QueryMultipleAsync(sql, new { id, collateralId });

        var item = await multi.ReadFirstOrDefaultAsync<VCollateralLines>();
        if (item is null)
            return Error.NotFound(CollateralMessages.NotFound);

        var loanDisbursements = (await multi.ReadAsync<VLoanDisbursments>()).ToList();
        if (loanDisbursements.Count > 0)
            item.LoanDisbursements = loanDisbursements;

        return item;
    }

    public async Task<ErrorOr<IEnumerable<VCollateralLines>>> GetCollateralLinesAsync(string? collateralId)
    {
        using var connection = connectionFactory.CreateConnection();

        var items = await connection.QueryAsync<VCollateralLines>(
            "SELECT * FROM VCollateralLines WHERE CollateralId=@CollateralId",
            new { collateralId });

        return items.Any() ? ErrorOrFactory.From(items) : Error.NotFound(CollateralMessages.NotFound);
    }

    public async Task<ErrorOr<VCustomerCollaterals>> GetCollateralDetailAsync(string? customerId,
        bool includeInUsed = true, CancellationToken cancellation = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellation);

        var parameters = new DynamicParameters();
        parameters.Add("@CustomerId", customerId);
        parameters.Add("@IncludeInUsed", includeInUsed);

        var lookup = new Dictionary<string, VCustomerCollaterals>();

        await connection.QueryAsync<VCustomerCollaterals, VCollateralLines, VCustomerCollaterals>(
            "spGetCustomerCollaterals",
            (collateral, collateralLine) =>
            {
                if (!lookup.TryGetValue(collateral.CollateralId!, out var existing))
                {
                    existing = collateral;
                    lookup[collateral.CollateralId!] = existing;
                }
                existing.CollateralLines.Add(collateralLine);
                return existing;
            },
            parameters,
            commandType: CommandType.StoredProcedure,
            splitOn: "Id");

        return lookup.TryGetValue(customerId!, out var result)
            ? result
            : Error.NotFound(CollateralMessages.NotFound);
    }

    public async Task<ErrorOr<string>> AddOrUpdateAsync(Collateral req, bool detarching, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            await SyncLocationsAsync(context, req.CollateralLines, cancellation);

            var existing = await context.Collaterals
                .Include(c => c.CollateralLines)
                .FirstOrDefaultAsync(x => x.CollateralId == req.CollateralId, cancellation);

            if (existing is null)
                context.Collaterals.Add(req);
            else
            {
                context.Entry(existing).CurrentValues.SetValues(req);

                UpsertCollateralLines(context, existing, req.CollateralLines, detarching);
            }

            await context.SaveChangesAsync(cancellation);

            AssignReferenceNumbers(existing?.CollateralLines ?? req.CollateralLines);

            await context.SaveChangesAsync(cancellation);

            return CollateralMessages.Created;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while inserting or updating collateral with ID: {CollateralId}", req.CollateralId);
            return Error.Failure(ex.Message);
        }
    }

    private static async Task SyncLocationsAsync(DataContext context, IEnumerable<CollateralLine> lines, CancellationToken cancellation)
    {
        var locations = lines
            .Select(x => x.Location)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Distinct()
            .ToList();

        if (locations.Count == 0) return;

        var existingLocations = await context.Locations
            .Where(x => locations.Contains(x.LocationName))
            .Select(x => x.LocationName)
            .ToHashSetAsync(cancellation);

        var newLocations = locations
            .Where(l => !existingLocations.Contains(l))
            .Select(l => new LtLocation { LocationName = l });

        await context.Locations.AddRangeAsync(newLocations, cancellation);
    }

    public async Task<ErrorOr<string>> InsertOrUpdateCollateralLineAsync(CollateralLine model, CancellationToken cancellation = default)
    {
        logger.LogInformation("Upserting collateral line with ID: {Id}", model.Id);
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            if (model.Id == 0)
                context.CollateralLines.Add(model);
            else
                context.CollateralLines.Update(model);

            await context.SaveChangesAsync(cancellation);

            return CollateralMessages.Created;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "Concurrency error while upserting collateral line: {Message}", ex.Message);
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<DeleteResult>> DeleteCollateralLineAsync(long? id, string? collateralId, CancellationToken cancellation = default)
    {
        logger.LogInformation("Deleting collateral line ID: {Id}, Collateral: {CollateralId}", id, collateralId);
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            if (await IsCollateralInUsedAsync(id, collateralId, context))
                return DeleteResult.InUse("Collateral line is in use and cannot be deleted.");

            var model = await context.CollateralLines
                .FirstOrDefaultAsync(x => x.Id == id && x.CollateralRefId == collateralId, cancellation);

            if (model is null)
                return DeleteResult.NotFound("Collateral line not found.");

            context.CollateralLines.Remove(model);
            await context.SaveChangesAsync(cancellation);

            return DeleteResult.Success("Collateral line was deleted.");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "Concurrency error while deleting collateral line: {Message}", ex.Message);
            return Error.Failure(ex.Message);
        }
    }

    public async Task<Pager<VCollateralLines>> GetCollateralLinesAsPaging(string? customerId, QueryParam query, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var baseQuery = context.VCollateralLines.AsQueryable();

        if (!string.IsNullOrEmpty(customerId))
            baseQuery = baseQuery.Where(x => x.CollateralId == customerId);

        baseQuery = ApplyFilter(query, baseQuery);
        baseQuery = ApplyOrdering(baseQuery, query);

        var totalItems = await baseQuery.CountAsync(cancellation);

        var items = await baseQuery
            .Skip(query.Page)
            .Take(query.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellation);

        return new Pager<VCollateralLines>(items, totalItems);
    }

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------

    private static void UpsertCollateralLines(
        DataContext context,
        Collateral existing,
        IEnumerable<CollateralLine> incomingLines,
        bool detarching)
    {
        foreach (var incoming in incomingLines)
        {
            var existingLine = existing.CollateralLines.FirstOrDefault(x => x.Id == incoming.Id);

            if (existingLine is null || existingLine.Id == 0)
                existing.CollateralLines.Add(incoming);
            else
                context.Entry(existingLine).CurrentValues.SetValues(incoming);
        }

        if (detarching)
        {
            var toRemove = existing.CollateralLines
                .Where(e => incomingLines.All(i => i.Id != e.Id))
                .ToList();
            context.RemoveRange(toRemove);
        }
    }

    private static void AssignReferenceNumbers(IEnumerable<CollateralLine> lines)
    {
        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty(line.ReferenceNo))
                continue;

            line.ReferenceNo = line.CategoryRefId switch
            {
                1 => BuildReferenceNo("PRO", line),
                3 => BuildReferenceNo("PER", line),
                _ => line.ReferenceNo
            };
        }
    }

    private static string BuildReferenceNo(string prefix, CollateralLine line) => $"LK_COL_{prefix}-{line.Id:D6}";

    private static Task<bool> IsCollateralInUsedAsync(long? id, string? collateralId, DataContext context)
    {
        var query = context.VCustomerCollateralLines.AsNoTracking().AsQueryable();

        if (id.HasValue)
            query = query.Where(x => x.Id == id.Value);

        if (!string.IsNullOrEmpty(collateralId))
            query = query.Where(x => x.CollateralId == collateralId);

        return query.AnyAsync(x => x.IsUsed == true);
    }

    private IQueryable<VCollateralLines> ApplyOrdering(IQueryable<VCollateralLines> query, QueryParam param)
    {
        if (!string.IsNullOrWhiteSpace(param.SortColumn) &&
            typeof(VCollateralLines).GetProperty(param.SortColumn) is not null)
        {
            return query.ApplyOrdering(param);
        }

        return query
            .OrderBy(x => x.LoanStatus)
            .ThenBy(x => x.TypeName);
    }

    private static IQueryable<VCollateralLines> ApplyFilter(QueryParam query, IQueryable<VCollateralLines> baseQuery)
    {
        var filterValue = query.Filter ?? string.Empty;
        var pattern = $"%{filterValue}%";

        if (string.IsNullOrWhiteSpace(query.FilterColumn))
        {
            string[] defaultColumns =
            [
                nameof(VCollateralLines.TypeName),
                nameof(VCollateralLines.CollateralSubTypeName),
                nameof(VCollateralLines.OwnerName),
                nameof(VCollateralLines.CollateralNo)
            ];
            return baseQuery.WhereMatchesFilter(pattern, defaultColumns);
        }

        if (!string.IsNullOrEmpty(query.FilterColumn) &&
            typeof(VCollateralLines).GetProperty(query.FilterColumn) is not null)
        {
            baseQuery = baseQuery.Where(e =>
                EF.Functions.Like(EF.Property<string>(e, query.FilterColumn), $"{pattern}%"));
        }

        return baseQuery;
    }

    private static Expression<Func<CollateralLine, VCollateralLines>> ProjectToCollateral() =>
        x => new VCollateralLines
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
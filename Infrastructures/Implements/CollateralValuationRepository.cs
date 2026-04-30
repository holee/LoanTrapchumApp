using Applications.Features.CollateralValuations.Dtos;
using Applications.Interfaces;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;
using ErrorOr;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class CollateralValuationRepository(
    ILogger<CollateralValuationRepository> logger,
    IDbContextFactory<DataContext> dbContextFactory) : ICollateralValuationService
{
    public async Task<IEnumerable<CollateralValuationDto>> GetAllAsync(string? searchText = null, CancellationToken cancellationToken = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        IQueryable<CollateralEvaluations> query = context.CollateralEvaluations;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(x =>
                EF.Functions.Like(x.CollateralRefId ?? string.Empty, $"%{searchText}%") ||
                EF.Functions.Like(x.ValueBy ?? string.Empty, $"%{searchText}%"));
        }

        return await query.OrderByDescending(x => x.ModifiedDT)
                          .ProjectToType<CollateralValuationDto>()
                          .AsNoTracking()
                          .ToListAsync(cancellationToken);
    }

    public async Task<Pager<CollateralValuationDto>> GetAsPagingAsync(QueryParam param, CancellationToken cancellationToken = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        IQueryable<CollateralEvaluations> query = context.CollateralEvaluations;

        if (!string.IsNullOrWhiteSpace(param.Filter))
        {
            query = query.Where(x =>
                EF.Functions.Like(x.CollateralRefId ?? string.Empty, $"%{param.Filter}%") ||
                EF.Functions.Like(x.ValueBy ?? string.Empty, $"%{param.Filter}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var skip = param.Page * param.PageSize;
        var take = param.PageSize > 0 ? param.PageSize : 15;

        var items = await query
            .OrderByDescending(x => x.ModifiedDT)
            .Skip(skip)
            .Take(take)
            .ProjectToType<CollateralValuationDto>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new Pager<CollateralValuationDto>(items, totalCount);
    }

    public async Task<ErrorOr<CollateralValuationDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var item = await context.CollateralEvaluations
            .Where(x => x.Id == id)
            .ProjectToType<CollateralValuationDto>()
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (item is null)
            return Error.NotFound(description: "Collateral valuation not found.");

        return item;
    }

    public async Task<ErrorOr<string>> CreateAsync(CollateralEvaluations entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            await context.CollateralEvaluations.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("CollateralValuation created with Id: {Id}", entity.Id);
            return "Collateral valuation created successfully.";
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error creating CollateralValuation");
            return Error.Unexpected(description: ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ErrorOr<string>> UpdateAsync(CollateralEvaluations entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var existing = await context.CollateralEvaluations.FindAsync([entity.Id], cancellationToken);
            if (existing is null)
                return Error.NotFound(description: "Collateral valuation not found.");

            existing.CollateralLineRefId = entity.CollateralLineRefId;
            existing.CollateralRefId = entity.CollateralRefId;
            existing.ValueDate = entity.ValueDate;
            existing.ValueBy = entity.ValueBy;
            existing.ValueAmount = entity.ValueAmount;
            existing.CurrencyRefId = entity.CurrencyRefId;
            existing.ModifiedDT = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("CollateralValuation updated with Id: {Id}", entity.Id);
            return "Collateral valuation updated successfully.";
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error updating CollateralValuation");
            return Error.Conflict(description: ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var existing = await context.CollateralEvaluations.FindAsync([id], cancellationToken);
            if (existing is null)
                return Error.NotFound(description: "Collateral valuation not found.");

            context.CollateralEvaluations.Remove(existing);
            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("CollateralValuation deleted with Id: {Id}", id);
            return true;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error deleting CollateralValuation");
            return Error.Unexpected(description: ex.InnerException?.Message ?? ex.Message);
        }
    }
}

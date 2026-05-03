using Applications.Features.Periods.Dto;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Domains.Models;
using ErrorOr;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class PeriodRepository(
    IDbContextFactory<DataContext> contextFactory,
    ILogger<PeriodRepository> logger) : IPeriodService
{
    public async Task<Pager<PeriodDto>> GetAllAsync(QueryParam? param, CancellationToken cancellation = default)
    {
        logger.LogInformation("Fetching all periods");

        param ??= new QueryParam();

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var query = context.LtPeriods.AsQueryable();

        if (!string.IsNullOrWhiteSpace(param.Filter))
        {
            query = query.Where(x => x.Period.ToString().Contains(param.Filter));
        }

        var count = await query.CountAsync(cancellation);

        if (param.Take > 0)
        {
            query = query.OrderBy(x => x.Period)
                         .Skip(param.Page)
                         .Take(param.Take);
        }

        var items = await query
            .AsNoTracking()
            .Select(x => new PeriodDto { Id = x.Id, Period = x.Period })
            .ToListAsync(cancellation);

        return new Pager<PeriodDto>(items, count);
    }

    public async Task<PeriodDto?> GetByIdAsync(int id, CancellationToken cancellation = default)
    {
        logger.LogInformation("Fetching period with ID: {Id}", id);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var item = await context.LtPeriods
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellation);

        return item is null ? null : new PeriodDto { Id = item.Id, Period = item.Period };
    }

    public async Task<ErrorOr<PeriodDto>> CreateAsync(PeriodDtoCreate request, CancellationToken cancellation = default)
    {
        logger.LogInformation("Creating period: {Period}", request.Period);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var exists = await context.LtPeriods.AnyAsync(x => x.Period == request.Period, cancellation);
       
        if (exists)
        {
            return Error.Conflict(description: $"Period {request.Period} already exists.");
        }

        var entity = new LtPeriod { Period = request.Period };
        context.LtPeriods.Add(entity);
        await context.SaveChangesAsync(cancellation);

        return new PeriodDto { Id = entity.Id, Period = entity.Period };
    }

    public async Task<ErrorOr<PeriodDto>> UpdateAsync(PeriodDtoUpdate request, CancellationToken cancellation = default)
    {
        logger.LogInformation("Updating period ID: {Id}", request.Id);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var entity = await context.LtPeriods.FirstOrDefaultAsync(x => x.Id == request.Id, cancellation);
        if (entity is null)
        {
            return Error.NotFound(description: $"Period with ID {request.Id} was not found.");
        }

        var duplicate = await context.LtPeriods.AnyAsync(x => x.Period == request.Period && x.Id != request.Id, cancellation);
        if (duplicate)
        {
            return Error.Conflict(description: $"Period {request.Period} already exists.");
        }

        entity.Period = request.Period;
        await context.SaveChangesAsync(cancellation);

        return new PeriodDto { Id = entity.Id, Period = entity.Period };
    }

    public async Task<ErrorOr<bool>> DeleteAsync(int id, CancellationToken cancellation = default)
    {
        logger.LogInformation("Deleting period ID: {Id}", id);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var entity = await context.LtPeriods.FirstOrDefaultAsync(x => x.Id == id, cancellation);
        if (entity is null)
        {
            return Error.NotFound(description: $"Period with ID {id} was not found.");
        }

        context.LtPeriods.Remove(entity);
        await context.SaveChangesAsync(cancellation);

        return true;
    }
}

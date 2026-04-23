using Applications.Features.LoanPurposes;
using Applications.Interfaces;
using Applications.Responses.Paginations;
using Domains.Models;
using ErrorOr;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class LoanPurposeRepository(
    IDbContextFactory<DataContext> contextFactory,
    ILogger<LoanPurposeRepository> logger) : ILoanPurposeService
{
    public async Task<Pager<LoanPurposeDto>> GetAllAsync(QueryParam? param, CancellationToken cancellation = default)
    {
        logger.LogInformation("Fetching all loan purposes");

        param ??= new QueryParam();

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var query = context.LtPurposes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(param.Filter))
        {
            query = query.Where(x => x.Title != null && x.Title.Contains(param.Filter));
        }

        var count = await query.CountAsync(cancellation);

        if (param.PageSize > 0)
        {
            query = query.OrderBy(x => x.Title)
                         .Skip(param.Page)
                         .Take(param.PageSize);
        }

        var items = await query
            .AsNoTracking()
            .Select(x => new LoanPurposeDto { Id = x.Id, Title = x.Title })
            .ToListAsync(cancellation);

        return new Pager<LoanPurposeDto>(items, count);
    }

    public async Task<LoanPurposeDto?> GetByIdAsync(int id, CancellationToken cancellation = default)
    {
        logger.LogInformation("Fetching loan purpose with ID: {Id}", id);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var item = await context.LtPurposes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellation);

        return item is null ? null : new LoanPurposeDto { Id = item.Id, Title = item.Title };
    }

    public async Task<ErrorOr<LoanPurposeDto>> CreateAsync(LoanPurposeDtoCreate request, CancellationToken cancellation = default)
    {
        logger.LogInformation("Creating loan purpose: {Title}", request.Title);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var exists = await context.LtPurposes.AnyAsync(x => x.Title == request.Title, cancellation);

        if (exists)
        {
            return Error.Conflict(description: $"Loan purpose '{request.Title}' already exists.");
        }

        var entity = new LtPurpose { Title = request.Title };
        context.LtPurposes.Add(entity);
        await context.SaveChangesAsync(cancellation);

        return new LoanPurposeDto { Id = entity.Id, Title = entity.Title };
    }

    public async Task<ErrorOr<LoanPurposeDto>> UpdateAsync(LoanPurposeDtoUpdate request, CancellationToken cancellation = default)
    {
        logger.LogInformation("Updating loan purpose ID: {Id}", request.Id);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var entity = await context.LtPurposes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellation);
        if (entity is null)
        {
            return Error.NotFound(description: $"Loan purpose with ID {request.Id} was not found.");
        }

        var duplicate = await context.LtPurposes.AnyAsync(x => x.Title == request.Title && x.Id != request.Id, cancellation);
        if (duplicate)
        {
            return Error.Conflict(description: $"Loan purpose '{request.Title}' already exists.");
        }

        entity.Title = request.Title;
        await context.SaveChangesAsync(cancellation);

        return new LoanPurposeDto { Id = entity.Id, Title = entity.Title };
    }

    public async Task<ErrorOr<bool>> DeleteAsync(int id, CancellationToken cancellation = default)
    {
        logger.LogInformation("Deleting loan purpose ID: {Id}", id);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var entity = await context.LtPurposes.FirstOrDefaultAsync(x => x.Id == id, cancellation);
        if (entity is null)
        {
            return Error.NotFound(description: $"Loan purpose with ID {id} was not found.");
        }

        context.LtPurposes.Remove(entity);
        await context.SaveChangesAsync(cancellation);

        return true;
    }
}

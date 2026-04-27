using Applications.Features.AdditionalInformations.Dto;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public class SalutaionRepository(IDbContextFactory<DataContext> contextFactory,
    ILogger<SalutaionRepository> logger) : ISalutationService
{

    public async Task<SalutationDto?> AddOrUpdateAsync(Salutation salutation, CancellationToken ct = default)
    {
        try
        {
            logger.LogInformation("Adding or updating salutation with title: {Title}", salutation.Title);

            await using var context = await contextFactory.CreateDbContextAsync(ct);

            var sal = await context.Salutations.AnyAsync(x => x.Title == salutation.Title, ct);

            if (sal)
            {
                throw new Exception("Title already existing.");
            }

            if (salutation.Id > 0)
            {
                context.Salutations.Update(salutation);
            }
            else
            {
                context.Salutations.Add(salutation);
            }
            await context.SaveChangesAsync(ct);

            return new SalutationDto
            {
                Id = salutation.Id,
                Title = salutation.Title,
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while adding or updating salutation with title: {Title}",
                salutation.Title);
            throw new Exception(ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int? id, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("Deleting salutation with ID: {Id}", id);

            await using var context = await contextFactory.CreateDbContextAsync();

            if (id > 0)
            {
                var result = await context.Salutations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                context.Salutations.Remove(result!);

                await context.SaveChangesAsync();

                return Result.Success(true);
            }

            return Result.Failure(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting salutation with ID: {Id}", id);

            return Result.Failure(false);
        }
    }

    public async Task<Pager<SalutationDto>> GetsSalutions(int? id, QueryParam? param, CancellationToken ct = default)
    {
        logger.LogInformation("Retrieving salutations with id: {Id}", id);

        await using var context = await contextFactory.CreateDbContextAsync(ct);

        var baseQuery = context.Salutations.AsQueryable();

        if (id is > 0)
        {
            baseQuery = baseQuery.Where(x => x.Id == id);
        }

        if (param is null)
        {
            param = new QueryParam();
        }

        if (!string.IsNullOrWhiteSpace(param.Filter))
        {
            baseQuery = baseQuery.Where(x => EF.Functions.Like(x.Title, $"%{param.Filter}%"));
        }

        var count = await baseQuery.CountAsync(ct);

        var items = await baseQuery
            .AsNoTracking()
            .Select(ProjectTo())
            .ToListAsync(ct);

        return PagerExtensions.Create(items, count);
    }

    private static System.Linq.Expressions.Expression<Func<Salutation, SalutationDto>> ProjectTo()
    {
        return x => new SalutationDto
        {
            Id = x.Id,
            Title = x.Title,
        };
    }
}
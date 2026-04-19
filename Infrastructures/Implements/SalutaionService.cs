using Applications.Extensions;
using Applications.Features.AdditionalInformations.Dto;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class SalutaionService : ISalutationService
    {
        private readonly DataContext _context;
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly ILogger<SalutaionService> _logger;

        public SalutaionService(DataContext context, IDbContextFactory<DataContext> contextFactory,
            ILogger<SalutaionService> logger)
        {
            _context = context;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Result<string>> AddSalution(Salutation salutation)
        {
            try
            {
                _logger.LogInformation("Adding or updating salutation with title: {Title}", salutation.Title);

                await using var context = await _contextFactory.CreateDbContextAsync();

                if (string.IsNullOrWhiteSpace(salutation.Title))
                {
                    return Result.Failure<string>(ErrorResponse.Validation("Title field is required."));
                }

                var sal = await context.Salutations.AnyAsync(x => x.Title == salutation.Title);

                if (sal)
                {
                    return Result.Failure<string>(ErrorResponse.Conflict("Title already existing."));
                }

                if (salutation.Id > 0)
                {
                    context.Salutations.Update(salutation);

                    await context.SaveChangesAsync();

                    return Result.Success(new string("Title was updated."));
                }

                context.Salutations.Add(salutation);

                await context.SaveChangesAsync();

                return Result.Success(new string("Title was added."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding or updating salutation with title: {Title}",
                    salutation.Title);

                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<SalutationDto?> AddSalution(Salutation salutation, CancellationToken ct = default)
        {
            try
            {
                _logger.LogInformation("Adding or updating salutation with title: {Title}", salutation.Title);

                await using var context = await _contextFactory.CreateDbContextAsync(ct);

                var sal = await context.Salutations.AnyAsync(x => x.Title == salutation.Title,ct);

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
                    Id =  salutation.Id,
                    Title = salutation.Title,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding or updating salutation with title: {Title}",
                    salutation.Title);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Result<string>> DeleteSalution(int id)
        {
            try
            {
                _logger.LogInformation("Deleting salutation with ID: {Id}", id);

                await using var context = await _contextFactory.CreateDbContextAsync();

                if (id > 0)
                {
                    var result = await context.Salutations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                    context.Salutations.Remove(result!);

                    await context.SaveChangesAsync();

                    return Result.Success(new string("Title was deleted."));
                }

                return Result.Success(new string("Title not found."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting salutation with ID: {Id}", id);

                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<IEnumerable<Salutation>>> GetSalutions(string? q = null)
        {
            _logger.LogInformation("Retrieving salutations with query: {Query}", q);

            await using var context = await _contextFactory.CreateDbContextAsync();

            IQueryable<Salutation> queries = context.Salutations;

            if (!string.IsNullOrEmpty(q))
            {
                queries = queries.Where(x => EF.Functions.Like(x.Title!, $"%{q}%") ||
                                             EF.Functions.Like(
                                                 EF.Functions.Collate(x.Id.ToString(), "Khmer_100_CI_AI_SC_UTF8"),
                                                 $"%{q}%"));
            }

            var result = await queries.AsNoTracking().ToListAsync();

            return Result.Success<IEnumerable<Salutation>>(result);
        }

        public async Task<Result<IEnumerable<string>>> GetsSalutions(string? q = null)
        {
            _logger.LogInformation("Retrieving salutations with query: {Query}", q);

            await using var context = await _contextFactory.CreateDbContextAsync();

            var salutations = await context.Salutations
                .WhereIf(!string.IsNullOrEmpty(q), x => x.Title!.Contains(q!))
                .AsNoTracking()
                .Select(x => x.Title)
                .ToListAsync();
            if (!salutations.Any())
            {
                return Result.Success<IEnumerable<string>>(Enumerable.Empty<string>());
            }

            return Result.Success<IEnumerable<string>>(salutations!);
        }


        public async Task<Pager<SalutationDto>> GetsSalutions(int? id, QueryParam? param,
            CancellationToken ct = default)
        {
            _logger.LogInformation("Retrieving salutations with id: {Id}", id);

            await using var context = await _contextFactory.CreateDbContextAsync(ct);

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
                .Select(x => new SalutationDto
                {
                    Id = x.Id,
                    Title = x.Title,
                })
                .ToListAsync(ct);

            return PagerExtensions.Create(items, count);
        }
    }
}
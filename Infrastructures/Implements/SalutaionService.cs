using Applications.Extensions;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class SalutaionService : ISalutationService
    {
        private readonly ApplicationDbContext _context;
        public SalutaionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<string>> AddSalution(Salutation salutation)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(salutation.Title))
                {
                    return Result.Failure<string>(ErrorResponse.Validation("Title field is required."));
                }
                var sal = await _context.Salutations.AnyAsync(x => x.Title == salutation.Title);
                if (sal)
                {
                    return Result.Failure<string>(ErrorResponse.Conflict("Title already existing."));
                }
                if (salutation.Id > 0)
                {
                    _context.Salutations.Update(salutation);
                    await _context.SaveChangesAsync();
                    return Result.Success(new string("Title was updated."));
                }
                _context.Salutations.Add(salutation);
                await _context.SaveChangesAsync();
                return Result.Success(new string("Title was added."));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> DeleteSalution(int id)
        {
            try
            {

                if (id > 0)
                {
                    var result = await _context.Salutations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                    _context.Salutations.Remove(result!);
                    await _context.SaveChangesAsync();
                    return Result.Success(new string("Title was deleted."));
                }
                return Result.Success(new string("Title not found."));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<IEnumerable<Salutation>>> GetSalutions(string? q = null)
        {
            IQueryable<Salutation> queries = _context.Salutations;

            if (!string.IsNullOrEmpty(q))
            {
                queries = queries.Where(x => EF.Functions.Like(x.Title!, $"%{q}%") ||
                                             EF.Functions.Like(EF.Functions.Collate(x.Id.ToString(), "Khmer_100_CI_AI_SC_UTF8"), $"%{q}%"));
            }
            var result = await queries.AsNoTracking().ToListAsync();

            return Result.Success<IEnumerable<Salutation>>(result);

        }

        public async Task<Result<IEnumerable<string>>> GetsSalutions(string? q = null)
        {
            var salutations = await _context.Salutations
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


    }
}

using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructures.Implements
{
    public class OccupationService : IOccupationService
    {

        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly ILogger<OccupationService> _logger;
        public OccupationService(IDbContextFactory<DataContext> contextFactory, ILogger<OccupationService> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Occupation?> GetOccupationByIdAsync(int? id)
        {
            _logger.LogInformation("Fetching occupation with ID: {Id}", id);

            await using var _context = await _contextFactory.CreateDbContextAsync();

            var occ = await _context.Occupations.FirstOrDefaultAsync(x => x.OccupationId == id);
            return occ;
        }

        public async Task<Pager<T>> GetOccupations<T>(int? id, string? term = null)
        {
            _logger.LogInformation("Fetching occupations with ID: {Id} and term: {Term}", id, term);

            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<Occupation> queries = _context.Occupations;

            if (id.HasValue && id > 0)
            {
                queries = queries.Where(o => o.OccupationId != id);
            }

            if (!string.IsNullOrEmpty(term))
            {
                queries = queries.Where(x => x.OccupationTitle != null && x.OccupationTitle.ToLower().StartsWith(term)
                                            || x.Khmer != null && x.Khmer.ToLower().StartsWith(term)
                                            || x.TypeOf!.ToLower().Contains(term));
            }

            int counts = queries.Count();

            var items = await queries
                                   .AsNoTracking()
                                   .ProjectToType<T>()
                                   .ToListAsync();

            return new Pager<T>(items, counts);
        }

        public async Task<IEnumerable<Occupation>> GetOccupationsAsync(string? expression = null)
        {
            _logger.LogInformation("Fetching occupations with expression: {Expression}", expression);

            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<Occupation> occupations = _context.Occupations;

            if (expression is not null)
            {
                occupations = occupations.Where(x => x.OccupationTitle != null && x.OccupationTitle.ToLower().StartsWith(expression)
                                            || x.Khmer != null && x.Khmer.ToLower().StartsWith(expression)
                                            || x.TypeOf!.ToLower().Contains(expression));
            }

            return await occupations.AsNoTracking().ToListAsync() ?? Enumerable.Empty<Occupation>();

        }
    }
}

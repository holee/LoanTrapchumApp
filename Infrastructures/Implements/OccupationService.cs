using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Implements
{
    public class OccupationService : IOccupationService
    {

        private readonly ApplicationDbContext _context;

        public OccupationService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Occupation?> GetOccupationByIdAsync(int? id)
        {
            var occ = await _context.Occupations.FirstOrDefaultAsync(x => x.OccupationId == id);
            return occ;
        }

        public async Task<Pager<T>> GetOccupations<T>(int? id, string? term = null)
        {
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

using Applications.Interfaces.Lists;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class OccupationService: IOccupationService
    {

        private readonly ApplicationDbContext _context;

        public OccupationService(ApplicationDbContext context)
        {
            this._context = context;
        }
 
        public async Task<Occupation?> FindOccupationAsync(int id)
        {
            var occ = await _context.Occupations.FirstOrDefaultAsync(x => x.OccupationId == id);
            return occ;

        }

        public async Task<IEnumerable<Occupation>> FindAllOccupationsAsync(string? expression = null)
        {
            IQueryable<Occupation> occupations = _context.Occupations;

            if (expression is not null)
            {
                occupations = occupations.Where(x => x.OccupationTitle !=null && x.OccupationTitle.ToLower().StartsWith(expression) 
                                            || x.Khmer !=null && x.Khmer.ToLower().StartsWith(expression)
                                            || x.TypeOf!.ToLower().Contains(expression));
            }

            return await occupations.AsNoTracking().ToListAsync() ?? Enumerable.Empty<Occupation>();

        }
    }
}

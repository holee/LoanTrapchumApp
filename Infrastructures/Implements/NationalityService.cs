using Applications.Interfaces.Lists;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class NationalityService: INationalityService
    {
        private readonly ApplicationDbContext _context;

        public NationalityService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Nationality>> GetNationalities(string? searchText=null)
        {
            IQueryable<Nationality> queries = _context.Nationalities;
            if (!string.IsNullOrEmpty(searchText))
            {
                queries=queries.Where(x=>x.NationalityName !=null && x.NationalityName.Contains(searchText,StringComparison.InvariantCultureIgnoreCase) ||
                                          x.NationalityCode != null && x.NationalityCode.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));   
            }

            return await queries.AsNoTracking().ToListAsync();  
        }
        public async Task<IEnumerable<Nationality>> GetNationalities(int id=12) 
        {
            IQueryable<Nationality> queries = _context.Nationalities;
            if (id > 0)
            {
                queries = queries.Where(x => x.NationalityId==id);
            }

            return await queries.AsNoTracking().ToListAsync();
        }
    }
}

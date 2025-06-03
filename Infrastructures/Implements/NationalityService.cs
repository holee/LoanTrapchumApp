using Applications.Interfaces.Lists;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class NationalityService : INationalityService
    {
        private readonly DataContext _context;
        private readonly ILogger<NationalityService> _logger;
        private readonly IDbContextFactory<DataContext> _contextFactory;
        public NationalityService(DataContext context, IDbContextFactory<DataContext> contextFactory, ILogger<NationalityService> logger)
        {
            this._context = context;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<Nationality>> GetNationalities(string? searchText = null)
        {

            _logger.LogInformation("Fetching nationalities with search text: {SearchText}", searchText);

            await using var context = await _contextFactory.CreateDbContextAsync();


            IQueryable<Nationality> queries = context.Nationalities;

            if (!string.IsNullOrEmpty(searchText))
            {
                queries = queries.Where(x => x.NationalityName != null && x.NationalityName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                                          x.NationalityCode != null && x.NationalityCode.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
            }

            return await queries.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Nationality>> GetNationalities(int id = 12)
        {
            _logger.LogInformation("Fetching nationalities with ID: {Id}", id);

            await using var context = await _contextFactory.CreateDbContextAsync();


            IQueryable<Nationality> queries = context.Nationalities;
            if (id > 0)
            {
                queries = queries.Where(x => x.NationalityId == id);
            }

            return await queries.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetNationalities<T>(int? id, string? searchText = null)
            where T : class
        {
            _logger.LogInformation("Fetching nationalities with ID: {Id} and search text: {SearchText}", id, searchText);

            await using var context = await _contextFactory.CreateDbContextAsync();

            IQueryable<Nationality> queries = context.Nationalities;

            if (id > 0)
            {
                queries = queries.Where(x => x.NationalityId == id);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                queries = queries.Where(n => EF.Functions.Like(n.NationalityName, $"%{searchText}%") ||
                                         EF.Functions.Like(n.NationalityCode, $"%{searchText}%"));
            }

            var items = await queries
                                    .ProjectToType<T>()
                                    .AsNoTracking().ToListAsync();


            return items;
        }
    }
}

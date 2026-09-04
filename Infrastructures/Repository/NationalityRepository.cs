using Applications.Features.AdditionalInformations.Dto;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Repository
{
    public sealed class NationalityRepository : INationalityService
    {
        private readonly ILogger<NationalityRepository> _logger;

        private readonly DataContext _contextFactory;
        
        public NationalityRepository(DataContext contextFactory, ILogger<NationalityRepository> logger)
        {
            _contextFactory = contextFactory;

            _logger = logger;
        }

        public async Task<IEnumerable<Nationality>> GetNationalities(string? searchText = null)
        {

            _logger.LogInformation("Fetching nationalities with search text: {SearchText}", searchText);

            var context = _contextFactory;


            IQueryable<Nationality> queries = context.Nationalities;

            if (!string.IsNullOrEmpty(searchText))
            {
                queries = queries.Where(x => x.NationalityName != null && x.NationalityName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                                          x.NationalityCode != null && x.NationalityCode.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
            }

            return await queries.AsNoTracking().ToListAsync();
        }


        public async Task<Pager<NationalityDto>> GetNationalities(int? id, QueryParam param, CancellationToken cancellation = default)
        {

            _logger.LogInformation("Fetching nationalities with ID: {Id} and query parameters: {QueryParam}", id, param);

            var context = _contextFactory;


            IQueryable<Nationality> queries = context.Nationalities;

            if (id.HasValue && id.Value > 0)
            {
                queries = queries.Where(x => x.NationalityId == id.Value);
            }

            if (!string.IsNullOrEmpty(param.Filter))
            {
                queries = queries.Where(x => EF.Functions.Like(x.NationalityName, $"%{param.Filter}%") ||
                                             EF.Functions.Like(x.NationalityCode, $"%{param.Filter}%"));
            }

            var counts = await queries.CountAsync(cancellation);

            var items = await queries
                                    .AsNoTracking()
                                    .Select(x => new NationalityDto
                                    {
                                        NationalityId = x.NationalityId,
                                        NationalityName = x.NationalityName,
                                        NationalityCode = x.NationalityCode
                                    })
                                    .ToListAsync(cancellation);


            return PagerExtensions.Create(items, counts);

        }



    
    }
}

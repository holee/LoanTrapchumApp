using Applications.Commons.Mappers;
using Applications.Extensions;
using Applications.Extensions.mappers;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels;
using Domains.Models.Lists;
using Domains.Shared.Enums;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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

        public async Task<Result<OccupationVm>> CreateAsync(Occupation occupation, CancellationToken cancellation = default)
        {

            _logger.LogInformation("Creating a new occupation: {OccupationTitle}", occupation.OccupationTitle);

            if (await IsOccupationExist(occupation.OccupationTitle, cancellation))
            {
                _logger.LogWarning("Occupation with title {OccupationTitle} already exists.", occupation.OccupationTitle);

                return Result.Failure<OccupationVm>(ErrorResponse.Conflict("Occupation with title already exists"));
            }

            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            context.Occupations.Add(occupation);

            await context.SaveChangesAsync();

            var created = occupation.Map();

            return Result.Success(created);
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
                queries = queries.Where(x => EF.Functions.Like(x.OccupationTitle!, $"%{term}%") ||
                                             EF.Functions.Like(x.Khmer, $"%{term}%") ||
                                             EF.Functions.Like(x.TypeOf, $"%{term}%"));
            }

            int counts = queries.Count();

            var items = await queries
                                   .AsNoTracking()
                                   .ProjectToType<T>()
                                   .ToListAsync();

            return new Pager<T>(items, counts);
        }


        public async Task<Pager<T>> GetOccupations<T>(int? id, TypeOfEnum ofOption, string? term = null, CancellationToken cancellation = default)
        {
            _logger.LogInformation("Fetching occupations with ID: {Id} and term: {Term}", id, term);

            await using var _context = await _contextFactory.CreateDbContextAsync(cancellation);

            var queries = ofOption switch
            {
                TypeOfEnum.Position => _context.Occupations.Where(o => o.TypeOf == TypeOfEnum.Position.ToString()),
                TypeOfEnum.Job => _context.Occupations.Where(o => o.TypeOf == TypeOfEnum.Job.ToString()),
                _ => _context.Occupations.AsNoTracking()
            };


            if (id.HasValue && id > 0)
            {
                queries = queries.Where(o => o.OccupationId != id);
            }

            if (!string.IsNullOrEmpty(term))
            {
                queries = queries.Where(x => EF.Functions.Like(x.OccupationTitle!, $"%{term}%") ||
                                             EF.Functions.Like(x.Khmer, $"%{term}%") ||
                                             EF.Functions.Like(x.TypeOf, $"%{term}%"));
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


        public async Task<Pager<OccupationVm>> GetOccupationsAsync(int? id, QueryParam param, CancellationToken cancellation = default)
        {
            _logger.LogInformation("Fetching occupations with expression");

            await using var _context = await _contextFactory.CreateDbContextAsync(cancellation);

            var queries = _context.Occupations
                                  .WhereIf(!string.IsNullOrEmpty(param.Filter),
                                           x => EF.Functions.Like(x.OccupationTitle, $"%{param.Filter}%") ||
                                                EF.Functions.Like(x.Khmer, $"%{param.Filter}%") ||
                                                EF.Functions.Like(x.TypeOf!, $"%{param.Filter}%"));
            if (id.HasValue && id > 0)
            {
                queries = queries.Where(x => x.OccupationId == id);
            }

            if (param.SortDescending)
            {
                queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
            }


            var counts = await queries.CountAsync(cancellation);

            if (param.PageSize > 0)
            {

                queries = queries.Skip(param.Page).Take(param.PageSize);
            }

            var items = await queries.AsNoTracking()
                                    .ProjectToType<OccupationVm>()
                                    .ToListAsync();

            return new Pager<OccupationVm>(items, counts);

        }





        private async Task<bool> IsOccupationExist(string occupationTitle, CancellationToken cancellation)
        {
            _logger.LogInformation("Checking if occupation with title {OccupationTitle} exists", occupationTitle);

            await using var _context = await _contextFactory.CreateDbContextAsync(cancellation);

            return await _context.Occupations
                                 .AsNoTracking()
                                 .AnyAsync(x => x.OccupationTitle!.ToLower() == occupationTitle.ToLower(), cancellation);
        }

    }
}

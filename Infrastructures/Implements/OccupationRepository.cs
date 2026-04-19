using Applications.Commons.Mappers;
using Applications.Extensions;
using Applications.Features.AdditionalInformations.Dto;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Domains.Shared.Enums;
using ErrorOr;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class OccupationRepository(IDbContextFactory<DataContext> contextFactory, ILogger<OccupationRepository> logger) : IOccupationService
    {
        public async Task<ErrorOr<OccupationDto>> CreateAsync(Occupation occupation, CancellationToken cancellation = default)
        {

            logger.LogInformation("Creating a new occupation: {OccupationTitle}", occupation.OccupationTitle);

            if (await IsOccupationExist(occupation.OccupationTitle, cancellation))
            {
                logger.LogWarning("Occupation with title {OccupationTitle} already exists.", occupation.OccupationTitle);

                return Error.Conflict(description: "Occupation with title already exists");
            }

            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            context.Occupations.Add(occupation);

            await context.SaveChangesAsync();

            var created = occupation.Map();

            return created;
        }



        public async Task<Occupation?> GetOccupationByIdAsync(int? id, CancellationToken cancellation = default)
        {
            logger.LogInformation("Fetching occupation with ID: {Id}", id);

            await using var _context = await contextFactory.CreateDbContextAsync(cancellation);

            var occ = await _context.Occupations.FirstOrDefaultAsync(x => x.OccupationId == id, cancellation);

            return occ;
        }



        public async Task<IEnumerable<Occupation>> GetOccupationsAsync(string? expression = null)
        {
            logger.LogInformation("Fetching occupations with expression: {Expression}", expression);

            await using var _context = await contextFactory.CreateDbContextAsync();

            IQueryable<Occupation> occupations = _context.Occupations;

            if (expression is not null)
            {
                occupations = occupations.Where(x => x.OccupationTitle != null && x.OccupationTitle.ToLower().StartsWith(expression)
                                            || x.Khmer != null && x.Khmer.ToLower().StartsWith(expression)
                                            || x.TypeOf!.ToLower().Contains(expression));
            }

            return await occupations.AsNoTracking().ToListAsync() ?? Enumerable.Empty<Occupation>();

        }





        public async Task<Pager<OccupationDto>> GetOccupationsAsync(int? id, QueryParam? param,
            TypeOfEnum ofOption = TypeOfEnum.Position,
            CancellationToken cancellation = default)
        {
            logger.LogInformation("Fetching occupations with expression");

            await using var _context = await contextFactory.CreateDbContextAsync(cancellation);

            var baseQueries = _context.Occupations.AsQueryable();

            if (param is null)
            {
                param = new QueryParam();
            }

            var queries = ofOption switch
            {
                TypeOfEnum.Position => baseQueries.Where(o => o.TypeOf == TypeOfEnum.Position.ToString()),

                TypeOfEnum.Job => baseQueries.Where(o => o.TypeOf == TypeOfEnum.Job.ToString()),

                _ => baseQueries
            };

            queries = _context.Occupations.WhereIf(!string.IsNullOrEmpty(param.Filter),
                                           x => EF.Functions.Like(x.OccupationTitle, $"%{param.Filter}%") ||
                                                EF.Functions.Like(x.Khmer, $"%{param.Filter}%") ||
                                                EF.Functions.Like(x.TypeOf!, $"%{param.Filter}%"));
            if (id.HasValue && id > 0)
            {
                queries = queries.Where(x => x.OccupationId == id);
            }

            if (param.SortDescending && !string.IsNullOrEmpty(param.SortColumn))
            {
                queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn!));
            }


            var counts = await queries.CountAsync(cancellation);

            if (param.PageSize > 0)
            {

                queries = queries.Skip(param.Page).Take(param.PageSize);
            }

            var items = await queries
                                    .AsNoTracking()
                                    .Select(x => new OccupationDto
                                    {
                                        OccupationId = x.OccupationId,
                                        OccupationTitle = x.OccupationTitle!,
                                        Khmer = x.Khmer,
                                        TypeOf = x.TypeOf
                                    }).ToListAsync(cancellation);

            return new Pager<OccupationDto>(items, counts);

        }





        private async Task<bool> IsOccupationExist(string occupationTitle, CancellationToken cancellation)
        {
            logger.LogInformation("Checking if occupation with title {OccupationTitle} exists", occupationTitle);

            await using var _context = await contextFactory.CreateDbContextAsync(cancellation);

            return await _context.Occupations
                                 .AsNoTracking()
                                 .AnyAsync(x => x.OccupationTitle!.ToLower() == occupationTitle.ToLower(), cancellation);
        }

    }
}

//public async Task<Pager<T>> GetOccupations<T>(int? id, string? term = null, CancellationToken cancellation = default)
//{
//    logger.LogInformation("Fetching occupations with ID: {Id} and term: {Term}", id, term);

//    await using var _context = await contextFactory.CreateDbContextAsync(cancellation);

//    var queries = ofOption switch
//    {
//        TypeOfEnum.Position => _context.Occupations.Where(o => o.TypeOf == TypeOfEnum.Position.ToString()),

//        TypeOfEnum.Job => _context.Occupations.Where(o => o.TypeOf == TypeOfEnum.Job.ToString()),

//        _ => _context.Occupations.AsNoTracking()
//    };


//    if (id.HasValue && id > 0)
//    {
//        queries = queries.Where(o => o.OccupationId != id);
//    }

//    if (!string.IsNullOrEmpty(term))
//    {
//        queries = queries.Where(x => EF.Functions.Like(x.OccupationTitle!, $"%{term}%") ||
//                                     EF.Functions.Like(x.Khmer, $"%{term}%") ||
//                                     EF.Functions.Like(x.TypeOf, $"%{term}%"));
//    }

//    int counts = queries.Count();

//    var items = await queries
//                           .AsNoTracking()
//                           .ProjectToType<T>()
//                           .ToListAsync();

//    return new Pager<T>(items, counts);
//}


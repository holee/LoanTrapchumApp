using Applications.Extensions;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class HolidayService : IHolidayService
    {
        private readonly ApplicationDbContext _context;

        private IEnumerable<DateTime> _xistingHolidays = [];

        private readonly IDbContextFactory<DataContext> _contextFactory;

        public HolidayService(ApplicationDbContext context, IDbContextFactory<DataContext> contextFactory)
        {
            _context = context;

            _contextFactory = contextFactory;
        }

        public async Task<Result<string>> CreateHoliday(Holiday entities)
        {
            try
            {

                if (entities.Id <= 0)
                {
                    _context.Holidays.Add(entities);

                    await _context.SaveChangesAsync();

                    return Result.Success<string>(new("New holiday was added successfully!"));
                }
                else
                {
                    var emp = _context.Holidays.AsNoTracking()
                    .Where(x => x.Id == entities.Id && x.HolidayDt.Month == entities.HolidayDt.Month)
                    .FirstOrDefault();

                    if (emp is null)
                    {
                        return Result.Failure<string>(ErrorResponse.NotFound("Something wrong, cannot update holiday!"));
                    }
                    _context.Holidays.Update(entities);
                    await _context.SaveChangesAsync();

                    return Result.Success<string>(new("Holiday was updated successfully!"));
                }

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Holiday?> GetHoliday(long id)
        {

            var result = await _context.Holidays.Where(x => x.Id == id).FirstOrDefaultAsync();

            return result ?? default;
        }


        public async Task<IEnumerable<Holiday>> GetHolidays(DateTime selectedDate)
        {
            IQueryable<Holiday> queries = _context.Holidays
                                                 .Where(x => x.HolidayDt.Month == selectedDate.Month &&
                                                        x.HolidayDt.Year == selectedDate.Year);
            return await queries.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious,DateTime? startFrom = null, DateTime? end = null)
        {
            IQueryable<Holiday> queries = _context.Holidays;
            if (isSelectedPrevious && startFrom is not null)
            {
                queries = queries.Where(d => d.HolidayDt >= startFrom);
            }
            if (!isSelectedPrevious && startFrom is not null)
            {
                queries = queries.Where(d => d.HolidayDt >= startFrom && d.HolidayDt <= end);
            }
            if (!isSelectedPrevious && startFrom is null)
            {
                queries = queries.Where(d => d.HolidayDt >= DateTime.Now && d.HolidayDt <= end);
            }
            return await queries.AsNoTracking().ToListAsync();
        }

        public async Task<Pager<Holiday>> GetHolidays(int pageNumber, int pageSize, string sortColumn = "HolidayName",
            bool isSelectedPrevious = false,
            bool sortDescending = false,
            string? q = null)
        {
            IQueryable<Holiday> queries = _context.Holidays;
            if (!isSelectedPrevious)
            {
                queries = queries.Where(d => d.HolidayDt >= DateTime.Now);
            }

            if (q != null)
            {
                queries = queries.Where(x => EF.Functions.Like(x.Description, $"%{q}%") ||
                                             EF.Functions.Like(x.HolidayDt.ToString(), $"%{q}%"));
            }

            if (sortDescending)
            {
                queries = queries.OrderByDescending(x => EF.Property<object>(x, sortColumn));
            }

            var counts = queries.Count();
            queries = queries.Skip(pageNumber).Take(pageSize);
            return new Pager<Holiday>
            {
                Count = counts,
                Items = await queries.AsNoTracking().ToListAsync()
            };
        }

        public async Task<Pager<Holiday>> GetHolidays(QueryParam q)
        {

            await using var context = await _contextFactory.CreateDbContextAsync(); 


            var queries = context.Holidays
                                  .WhereIf(!q.IsIncludeInActive, d => d.HolidayDt >= DateTime.Now)
                                  .WhereIf(!string.IsNullOrEmpty(q.Filter),
                                            x=> EF.Functions.Like(x.Description, $"%{q}%") ||
                                                EF.Functions.Like(x.HolidayNum.ToString(), $"%{q}%") ||
                                                EF.Functions.Like(x.HolidayDt.ToString(), $"%{q}%"));

            if (q.SortDescending)
            {
                queries = queries.OrderByDescending(x => EF.Property<object>(x, q.SortColumn));
            }

            var counts =await queries.CountAsync();

            if(q.PageSize > 0)
            {
                queries = queries.Skip(q.PageNumber).Take(q.PageSize);

            }

            return new Pager<Holiday>
            {
                Count = counts,

                Items = await queries.AsNoTracking().ToListAsync()
            };
        }


        public async Task<IEnumerable<Holiday>> GetActiveHolidays(DateTime? selectedDate) 
        {

            await using var context = await _contextFactory.CreateDbContextAsync();

            var queries =await context.Holidays
                                  .Where(d => d.HolidayDt >= selectedDate)
                                  .AsNoTracking()
                                  .ToListAsync();

            return  queries ?? [];
        }



        public async Task<bool> Existdate(DateTime setdate)
        {
            var query = await _context.Holidays.Where(x => x.HolidayDt == setdate)
                                .ToListAsync();
            return query.Count() > 0;
        }
        public async Task<Result<string>> ActivateOrDeactivate(int id, Holiday req)
        {
            try
            {
                var data = await _context.Holidays.Where(x => x.Id != id && x.Status == req.Status).FirstOrDefaultAsync();

                if (data is not null)
                    return Result.Failure<string>(ErrorResponse.Conflict("Holiday status already exist!"));

                _context.Holidays.Update(req);

                await _context.SaveChangesAsync();

                return Result.Success<string>(new("Holiday status was updated successfully!"));

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }

        }
        public async Task<IEnumerable<Holiday>> GetDate(DateTime q)
        {
            IQueryable<Holiday> respone = _context.Holidays
                                                .Where(x => x.HolidayDt.Month == q.Month &&
                                                        x.HolidayDt.Year == q.Year);
            return await respone.AsNoTracking().ToListAsync() ?? Enumerable.Empty<Holiday>();
        }
        public async Task<Result<Holiday>> GetByDate(DateTime? dt)
        {
            try
            {
                var data = await _context.Holidays.Where(x => x.HolidayDt == dt).FirstOrDefaultAsync();

                if (data is null)
                    return Result.Failure<Holiday>(ErrorResponse.NotFound("No data found"));
                return Result.Success(data);
            }
            catch (Exception ex)
            {
                return Result.Failure<Holiday>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }


        public async Task<Result<string>> BulkCreateHoliday(List<Holiday> enity)
        {
            try
            {
                _context.Holidays.AddRange(enity);
                await _context.SaveChangesAsync();
                return Result.Success<string>(new("New holiday was added successfully!"));

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }


        public async Task<Result<string>> BulkDeleteHoliday(List<long> id)
        {
            try
            {
                foreach (var i in id)
                {
                    var result = await _context.Holidays.AsNoTracking().FirstOrDefaultAsync(x => x.Id == i);

                    _context.Holidays.Remove(result!);

                }
                await _context.SaveChangesAsync();
                return Result.Success<string>(new("Holiday was deleted successfully!"));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> DeleteHoliday(long id)
        {
            try
            {

                var result = await _context.Holidays.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                _context.Holidays.Remove(result!);
                await _context.SaveChangesAsync();
                return Result.Success<string>(new("Holiday was deleted successfully!"));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
    }
}


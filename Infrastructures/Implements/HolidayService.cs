using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreBussiness.Implements
{
    public class HolidayService : IHolidayService
    {
        private readonly ApplicationDbContext _context;
        private IEnumerable<DateTime> _xistingHolidays = [];

        public HolidayService(ApplicationDbContext context)
        {
            this._context = context;
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
                    .Where(x => x.Id == entities.Id && x.HolidayName.Month == entities.HolidayName.Month)
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
                                                 .Where(x => x.HolidayName.Month == selectedDate.Month &&
                                                        x.HolidayName.Year == selectedDate.Year);
            return await queries.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious,
            DateTime? startFrom = null, DateTime? end = null)
        {
            IQueryable<Holiday> queries = _context.Holidays;
            if (isSelectedPrevious && startFrom is not null)
            {
                queries = queries.Where(d => d.HolidayName >= startFrom);
            }
            if (!isSelectedPrevious && startFrom is not null)
            {
                queries = queries.Where(d => d.HolidayName >= startFrom && d.HolidayName <= end);
            }
            if (!isSelectedPrevious && startFrom is null)
            {
                queries = queries.Where(d => d.HolidayName >= DateTime.Now && d.HolidayName <= end);
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
                queries = queries.Where(d => d.HolidayName >= DateTime.Now);
            }

            if (q != null)
            {
                queries = queries.Where(x => EF.Functions.Like(x.Description, $"%{q}%") ||
                                             EF.Functions.Like(x.HolidayName.ToString(), $"%{q}%"));
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
        public async Task<bool> Existdate(DateTime setdate)
        {
            var query = await _context.Holidays.Where(x => x.HolidayName == setdate)
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
                                                .Where(x => x.HolidayName.Month == q.Month &&
                                                        x.HolidayName.Year == q.Year);
            return await respone.AsNoTracking().ToListAsync() ?? Enumerable.Empty<Holiday>();
        }
        public async Task<Result<Holiday>> GetByDate(DateTime? dt)
        {
            try
            {
                var data = await _context.Holidays.Where(x => x.HolidayName == dt).FirstOrDefaultAsync();

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
                return Result.Failure<string>(ErrorResponse.Failure("500", [$"{ex.InnerException!.Message}"]));
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
                return Result.Failure<string>(ErrorResponse.Failure("500", [$"{ex.InnerException!.Message}"]));
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
                return Result.Failure<string>(ErrorResponse.Failure("500", [$"{ex.InnerException!.Message}"]));
            }
        }
    }
}


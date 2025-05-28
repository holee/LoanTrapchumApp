using Applications.Extensions;
using Applications.Extensions.DateTimes;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Holidays;
using Dapper;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class HolidayService : IHolidayService
    {
        private readonly DataContext _context;

        private IEnumerable<DateTime> _xistingHolidays = [];

        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly IDapperConnectionFactory _connectionFactory;
        public HolidayService(DataContext context, IDbContextFactory<DataContext> contextFactory, IDapperConnectionFactory connectionFactory)
        {
            _context = context;

            _contextFactory = contextFactory;
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<string>> CreateOrUpdateHoliday(Holiday entities)
        {
            try
            {

                if (entities.Id <= 0)
                {
                    _context.Holidays.Add(entities);

                }
                else
                {
                    var emp = await _context.Holidays
                                        .Where(x => x.Id == entities.Id &&
                                                    x.HolidayNum == x.HolidayNum)
                                        .FirstOrDefaultAsync();

                    if (emp is null)
                    {
                        return Result.Failure<string>(ErrorResponse.NotFound("Something wrong, cannot update holiday!"));
                    }

                    emp.Id = entities.Id;
                    emp.HolidayName = entities.HolidayName;
                    emp.HolidayNum = entities.HolidayNum;
                    emp.HolidayDt = entities.HolidayDt;
                    emp.Status = entities.Status;
                    emp.Description = entities.Description;
                }

                await _context.SaveChangesAsync();

                return Result.Success<string>(new("Holiday was updated successfully!"));

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }


        public async Task<Result<IEnumerable<HolidayVm>>> CreateHolidays(IEnumerable<Holiday> entities)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();

                ICollection<Holiday> addedEntitities = [];

                foreach (var entity in entities)
                {

                    if (await ExistHolidays(entity.HolidayDt)) continue;

                    addedEntitities.Add(entity);

                }

                if (!addedEntitities.Any())
                {
                    return Result.Failure<IEnumerable<HolidayVm>>(ErrorResponse.NotFound("No new holidays were added!"));
                }


                _context.Holidays.AddRange(addedEntitities);

                await _context.SaveChangesAsync();

                var mapper = addedEntitities.ToHolidays();

                return Result.Success(mapper);

            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<HolidayVm>>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }



        public async Task<Result<Holiday>> GetHoliday(long id)
        {

            var result = await _context.Holidays.FindAsync(id);

            if (result is null)
            {
                return Result.Failure<Holiday>(ErrorResponse.NotFound("No data found"));
            }
            return Result.Success(result);
        }


        public async Task<IEnumerable<HolidayVm>> GetHolidays(DateTime? selectedDate)
        {

            var value = selectedDate ?? DateTime.Now;

            var month = value.Month;

            var year = value.Year;

            var queries = await _context.Database
                                        .SqlQuery<HolidayVm>(@$"SELECT h.* FROM lt_holidays  h 
                                                        WHERE (h.HolidayDt IS NOT NULL) AND
                                                        DATEPART(YEAR,h.HolidayDt) = {year} AND 
                                                        DATEPART(MONTH,h.HolidayDt) = {month}")
                                        .OrderByDescending(x => x.HolidayDt)
                                        .AsNoTracking()
                                        .ToListAsync();
            return queries;
        }




        public async Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious, DateTime? startFrom = null, DateTime? end = null)
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

        public async Task<Pager<HolidayVm>> GetHolidays(QueryParam q)
        {

            await using var context = await _contextFactory.CreateDbContextAsync();


            var queries = context.Database
                                  .SqlQuery<HolidayVm>($"SELECT * FROM VHolidaysVm")
                                  .WhereIf(!q.IsIncludeInActive, d => d.HolidayDt >= DateTime.Now)
                                  .WhereIf(!string.IsNullOrEmpty(q.Filter),
                                            x => EF.Functions.Like(x.Description, $"%{q}%") ||
                                                EF.Functions.Like(x.HolidayNum.ToString(), $"%{q}%") ||
                                                EF.Functions.Like(x.HolidayDt.ToString(), $"%{q}%"));

            if (q.SortDescending)
            {
                queries = queries.OrderByDescending(x => EF.Property<object>(x, q.SortColumn));
            }
            else
            {
                queries = queries.OrderBy(x => x.HolidayDt);
            }

            var counts = await queries.CountAsync();

            if (q.PageSize > 0)
            {
                queries = queries
                                .Skip(q.Skip)
                                .Take(q.PageSize);

            }

            //var builder = new SqlBuilder();

            var items = await queries.AsNoTracking().ToListAsync();

            return new Pager<HolidayVm>
            {
                Count = counts,

                Items = items
            };
        }


        public async Task<IEnumerable<HolidayVm>> GetActiveHolidays(DateTime? selectedDate,int? selectedYear=0)
        {

            using var db = _connectionFactory.CreateConnection();

            var lastSelectedDate = selectedYear > 0 ? selectedDate!.Value.AddYears(selectedYear!.Value) : selectedDate;

            var parameters = new DynamicParameters();

            parameters.Add("@selectedDate", selectedDate);

            parameters.Add("@lastSelectedDate", lastSelectedDate);

            var sql = @"SELECT Id, HolidayDt, HolidayName, HolidayName, HolidayNum, Status, Description
                        FROM VHolidaysVm WHERE Status = CAST(1 AS BIT) AND 
                        CAST(HolidayDt AS DATE) BETWEEN CAST(@selectedDate AS DATE) AND 
                        CAST(@lastSelectedDate AS DATE);
                       ";
            var queries=await db.QueryAsync<HolidayVm>(sql, parameters);

            return queries ?? [];
        }



        public async Task<bool> ExistHolidays(DateTime? setdate)
        {
            var query = await _context
                                .Holidays.Where(x => x.HolidayNum == setdate.DayNum())
                                .AsNoTracking()
                                .ToListAsync();
            return query.Any();
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
                                                .Where(x => x.HolidayDt!.Value.Month == q.Month &&
                                                            x.HolidayDt.Value.Year == q.Year);
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


using Applications.Bases;
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
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public sealed class HolidayRepository : IHolidayService
    {
        private readonly DataContext _context;

        private IEnumerable<DateTime> _xistingHolidays = [];

        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly IDapperConnectionFactory _connectionFactory;
        private readonly ILogger<HolidayRepository> logger;

        public HolidayRepository(DataContext context,
            IDbContextFactory<DataContext> contextFactory,
            IDapperConnectionFactory connectionFactory,
            ILogger<HolidayRepository> logger)
        {
            _context = context;

            _contextFactory = contextFactory;
            _connectionFactory = connectionFactory;
            this.logger = logger;
        }

        public async Task<Result<string>> CreateOrUpdateHoliday(Holiday entities)
        {
            try
            {

                await using var ctx = await _contextFactory.CreateDbContextAsync();
                if (entities.Id <= 0)
                {
                    ctx.Holidays.Add(entities);

                }
                else
                {
                    var emp = await ctx.Holidays
                                        .Where(x => x.Id == entities.Id &&
                                                    x.HolidayNum == entities.HolidayNum)
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

                    ctx.Holidays.Update(emp);
                }

                await ctx.SaveChangesAsync();

                return Result.Success<string>(new("Holiday was updated successfully!"));

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }



        public async Task<ErrorOr.ErrorOr<long>> CreateOrUpdate(Holiday entities, CancellationToken cancellation = default)
        {
            try
            {

                await using var ctx = await _contextFactory.CreateDbContextAsync(cancellation);

                if (entities.Id <= 0)
                {
                    ctx.Holidays.Add(entities);
                }
                else
                {
                    ctx.Holidays.Update(entities);
                }

                await ctx.SaveChangesAsync();

                return entities.Id;

            }
            catch (Exception ex)
            {
                return ErrorOr.Error.Unexpected($"{ex.InnerException!.Message}");
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


                context.Holidays.AddRange(addedEntitities);

                await context.SaveChangesAsync();

                var mapper = addedEntitities.ToHolidays();

                return Result.Success(mapper);

            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<HolidayVm>>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }


        public async Task<ErrorOr.ErrorOr<IEnumerable<HolidayVm>>> CreateHolidays(List<Holiday> entities)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();

                ICollection<Holiday> addedEntitities = new List<Holiday>();

                foreach (var entity in entities)
                {

                    if (await ExistHolidays(entity.HolidayDt)) continue;

                    addedEntitities.Add(entity);

                }

                if (!addedEntitities.Any())
                {
                    return ErrorOr.Error.NotFound("No new holidays were added!");
                }


                context.Holidays.AddRange(addedEntitities);

                await context.SaveChangesAsync();

                var mapper = addedEntitities.ToHolidays();

                return ErrorOr.ErrorOrFactory.From(mapper);

            }
            catch (Exception ex)
            {
                return ErrorOr.Error.Unexpected($"{ex.InnerException!.Message}");
            }
        }


        public async Task<Result<Holiday>> GetHoliday(long id)
        {
            try
            {
                await using var ctx = await _contextFactory.CreateDbContextAsync();

                var result = await ctx.Holidays.FindAsync(id);

                if (result is null)
                {
                    return Result.Failure<Holiday>(ErrorResponse.NotFound("No data found"));
                }
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure<Holiday>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }

        }


        public async Task<IEnumerable<HolidayVm>> GetHolidays(DateTime? selectedDate)
        {
            try
            {
                logger.LogInformation("Fetching holidays for the selected date.");
                await using var ctx = await _contextFactory.CreateDbContextAsync();

                var value = selectedDate ?? DateTime.Now;

                var month = value.Month;

                var year = value.Year;

                var queries = await ctx.Database
                                            .SqlQuery<HolidayVm>(@$"SELECT h.* FROM lt_holidays  h 
                                                        WHERE (h.HolidayDt IS NOT NULL) AND
                                                        DATEPART(YEAR,h.HolidayDt) = {year} AND 
                                                        DATEPART(MONTH,h.HolidayDt) = {month}")
                                            .OrderByDescending(x => x.HolidayDt)
                                            .AsNoTracking()
                                            .ToListAsync();
                return queries;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetHolidays: {ex.Message}", ex);
                throw new Exception($"Error in GetHolidays: {ex.Message}");
            }

        }




        public async Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious, DateTime? startFrom = null, DateTime? end = null)
        {
            try
            {
                logger.LogInformation("Fetching holidays based on the selected criteria.");
                await using var ctx = await _contextFactory.CreateDbContextAsync();

                IQueryable<Holiday> queries = ctx.Holidays;

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
            catch (Exception ex)
            {
                logger.LogError($"Error in GetHolidays: {ex.Message}", ex);
                throw new Exception($"Error in GetHolidays: {ex.Message}");
            }

        }

        public async Task<Pager<Holiday>> GetHolidays(int pageNumber, int pageSize, string sortColumn = "HolidayName",
            bool isSelectedPrevious = false,
            bool sortDescending = false,
            string? q = null)
        {
            try
            {
                logger.LogInformation("Fetching paginated holidays with sorting and filtering.");
                await using var ctx = await _contextFactory.CreateDbContextAsync();
                IQueryable<Holiday> queries = ctx.Holidays;
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
            catch (Exception ex)
            {
                logger.LogError($"Error in GetHolidays: {ex.Message}", ex);
                throw new Exception($"Error in GetHolidays: {ex.Message}");
            }

        }

        public async Task<Pager<HolidayVm>> GetHolidays(QueryParam q, CancellationToken cancellation = default)
        {
            try
            {
                logger.LogInformation("Fetching paginated holidays with query parameters.");
                await using var context = await _contextFactory.CreateDbContextAsync(cancellation);


                var queries = context.Database
                                      .SqlQuery<HolidayVm>($"SELECT * FROM VHolidaysVm")
                                      .WhereIf(!q.IncludeInActive, d => d.HolidayDt >= DateTime.Now)
                                      .WhereIf(!string.IsNullOrEmpty(q.Filter),
                                                x => EF.Functions.Like(x.Description, $"%{q.Filter}%") ||
                                                    EF.Functions.Like(x.HolidayNum.ToString(), $"%{q.Filter}%") ||
                                                    EF.Functions.Like(x.HolidayName!.ToString(), $"%{q.Filter}%") ||
                                                    EF.Functions.Like(x.HolidayDt.ToString(), $"%{q.Filter}%"));

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
                                    .Skip(q.Page)
                                    .Take(q.PageSize);

                }

                var items = await queries.AsNoTracking().ToListAsync(cancellation);

                return new Pager<HolidayVm>
                {
                    Count = counts,

                    Items = items
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetHolidays: {ex.Message}", ex);
                throw new Exception($"Error in GetHolidays: {ex.Message}");
            }

        }


        public async Task<IEnumerable<HolidayVm>> GetActiveHolidays(DateTime? selectedDate, int? selectedYear = 0)
        {
            try
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
                var queries = await db.QueryAsync<HolidayVm>(sql, parameters);

                return queries ?? [];
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetActiveHolidays: {ex.Message}", ex);
                throw new Exception($"Error in GetActiveHolidays: {ex.Message}");
            }


        }



        public async Task<bool> ExistHolidays(DateTime? setdate)
        {
            try
            {
                logger.LogInformation("Checking if holidays exist for the given date.");
                var query = await _context
                                .Holidays.Where(x => x.HolidayNum == setdate.DayNum())
                                .AsNoTracking()
                                .ToListAsync();
                return query.Any();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in ExistHolidays: {ex.Message}", ex);
                throw new Exception($"Error in ExistHolidays: {ex.Message}");
            }

        }
        public async Task<Result<string>> ActivateOrDeactivate(int id, Holiday req)
        {
            try
            {
                await using var ctx = await _contextFactory.CreateDbContextAsync();
                var data = await ctx.Holidays.Where(x => x.Id != id && x.Status == req.Status).FirstOrDefaultAsync();

                if (data is not null)
                    return Result.Failure<string>(ErrorResponse.Conflict("Holiday status already exist!"));

                ctx.Holidays.Update(req);

                await ctx.SaveChangesAsync();

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
                await using var ctx = await _contextFactory.CreateDbContextAsync();
                ctx.Holidays.AddRange(enity);
                await ctx.SaveChangesAsync();
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
                await using var ctx = await _contextFactory.CreateDbContextAsync();
                foreach (var i in id)
                {
                    var result = await ctx.Holidays.AsNoTracking().FirstOrDefaultAsync(x => x.Id == i);

                    ctx.Holidays.Remove(result!);

                }
                await ctx.SaveChangesAsync();
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
                await using var ctx = await _contextFactory.CreateDbContextAsync();
                var result = await ctx.Holidays.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                ctx.Holidays.Remove(result!);
                await ctx.SaveChangesAsync();
                return Result.Success<string>(new("Holiday was deleted successfully!"));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<ErrorOr.ErrorOr<DeleteResult>> DeleteHoliday(long? id, CancellationToken cancellation = default)
        {
            try
            {
                await using var ctx = await _contextFactory.CreateDbContextAsync(cancellation);

                var result = await ctx.Holidays.FirstOrDefaultAsync(x => x.Id == id);

                if (result is null)
                {
                    return ErrorOr.Error.NotFound("No Holiday found");
                }

                if (await IsHolidayInUsed(ctx, result, cancellation))
                {
                    return DeleteResult.InUse("Holiday is currently in use and cannot be deleted.");
                }

                ctx.Holidays.Remove(result);

                await ctx.SaveChangesAsync(cancellation);

                return DeleteResult.Success("Holiday was deleted successfully!");
            }
            catch (Exception ex)
            {
                return ErrorOr.Error.Unexpected($"{ex.InnerException!.Message}");
            }
        }

        private async Task<bool> IsHolidayInUsed(DataContext ctx, Holiday result, CancellationToken cancellation)
        {
            var isInUse = await ctx.LoanSchedules
                                 .AnyAsync(el => el.PaymentDate == result.HolidayDt, cancellation);
            return isInUse;
        }
    }
}


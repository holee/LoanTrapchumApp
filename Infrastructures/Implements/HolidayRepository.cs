using Applications.Bases;
using Applications.Extensions.DateTimes;
using Applications.Features.Holidays.Dtos;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Dapper;
using Domains.Models.Lists;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class HolidayRepository(IDbContextFactory<DataContext> contextFactory,
    IDapperConnectionFactory connectionFactory,
    ILogger<HolidayRepository> logger) : IHolidayService
{
    private IEnumerable<DateTime> _xistingHolidays = [];

    public async Task<ErrorOr<HolidayDto>> CreateOrUpdate(Holiday entities, CancellationToken cancellation = default)
    {
        try
        {

            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            if (entities.Id <= 0)
            {
                ctx.Holidays.Add(entities);
            }
            else
            {
                ctx.Holidays.Update(entities);
            }

            await ctx.SaveChangesAsync();

            var createdHoliday = await GetHolidayDetailsById(entities.Id, cancellation);

            return createdHoliday;
        }
        catch (Exception ex)
        {
            return Error.Unexpected($"{ex.InnerException!.Message}");
        }
    }

    public async Task<ErrorOr<IEnumerable<HolidayDto>>> CreateHolidays(List<Holiday> entities)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();

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

    public async Task<ErrorOr.ErrorOr<Holiday>> GetHoliday(long? id, CancellationToken cancellation = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            var result = await ctx.Holidays
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == id, cancellation);

            if (result is null)
            {
                return ErrorOr.Error.NotFound("No data found");
            }
            return ErrorOr.ErrorOrFactory.From(result);
        }
        catch (Exception ex)
        {
            return ErrorOr.Error.Unexpected($"{ex.InnerException!.Message}");
        }

    }

    public async Task<IEnumerable<HolidayDto>> GetHolidays(DateTime? selectedDate)
    {
        try
        {
            logger.LogInformation("Fetching holidays for the selected date.");

            await using var ctx = await contextFactory.CreateDbContextAsync();

            var value = selectedDate ?? DateTime.Now;

            var month = value.Month;

            var year = value.Year;

            var queries = await ctx.Database
                                        .SqlQuery<HolidayDto>(@$"SELECT h.* FROM lt_holidays  h 
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

            await using var ctx = await contextFactory.CreateDbContextAsync();

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
    public async Task<Pager<HolidayDto>> GetHolidays(QueryParam q, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("Fetching paginated holidays with query parameters.");

            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var baseQuery = context.Set<HolidayDto>().AsQueryable();

            var queries = ApplyFilter(q, baseQuery);

            queries = AppplyOrder(q, queries);

            var counts = await queries.CountAsync(cancellation);

            if (q.Take > 0)
            {
                queries = queries.Skip(q.Page).Take(q.Take);

            }

            var items = await queries.AsNoTracking().ToListAsync(cancellation);

            return PagerExtensions.Create(items, counts);

        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetHolidays: {ex.Message}", ex);

            throw new Exception($"Error in GetHolidays: {ex.Message}");
        }

    }
    public async Task<Pager<HolidayDto>> GetHolidays(QueryParam q, DateTime? selectedDate = null, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("Fetching paginated holidays with query parameters.");

            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var baseQuery = context.Set<HolidayDto>().AsQueryable();

            if (selectedDate.HasValue)
            {
                int year = selectedDate.Value.Year;

                int month = selectedDate.Value.Month;

                baseQuery = baseQuery.Where(x => x.HolidayDt.HasValue &
                                            x.HolidayDt!.Value.Year == year &&
                                            x.HolidayDt!.Value.Month == month)
                                    .OrderByDescending(x => x.HolidayDt);
            }

            var queries = ApplyFilter(q, baseQuery);

            queries = AppplyOrder(q, queries);

            var counts = await queries.CountAsync(cancellation);

            if (q.Take > 0)
            {

                var page = q.Page > 0 ? (q.Page - 1) * q.Take : 0;

                queries = queries.Skip(page).Take(q.Take);

            }

            var items = await queries.AsNoTracking().ToListAsync(cancellation);

            return PagerExtensions.Create(items, counts);
        }
        catch (Exception ex)
        {

            logger.LogError(ex, "Error occurred while fetching holidays.");

            throw new Exception($"Error in GetHolidays: {ex.Message}");

        }

    }
    public async Task<Pager<HolidayDto>> GetActiveHolidays(int? selectedYear, CancellationToken cancellation = default)
    {
        try
        {
            await using var db = await contextFactory.CreateDbContextAsync(cancellation);

            var endDate = selectedYear switch
            {
                > 0 => new DateTime(selectedYear.Value, 12, 31),

                _ => DateTime.Now.AddYears(1)
            };

            var queries = db.Set<HolidayDto>()
                            .Where(x => x.HolidayDt >= DateTime.Now && x.HolidayDt <= endDate)
                            .OrderBy(x => x.HolidayDt);
            var counts = await queries.CountAsync(cancellation);
            var items = await queries.AsNoTracking().ToListAsync(cancellation);
            return PagerExtensions.Create(items, counts);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetActiveHolidays: {ex.Message}", ex);
            throw new Exception($"Error in GetActiveHolidays: {ex.Message}");
        }


    }
    public async Task<IEnumerable<HolidayDto>> GetActiveHolidays(DateTime? selectedDate, int? selectedYear = 1)
    {
        try
        {
            using var db = connectionFactory.CreateConnection();

            var lastSelectedDate = selectedYear > 0 ? selectedDate!.Value.AddYears(selectedYear!.Value) : selectedDate;

            var parameters = new DynamicParameters();

            parameters.Add("@selectedDate", selectedDate);

            parameters.Add("@lastSelectedDate", lastSelectedDate);

            var sql = @"SELECT Id, HolidayDt, HolidayName, HolidayName, HolidayNum, Status, Description
                  FROM VHolidaysVm WHERE Status = CAST(1 AS BIT) AND 
                  CAST(HolidayDt AS DATE) BETWEEN CAST(@selectedDate AS DATE) AND 
                  CAST(@lastSelectedDate AS DATE);
                 ";
            var queries = await db.QueryAsync<HolidayDto>(sql, parameters);

            return queries ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetActiveHolidays: {ex.Message}", ex);
            throw new Exception($"Error in GetActiveHolidays: {ex.Message}");
        }


    }
    public async Task<bool> ExistHolidays(DateTime? setdate, CancellationToken cancellation = default)
    {

        await using var _context = await contextFactory.CreateDbContextAsync(cancellation);

        try
        {
            logger.LogInformation("Checking if holidays exist for the given date.");
            var query = await _context
                            .Holidays.Where(x => x.HolidayNum == setdate.DayNum())
                            .AsNoTracking()
                            .ToListAsync(cancellation);
            return query.Any();
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in ExistHolidays: {ex.Message}", ex);
            throw new Exception($"Error in ExistHolidays: {ex.Message}");
        }

    }
    public async Task<ErrorOr<string>> ActivateOrDeactivate(int id, Holiday req, CancellationToken cancellation = default)
    {

        logger.LogInformation("Activating or deactivating holiday with ID: {HolidayId}", id);

        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync();

            var data = await ctx.Holidays.Where(x => x.Id != id && x.Status == req.Status).FirstOrDefaultAsync();

            if (data is not null)
                return ErrorOr.Error.Conflict("Holiday status already exist!");

            //ctx.Holidays.Update(req);

            await ctx.SaveChangesAsync();

            return "Holiday status was updated successfully!";

        }
        catch (Exception ex)
        {

            logger.LogError($"Error in ActivateOrDeactivate: {ex.Message}", ex);

            return ErrorOr.Error.Unexpected($"{ex.Message}");
        }

    }

    public async Task<ErrorOr<HolidayDto>> ActivateOrDeactivate(long? id, CancellationToken cancellation = default)
    {

        logger.LogInformation("Activating or deactivating holiday with ID: {HolidayId}", id);

        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            var holiday = await ctx.Holidays.FindAsync(id, cancellation);

            if (holiday is null)
                return Error.NotFound("Holiday status not found!");

            // Toggle status (default to true if null)
            holiday.Status = !(holiday.Status ?? false);

            await ctx.SaveChangesAsync();

            return await GetHolidayDetailsById(id, cancellation);

        }
        catch (Exception ex)
        {

            logger.LogError(ex, "Error activating/deactivating holiday with ID: {HolidayId}", id);

            return Error.Unexpected("Holiday.ActivateDeactivateFailed", "An error occurred while updating the holiday status.");
        }

    }

    public async Task<ErrorOr<Holiday>> GetByDate(DateTime? dt, CancellationToken cancellation = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            var data = await ctx.Holidays
                        .AsNoTracking()
                        .Where(x => x.HolidayDt == dt)
                        .FirstOrDefaultAsync(cancellation);

            if (data is null)
                return ErrorOr.Error.NotFound("No data found");

            return data;
        }
        catch (Exception ex)
        {
            return ErrorOr.Error.Unexpected($"{ex.Message}");
        }
    }
    public async Task<Pager<Holiday>> BulkCreateHoliday(List<Holiday> enities, CancellationToken cancellation = default)
    {

        await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

        ctx.Holidays.AddRange(enities);

        await ctx.SaveChangesAsync(cancellation);

        var counts = enities.Count;

        return PagerExtensions.Create(enities, counts);

    }
    public async Task<ErrorOr<DeleteResult>> DeleteHoliday(long? id, CancellationToken cancellation = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

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
    private async Task<ErrorOr<HolidayDto>> GetHolidayDetailsById(long? id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellationToken);

            var result = await ctx.Set<HolidayDto>()
                                  .AsNoTracking()
                                  .Select(x => new HolidayDto
                                  {
                                      Id = x.Id,
                                      HolidayDt = x.HolidayDt,
                                      HolidayName = x.HolidayName,
                                      HolidayNum = x.HolidayNum,
                                      Status = x.Status,
                                      Description = x.Description
                                  })
                                  .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result is null)
            {
                return ErrorOr.Error.NotFound("No data found");
            }
            return result;
        }
        catch (Exception ex)
        {
            return ErrorOr.Error.Unexpected($"{ex.InnerException!.Message}");
        }

    }
    private static IQueryable<HolidayDto> AppplyOrder(QueryParam q, IQueryable<HolidayDto> queries)
    {
        if (q.SortDescending
            && !string.IsNullOrEmpty(q.SortColumn)
            && AllowedFilterColumns.Contains(q.SortColumn))
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, q.SortColumn));
        }
        else
        {
            queries = queries.OrderBy(x => x.HolidayDt);
        }

        return queries;
    }
    private static IQueryable<HolidayDto> ApplyFilter(QueryParam q, IQueryable<HolidayDto> queries)
    {

        var filter = q.Filter?.Trim();

        if (string.IsNullOrEmpty(filter))
            return queries;

        if (string.IsNullOrEmpty(q.FilterColumn))
        {
            return queries.StartWiths(filter, AllowedFilterColumns.ToArray());
        }

        if (AllowedFilterColumns.Contains(q.FilterColumn))
        {
            return queries.StartWiths(filter, q.FilterColumn);
        }

        return queries;
    }

    private static readonly HashSet<string> AllowedFilterColumns = new HashSet<string>
    {
        nameof(HolidayDto.HolidayName),
        nameof(HolidayDto.HolidayNum),
        nameof(HolidayDto.HolidayDt),
        nameof(HolidayDto.Description)
    };
}


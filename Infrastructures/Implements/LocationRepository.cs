using Applications.Features.Locations.Dtos;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using ErrorOr;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class LocationRepository(
    IDbContextFactory<DataContext> contextFactory,
    ILogger<LocationRepository> logger) : ILocationService
{
    public async Task<ErrorOr<LocationDto>> GetByIdAsync(long id, CancellationToken cancellation = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            var result = await ctx.Locations
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new LocationDto
                {
                    Id = x.Id,
                    LocationName = x.LocationName,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    CreatedDT = x.CreatedDT,
                    ModifiedDT = x.ModifiedDT
                })
                .FirstOrDefaultAsync(cancellation);

            if (result is null)
                return Error.NotFound(description: "Location not found.");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching location by id {Id}", id);
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<IEnumerable<LocationDto>> GetAllAsync(CancellationToken cancellation = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            return await ctx.Locations
                .AsNoTracking()
                .OrderBy(x => x.LocationName)
                .Select(x => new LocationDto
                {
                    Id = x.Id,
                    LocationName = x.LocationName,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    CreatedDT = x.CreatedDT,
                    ModifiedDT = x.ModifiedDT
                })
                .ToListAsync(cancellation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching all locations.");
            throw new Exception($"Error fetching locations: {ex.Message}");
        }
    }

    public async Task<Pager<LocationDto>> GetPagedAsync(QueryParam q, CancellationToken cancellation = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            var baseQuery = ctx.Locations
                .AsNoTracking()
                .Select(x => new LocationDto
                {
                    Id = x.Id,
                    LocationName = x.LocationName,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    CreatedDT = x.CreatedDT,
                    ModifiedDT = x.ModifiedDT
                });

            if (!string.IsNullOrWhiteSpace(q.Filter))
            {
                var filter = q.Filter.Trim();
                baseQuery = baseQuery.Where(x =>
                    (x.LocationName != null && EF.Functions.Like(x.LocationName, $"%{filter}%")) ||
                    (x.Latitude != null && EF.Functions.Like(x.Latitude, $"%{filter}%")) ||
                    (x.Longitude != null && EF.Functions.Like(x.Longitude, $"%{filter}%")));
            }

            baseQuery = q.SortDescending
                ? baseQuery.OrderByDescending(x => x.LocationName)
                : baseQuery.OrderBy(x => x.LocationName);

            var count = await baseQuery.CountAsync(cancellation);

            if (q.Take > 0)
            {
                var page = q.Page > 0 ? (q.Page - 1) * q.Take : 0;

                baseQuery = baseQuery.Skip(page).Take(q.Take);

            }

            var items = await baseQuery.ToListAsync(cancellation);

            return PagerExtensions.Create(items, count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching paged locations.");
            throw new Exception($"Error fetching paged locations: {ex.Message}");
        }
    }

    public async Task<ErrorOr<LocationDto>> CreateAsync(LtLocation entity, CancellationToken cancellation = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            ctx.Locations.Add(entity);
            await ctx.SaveChangesAsync(cancellation);

            return await GetByIdAsync(entity.Id, cancellation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating location.");
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<LocationDto>> UpdateAsync(LtLocation entity, CancellationToken cancellation = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            var existing = await ctx.Locations.FirstOrDefaultAsync(x => x.Id == entity.Id, cancellation);
            if (existing is null)
                return Error.NotFound(description: "Location not found.");

            existing.LocationName = entity.LocationName;
            existing.Latitude = entity.Latitude;
            existing.Longitude = entity.Longitude;
            existing.ModifiedDT = DateTimeOffset.UtcNow;

            await ctx.SaveChangesAsync(cancellation);

            return await GetByIdAsync(existing.Id, cancellation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating location.");
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<string>> DeleteAsync(long id, CancellationToken cancellation = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            var existing = await ctx.Locations.FirstOrDefaultAsync(x => x.Id == id, cancellation);
            if (existing is null)
                return Error.NotFound(description: "Location not found.");

            ctx.Locations.Remove(existing);
            await ctx.SaveChangesAsync(cancellation);

            return "Location was deleted successfully.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting location with id {Id}", id);
            return Error.Unexpected(description: ex.Message);
        }
    }
}

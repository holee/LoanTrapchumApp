using Applications.Commons.Mappers;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Dto.Responses;
using Applications.Interfaces.Vendors;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using ErrorOr;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class VendorRepository(
    IDbContextFactory<DataContext> dbContextFactory,
    ILogger<VendorRepository> logger) : IVendorService
{
    public async Task<ErrorOr<VendorDto>> CreateAsync(EntityNames vendor, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            context.EntityNames.Add(vendor);
            await context.SaveChangesAsync(cancellation);

            var created = await context.Database
                .SqlQuery<VendorDto>($"SELECT * FROM VVendors WHERE VendorId={vendor.Id}")
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellation);

            return created is null
                ? Error.NotFound(description: "Vendor was created but could not be retrieved.")
                : created;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to create vendor.");
            return Error.Conflict(description: ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ErrorOr<VendorDto>> UpdateAsync(EntityNames vendor, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            context.EntityNames.Update(vendor);
            await context.SaveChangesAsync(cancellation);

            var updated = await context.Database
                .SqlQuery<VendorDto>($"SELECT * FROM VVendors WHERE VendorId={vendor.Id}")
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellation);

            return updated is null
                ? Error.NotFound(description: "Vendor was updated but could not be retrieved.")
                : updated;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to update vendor {VendorId}.", vendor.Id);
            return Error.Conflict(description: ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ErrorOr<Updated>> ChangeVendorStatusAsync(string? vendorId, bool state, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            var vendor = await context.EntityNames
                .Where(x => x.Id == vendorId)
                .Include(x => x.Vendor)
                .FirstOrDefaultAsync(cancellation);

            if (vendor is null)
                return Error.NotFound(description: "Vendor not found.");

            vendor.IsActive = state;
            await context.SaveChangesAsync(cancellation);

            return Result.Updated;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to change status for vendor {VendorId}.", vendorId);
            return Error.Conflict(description: ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(string? id, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

            var vendor = await context.EntityNames.FindAsync([id], cancellation);

            if (vendor is null)
                return Error.NotFound(description: "Vendor not found.");

            var isInUse = await context.Database
                .SqlQuery<VLoanGuarantors>($"SELECT * FROM VLoanGuarators WHERE GuarantorId={id}")
                .AnyAsync(cancellation);

            if (isInUse)
                return Error.Conflict(description: "Vendor is currently in use and cannot be deleted.");

            context.EntityNames.Remove(vendor);
            await context.SaveChangesAsync(cancellation);

            return Result.Deleted;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete vendor {VendorId}.", id);
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<EntityNames>> GetVendorEntityByIdAsync(string? vendorId, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var vendor = await context.EntityNames
            .Include(x => x.Vendor)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == vendorId, cancellation);

        return vendor is null
            ? Error.NotFound(description: $"Vendor with id '{vendorId}' was not found.")
            : vendor;
    }


    public async Task<ErrorOr<EntityVendorCreate>> GetEditVendorByIdAsync(string? vendorId, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var vendor = await context.EntityNames
                            .Include(x => x.Vendor)
                            .Where(x => x.Id == vendorId)
                            .Select(x => x.ToEntityVendorCreate())
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellation);

        return vendor is null
            ? Error.NotFound(description: $"Vendor with id '{vendorId}' was not found.")
            : vendor;
    }

    public async Task<ErrorOr<VendorDto>> GetVendorByIdAsync(string? vendorId, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var vendor = await context.Database
            .SqlQuery<VendorDto>($"SELECT * FROM VVendors WHERE VendorId={vendorId}")
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellation);

        return vendor is null
            ? Error.NotFound(description: $"Vendor with id '{vendorId}' was not found.")
            : vendor;
    }

    public async Task<Pager<VendorDto>> GetVendorsAsync(QueryParam q, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var query = context.VVendor.AsNoTracking();

        if (!string.IsNullOrEmpty(q.Filter))
        {
            var filter = $"%{q.Filter}%";
            query = query.Where(e =>
                EF.Functions.Like(e.FirstName!, filter) ||
                EF.Functions.Like(e.LastName!, filter) ||
                EF.Functions.Like(e.Phone!, filter) ||
                EF.Functions.Like(e.Salutation!, filter) ||
                EF.Functions.Like(e.EmailAddress!, filter) ||
                EF.Functions.Like(e.AltName!, filter));
        }

        if (!q.IncludeInActive)
            query = query.Where(x => x.IsActive);

        if (q.SortDescending && !string.IsNullOrEmpty(q.SortColumn))
            query = query.OrderByDescending(e => EF.Property<object>(e, q.SortColumn));

        var totalCount = await query.CountAsync(cancellation);

        if (q.Take > 0)
            query = query.Skip(q.Page).Take(q.Take);

        var items = await query.ToListAsync(cancellation);

        return new Pager<VendorDto>(items, totalCount);
    }
}

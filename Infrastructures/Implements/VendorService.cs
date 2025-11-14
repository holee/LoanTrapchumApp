using Applications.Features.Loans.Responses;
using Applications.Interfaces.Vendors;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class VendorService : IVendorService
    {
        private readonly DataContext _context;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public readonly ILogger<VendorService> logger;
        public VendorService(DataContext context, IDbContextFactory<DataContext> dbContextFactory, ILogger<VendorService> logger)
        {
            this._context = context;
            _dbContextFactory = dbContextFactory;
            this.logger = logger;
        }

        public async Task<Result<string>> CreateAsync(EntityNames vendor, CancellationToken cancellation = default)
        {
            try
            {
                await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

                context.EntityNames.Add(vendor);

                await context.SaveChangesAsync();

                return Result.Success(new string("Name was created or updated."));

            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
            }
        }


        public async Task<Result<string>> UpdateAsync(EntityNames vendor, CancellationToken cancellation = default)
        {
            try
            {

                await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

                context.EntityNames.Update(vendor);

                await context.SaveChangesAsync(cancellation);

                return Result.Success(new string("Name was created or updated."));

            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
            }

        }


        public async Task<Result<string>> ChangeVendorStatus(string? vendorId, bool state, CancellationToken cancellation = default)
        {
            try
            {

                await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

                var vendor = await context.EntityNames
                                           .Where(x => x.Id == vendorId)
                                           .Include(x => x.Vendor)
                                           .FirstOrDefaultAsync(cancellation);

                if (vendor is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("Vendor not found."));
                }

                vendor.IsActive = state;

                await context.SaveChangesAsync(cancellation);

                return Result.Success(new string("Vendor was changed."));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
            }

        }


        public async Task<Result<string>> DeleteAsync(string? id, CancellationToken cancellation = default)
        {
            try
            {
                await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

                var vendor = await context.EntityNames.FindAsync(id, cancellation);

                if (vendor is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("There is no any vendor to delete."));
                }

                var usedVendor = await context.Database
                                                .SqlQuery<VLoanGuarantors>($"SELECT * FROM VLoanGuarators WHERE GuarantorId={id}")
                                                .AnyAsync(cancellation);
                if (usedVendor)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("Vendor already in used."));
                }

                context.EntityNames.Remove(vendor);

                await context.SaveChangesAsync(cancellation);

                return Result.Success(new string("Name was created was deleted."));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
            }
        }


        public async Task<Result<EntityNames>> GetVendorByIdAsync(string? vendorId, bool include)
        {

            await using var context = await _dbContextFactory.CreateDbContextAsync();


            var vendor = await context.EntityNames
                                    .Where(x => x.Id == vendorId)
                                    .Include(x => x.Vendor)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

            if (vendor is null)
            {
                return Result.Failure<EntityNames>(ErrorResponse.NotFound("There is no any vendor to delete."));
            }
            return Result.Success(vendor);
        }

        public async Task<Result<VendorVm>> GetVendorByIdAsync(string? vendorId)
        {

            await using var context = await _dbContextFactory.CreateDbContextAsync();


            var vendor = await context.Database.SqlQuery<VendorVm>($"SELECT * FROM VVendors WHERE VendorId={vendorId}")
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync();

            if (vendor is null)
            {
                return Result.Failure<VendorVm>(ErrorResponse.NotFound("There is no any vendor to delete."));
            }
            return Result.Success(vendor);
        }


        public async Task<Pager<VendorVm>> GetVendorsAsync(QueryParam q, CancellationToken cancellation = default)
        {

            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var queries = context.VVendor.AsNoTracking();

            if (!string.IsNullOrEmpty(q.Filter))
            {
                queries = queries.Where(e => EF.Functions.Like(e.FirstName!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(e.LastName!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(e.Phone!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(e.Salutation!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(e.EmailAddress!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(e.AltName!, $"%{q.Filter}%"));
            }

            if (!q.IsIncludeInActive)
            {
                queries = queries.Where(x => x.IsActive == true);
            }

            if (q.SortDescending)
            {
                queries = queries.OrderByDescending(e => EF.Property<object>(e, q.SortColumn));
            }

            var counts = queries.Count();

            if (q.PageSize > 0)
            {
                queries = queries.Skip(q.Skip).Take(q.PageSize);
            }
            var items = await queries.AsNoTracking().ToListAsync();

            return new Pager<VendorVm>(items, counts);
        }
    }
}

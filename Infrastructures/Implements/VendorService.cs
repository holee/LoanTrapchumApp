using Applications.Interfaces.Vendors;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models.Bases;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class VendorService : IVendorService
    {
        private readonly ApplicationDbContext _context;
        public VendorService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Result<string>> CreateAsync(EntityNames vendor)
        {
            try
            {
                vendor.Id = $"{Guid.CreateVersion7()}";

                vendor.EntType = EntType.Vendor.ToString();

                _context.EntityNames.Add(vendor);

                await _context.SaveChangesAsync();

                return Result.Success(new string("Name was created or updated."));

            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
            }
        }


        public async Task<Result<string>> UpdateAsync(EntityNames vendor)
        {
            try
            {
                _context.Entry(vendor).State = EntityState.Modified;

                _context.Entry(vendor.Vendor!).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Result.Success(new string("Name was created or updated."));

            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
            }

        }


        //public async Task<Result<string>> ChangeVendorStatus(EntVendorDtoChangeStatus req)
        //{
        //    try
        //    {

        //        var vendor = await _context.EntityNames.FirstOrDefaultAsync(x => x.Id == req.VendorId && x.EditSeq == req.EditSeq);

        //        if (vendor is null)
        //        {
        //            return Result.Failure<string>(ErrorResponse.NotFound("Vendor not found."));
        //        }

        //        vendor.IsActive = req.IsActive;

        //        vendor.EditSeq = req.EditSeq + 1;

        //        await _context.SaveChangesAsync();

        //        return Result.Success(new string("Vendor was changed."));
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
        //    }

        //}


        public async Task<Result<string>> DeleteAsync(string? id)
        {
            try
            {

                var vendor = await _context.EntityNames.FindAsync(id);

                if (vendor is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("There is no any vendor to delete."));
                }

                var usedVendor = await _context.Database
                                                .SqlQuery<VLoanGuarantors>($"SELECT * FROM VLoanGuarators WHERE GuarantorId={id}")
                                                .AnyAsync();
                if (usedVendor)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("Vendor already in used."));
                }

                _context.EntityNames.Remove(vendor);

                await _context.SaveChangesAsync();

                return Result.Success(new string("Name was created was deleted."));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
            }
        }


        public async Task<Result<EntityNames>> GetVendorByIdAsync(string? vendorId, bool include)
        {
            var vendor = await _context.EntityNames
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
            var vendor = await _context.Database.SqlQuery<VendorVm>($"SELECT * FROM VVendors WHERE VendorId={vendorId}")
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync();

            if (vendor is null)
            {
                return Result.Failure<VendorVm>(ErrorResponse.NotFound("There is no any vendor to delete."));
            }
            return Result.Success(vendor);
        }


        public async Task<Pager<VendorVm>> GetVendorsAsync(QueryParam q)
        {

            var queries = _context.Database.SqlQuery<VendorVm>($"SELECT * FROM VVendors");

            if (!string.IsNullOrEmpty(q.Filter))
            {
                queries = queries.Where(e => EF.Functions.Like(e.FirstName!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(e.LastName!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(e.Phone!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(e.AltName!, $"%{q.Filter}%"));
            }

            if (q.SortDescending)
            {
                queries = queries.OrderByDescending(e => EF.Property<object>(e, q.SortColumn));
            }

            var counts = queries.Count();

            if (q.PageSize > 0)
            {
                queries = queries.Skip(q.PageSize * (q.PageNumber - 1)).Take(q.PageSize);
            }
            var items = await queries.AsNoTracking().ToListAsync();

            return new Pager<VendorVm>(items, counts);
        }
    }
}

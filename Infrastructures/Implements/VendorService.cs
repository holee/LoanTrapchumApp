using Applications.Interfaces.Vendors;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models.Lists;
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

        public async Task<Result<string>> CreateOrUpdate(EntityName vendor)
        {
            try
            {
                if (vendor.EntId == Guid.Empty && !CheckVendorName(vendor.Name!))
                {
                    _context.Vendors.Add(vendor);
                }
                else
                {
                    if (!VendorExist(vendor.EntId, vendor.EditSeq))
                    {
                        return Result.Failure<string>(ErrorResponse.NotFound("There is no Vendor Or Someone has changed this Vendor"));
                    }

                    if (CheckVendorName(vendor.EntId, vendor.Name!))
                    {
                        return Result.Failure<string>(ErrorResponse.NotFound("Name already exist"));
                    }

                    _context.Update(vendor);
                }

                await _context.SaveChangesAsync();

                return Result.Success(new string("Name was created or updated."));

            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
            }

        }


        public async Task<Result<string>> ChangeVendorStatus(EntVendorDtoChangeStatus req)
        {
            try
            {

                var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.EntId == req.VendorId && x.EditSeq == req.EditSeq);

                if (vendor is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("Vendor not found."));
                }

                vendor.IsActive = req.IsActive;

                vendor.EditSeq = req.EditSeq + 1;

                await _context.SaveChangesAsync();

                return Result.Success(new string("Vendor was changed."));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
            }

        }


        public async Task<Result<string>> DeleteAsync(Guid? id)
        {
            try
            {

                var vendor = _context.Vendors.Where(x => x.EntId == id).FirstOrDefault();

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



                _context.Vendors.Remove(vendor);

                await _context.SaveChangesAsync();

                return Result.Success(new string("Name was created was deleted."));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
            }
        }


        public async Task<Result<EntityName>> GetVendorByIdAsync(Guid? vendorId, bool include)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.EntId == vendorId);

            if (vendor is null)
            {
                return Result.Failure<EntityName>(ErrorResponse.NotFound("There is no any vendor to delete."));
            }
            return Result.Success(vendor);
        }

        public async Task<Result<VVendor>> GetVendorByIdAsync(Guid? vendorId)
        {
            var vendor = await _context.Database.SqlQuery<VVendor>($"SELECT * FROM VVendors")
                                        .FirstOrDefaultAsync(v => v.EntId == vendorId);

            if (vendor is null)
            {
                return Result.Failure<VVendor>(ErrorResponse.NotFound("There is no any vendor to delete."));
            }
            return Result.Success(vendor);
        }


        public async Task<Pager<VVendor>> GetVendorsAsync(QueryParam q)
        {

            var queries = _context.Database.SqlQuery<VVendor>($"SELECT * FROM VVendors");

            if (!string.IsNullOrEmpty(q.Filter))
            {
                queries = queries.Where(e => EF.Functions.Like(e.FirstName, $"{q.Filter}") ||
                                         EF.Functions.Like(e.LastName, $"{q.Filter}") ||
                                         EF.Functions.Like(e.Phone, $"{q.Filter}") ||
                                         EF.Functions.Like(e.AltName, $"{q.Filter}"));
            }

            if (q.SortDescending)
            {
                queries = queries.OrderByDescending(e => EF.Property<object>(e, q.SortColumn));
            }

            var counts = await queries.CountAsync();
            var items = await queries.Skip(q.PageIndex).Take(q.PageSize).ToListAsync();
            return new Pager<VVendor>(items, counts);
        }


        private bool VendorExist(Guid? id, int editSeq)
        {
            var vendors = _context.Vendors
                                    .Where(v => v.EntId == id && v.EditSeq == editSeq);
            return vendors.Any();

        }

        private bool CheckVendorName(string name)
        {
            var vendors = _context.Vendors
                                    .Where(v => v.Name!.ToLower() == name.ToLower());
            return vendors.Any();

        }

        private bool CheckVendorName(Guid? id, string name)
        {
            var vendors = _context.Vendors
                                    .Where(v => v.Name!.ToLower() == name.ToLower() && v.EntId != id);
            return vendors.Any();

        }

    }
}

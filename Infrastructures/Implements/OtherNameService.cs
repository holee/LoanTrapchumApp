using Applications.Enumerations;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;
using FluentValidation;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class OtherNameService : IOtherNameService
    {
        private readonly ApplicationDbContext _context;

        private IValidator<EntityNameDtoCreate> _validator;
        public OtherNameService(ApplicationDbContext context,
            IValidator<EntityNameDtoCreate> validator)
        {
            this._context = context;
            _validator = validator;
        }
        public async Task<Result<string>> CreateAsync(EntityName othername)
        {
            try
            {
                _context.Add(othername);
                await SaveAsync();
                return Result.Success(new string("A new Name was created"));

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }


        public async Task<Result<string>> CreateGaurantorAsync(EntityName othername)
        {
            try
            {
                var req = othername.Adapt<EntityNameDtoCreate>();
                var validatorResult = _validator.Validate(req);
                if (!validatorResult.IsValid)
                {
                    var errors = validatorResult.Errors.Select(er => er.ErrorMessage).ToList();
                    var errString = string.Join(Environment.NewLine, errors);
                    return Result.Failure<string>(ErrorResponse.Validation(errString));
                }
                _context.Add(othername);
                await _context.SaveChangesAsync();
                return Result.Success(new string("A new guarantor was created"));

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.Validation($"{ex.Message}"));
            }
        }


        public async Task<Result<string>> DeleteAsync(long othernameId)
        {
            try
            {
                var result = await _context.OtherNames
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.EntId == othernameId);

                if (result is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("No Data Found."));
                }

                var usedVendor = await _context.Database
                                              .SqlQuery<VLoanGuarantors>($"SELECT * FROM VLoanGuarators WHERE GuarantorId={othernameId}")
                                              .AnyAsync();
                if (usedVendor)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("Vendor already in used."));
                }

                _context.OtherNames.Remove(result);

                await SaveAsync();

                return Result.Success<string>(new("Name was deleted."));

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}."));
            }
        }
        public async Task<IEnumerable<EntityName>> GetAllAsync(string? searchTerm, string sortField = "AltName", string sortOrder = "ASC", bool isActive = false)
        {
            IQueryable<EntityName> entities = _context.OtherNames.Where(x => x.EntType == "Other Name").OrderByDynamic(sortField, sortOrder);
            if (isActive == false)
            {
                entities = entities.Where(x => x.IsActive == true);
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(x => x.FirstName!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                         x.Name!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                         x.AltName!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                        x.MiddleName!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                        x.LastName!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                        x.Sex!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                        x.Phone!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase)).OrderByDynamic(sortField, sortOrder);
            }
            var items = await entities.AsNoTracking().ToListAsync() ?? [];

            return items;
        }



        public async Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type)
        {

            var queries = _context.Database.SqlQuery<VOtherNames>($"SELECT * FROM VNames WHERE EntType={type}");

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
            return new Pager<VOtherNames>(items, counts);
        }


        public async Task<EntityName?> GetByIdAsync(long id)
        {
            return await _context.OtherNames.AsNoTracking().FirstOrDefaultAsync(x => x.EntId == id);
        }

        public Task<EntityName?> GetDetailAsync(long id)
        {
            throw new NotImplementedException();
        }

        private async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Result<string>> ConvertToOtherNameAsync(long id, EntityName othername)
        {
            try
            {
                var result = await _context.OtherNames.AsNoTracking().FirstOrDefaultAsync(x => x.EntId == id);

                if (result is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound($"NO Data Found."));
                }
                _context.OtherNames.Update(othername);
                await SaveAsync();
                return Result.Success(new string("Name was converted to vendor"));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}."));
            }
        }

        public async Task<Result<string>> ConvertToCustomerAsync(long id, Customer customer)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var otherName = await _context.OtherNames.FindAsync(id);
                _context.OtherNames.Remove(otherName!);
                await SaveAsync();
                //customer.EntType = "Employee";
                _context.Customers.Add(customer);
                await SaveAsync();
                transaction.Commit();

                return Result.Success(new string("Name was converted to vendor"));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}."));
            }
        }
        public async Task<Result<string>> ConvertToVendorAsync(long otherNameId, Vendor vendor)
        {
            try
            {
                var otherName = await _context.OtherNames.FindAsync(otherNameId);

                if (otherName is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("No Data found."));
                }
                await _context.OtherNames.Where(x => x.EntId == otherNameId)
                                  .ExecuteUpdateAsync(x => x.SetProperty(p => p.EntType, EntityTypes.Vendor.ToString()));

                return Result.Success(new string("Name was converted to vendor"));

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}."));
            }
        }

        public async Task<Result<string>> ConvertToEmployeeAsync(long id, Employee employee)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var otherName = await _context.OtherNames.FindAsync(id);
                _context.OtherNames.Remove(otherName!);
                await SaveAsync();
                _context.Employees.Add(employee);
                await SaveAsync();
                transaction.Commit();

                return Result.Success(new string("Name was converted to vendor"));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}."));
            }
        }

        public async Task<Result<string>> UpdateStatusAsync(long othernameId)
        {
            try
            {
                var result = await _context.OtherNames.AsNoTracking().FirstOrDefaultAsync(x => x.EntId == othernameId);
                result!.IsActive = result.IsActive ? false : true;
                _context.OtherNames.Update(result);
                await _context.SaveChangesAsync();
                return Result.Success("Other Name was updated successfully!");
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }
    }
}

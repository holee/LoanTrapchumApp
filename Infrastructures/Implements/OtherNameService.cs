using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Bases;
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
        private readonly DataContext _context;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public OtherNameService(DataContext context, IDbContextFactory<DataContext> dbContextFactory)
        {
            _context = context;
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Result<string>> CreateAsync(EntityNames othername, CancellationToken cancellation = default)
        {
            try
            {
                await using var _context = await _dbContextFactory.CreateDbContextAsync(cancellation);

                _context.EntityNames.Add(othername);

                await _context.SaveChangesAsync(cancellation);

                return Result.Success(new string("A new Name was created"));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }


        public async Task<Result<string>> UpdateAsync(EntityNames othername)
        {
            try
            {
                await using var context = await _dbContextFactory.CreateDbContextAsync();

                context.EntityNames.Update(othername);

                await context.SaveChangesAsync();

                return Result.Success(new string("A new Name was created"));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }

        public async Task<Result<string>> CreateGaurantorAsync(EntityNames othername, CancellationToken cancellation = default)
        {
            try
            {

                await using var context = await _dbContextFactory.CreateDbContextAsync();

                _context.EntityNames.Add(othername);

                await _context.SaveChangesAsync();

                return Result.Success(new string("A new guarantor was created"));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.Validation($"{ex.Message}"));
            }
        }


        public async Task<Result<string>> DeleteAsync(string? id)
        {
            try
            {
                var result = await _context.EntityNames.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (result is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("No Data Found."));
                }

                var usedVendor = await _context.Database
                    .SqlQuery<VLoanGuarantors>($"SELECT * FROM VLoanGuarators WHERE GuarantorId={id}")
                    .AnyAsync();


                if (usedVendor)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("Vendor already in used."));
                }

                _context.EntityNames.Remove(result);

                await _context.SaveChangesAsync();

                return Result.Success<string>(new("Name was deleted."));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}."));
            }
        }


        public async Task<Result<T>> GetDetailAsync<T>(string? id)
            where T : class
        {
            var item = await _context.Database.SqlQuery<VOtherNames>($"SELECT * FROM VNames WHERE Id={id}")
                .ProjectToType<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (item is null)
            {
                return Result.Failure<T>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            return Result.Success(item);
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
            var items = await queries.Skip(q.Page).Take(q.PageSize).ToListAsync();
            return new Pager<VOtherNames>(items, counts);
        }


        public async Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, CancellationToken cancellation = default)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var queries = context.VOtherNames.AsNoTracking();

            if (!string.IsNullOrEmpty(q.Filter))
            {
                queries = queries.Where(e => EF.Functions.Like(e.FirstName, $"%{q.Filter}%") ||
                                             EF.Functions.Like(e.LastName, $"%{q.Filter}%") ||
                                             EF.Functions.Like(e.EmailAddress, $"%{q.Filter}%") ||
                                             EF.Functions.Like(e.Gender, $"%{q.Filter}%") ||
                                             EF.Functions.Like(e.Phone, $"%{q.Filter}%") ||
                                             EF.Functions.Like(e.AltName, $"%{q.Filter}%"));
            }

            if (!q.IncludeInActive)
            {
                queries = queries.Where(x => x.IsActive == true);
            }

            if (q.SortDescending)
            {
                queries = queries.OrderByDescending(e => EF.Property<object>(e, q.SortColumn));
            }

            var counts = await queries.CountAsync(cancellation);

            if (q.PageSize > 0)
            {
                queries = queries.Skip(q.Page).Take(q.PageSize);
            }


            var items = await queries.AsNoTracking().ToListAsync(cancellation);

            return new Pager<VOtherNames>(items, counts);
        }




        public async Task<Result<EntityNames>> GetByIdAsync(string? id)
        {
            var name = await _context.EntityNames
                .Where(x => x.Id == id && x.EntType!.Equals(EntType.OtherName.ToString()))
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (name is null)
            {
                return Result.Failure<EntityNames>(ErrorResponse.NotFound(Messages.DataNotFound));
            }


            return Result.Success(name);
        }


        public async Task<Result<string>> ConvertToOtherNameAsync(string? id, EntityNames othername)
        {
            try
            {
                var result = await _context.EntityNames.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (result is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound($"NO Data Found."));
                }

                _context.EntityNames.Update(othername);

                await _context.SaveChangesAsync();

                return Result.Success(new string("Name was converted to vendor"));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}."));
            }
        }

        public async Task<Result<string>> ConvertToCustomerAsync(string? id, Customer customer, CancellationToken cancellation = default)
        {

            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);
            try
            {
                var otherName = await context.EntityNames
                                              .FirstOrDefaultAsync(x => x.Id == id &&
                                              x.Converted == false &&
                                              x.EntType == EntType.OtherName.ToString(), cancellation);

                if (otherName is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
                }

                otherName.EntType = EntityTypes.Customer.ToString();
                otherName.Converted = true;
                otherName.Customer = customer;
                await context.SaveChangesAsync(cancellation);
                return Result.Success(new string("Name was converted to customer"));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}."));
            }
        }

        public async Task<Result<string>> ConvertToVendorAsync(string? id, Vendor vendor, CancellationToken cancellation = default)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);
            try
            {
                var otherName = await context.EntityNames
                                        .FirstOrDefaultAsync(x => x.Id == id &&
                                              x.Converted == false &&
                                              x.EntType == EntType.OtherName.ToString(), cancellation);

                if (otherName is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
                }
                otherName.EntType = EntityTypes.Vendor.ToString();
                otherName.Converted = true;
                otherName.Vendor = vendor;
                await context.SaveChangesAsync(cancellation);
                return Result.Success(new string("Name was converted to vendor"));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}."));
            }
        }

        public async Task<Result<string>> ConvertToEmployeeAsync(string? id, Employee employee, CancellationToken cancellation = default)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);
            try
            {
                var otherName = await context.EntityNames
                                            .FirstOrDefaultAsync(x => x.Id == id &&
                                              x.Converted == false &&
                                              x.EntType == EntType.OtherName.ToString());

                if (otherName is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
                }

                otherName.EntType = EntityTypes.Employee.ToString();
                otherName.Converted = true;
                otherName.Employee = employee;
                await context.SaveChangesAsync();
                return Result.Success(new string("Name was converted to vendor"));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}."));
            }
        }

        public async Task<IEnumerable<T>> GetNameLists<T>(string? nameId, QueryParam param) where T : class, new()
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            IQueryable<NameListVm> queries = context.Database
                .SqlQuery<NameListVm>($@"SELECT * FROM VNameList")
                .WhereIf(!string.IsNullOrEmpty(param.Filter), x =>
                    EF.Functions.Like(x.FirstName!, $"%{param.Filter}%") ||
                    EF.Functions.Like(x.LastName!, $"%{param.Filter}%") ||
                    EF.Functions.Like(x.AltName!, $"%{param.Filter}%"))
                .WhereIf(!string.IsNullOrEmpty(nameId), x => x.Id.Contains(nameId!));

            if (param.PageSize > 0)
            {
                queries = queries.Take(param.PageSize).OrderByDescending(x => x.FirstName)
                                                        .ThenByDescending(x => x.LastName);
            }

            var items = await queries
                .ProjectToType<T>()
                .AsNoTracking()
                .ToListAsync();

            return items;
        }



        public async Task<Pager<T>> GetNameAsync<T>(string? nameId, QueryParam param, CancellationToken cancellation = default) where T : class, new()
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var queries = context.VNameLists
                .WhereIf(!string.IsNullOrEmpty(param.Filter), x =>
                    EF.Functions.Like(x.FullName!, $"%{param.Filter}%") ||
                    EF.Functions.Like(x.AltName!, $"%{param.Filter}%") ||
                    EF.Functions.Like(x.LastName!, $"%{param.Filter}%") ||
                    EF.Functions.Like(x.Phone!, $"%{param.Filter}%") ||
                    EF.Functions.Like(x.FirstName!, $"%{param.Filter}%"))
                .WhereIf(!string.IsNullOrEmpty(nameId), x => x.Id.Contains(nameId!));

            var counts = await queries.CountAsync(cancellation);

            if (param.PageSize > 0)
            {
                queries = queries.Take(param.PageSize).OrderByDescending(x => x.FirstName)
                                                        .ThenByDescending(x => x.LastName);
            }

            var items = await queries
                .ProjectToType<T>()
                .AsNoTracking()
                .ToListAsync(cancellation);

            return new Pager<T>(items, counts);
        }





        public async Task<Result<string>> UpdateStatusAsync(string? othernameId)
        {
            try
            {

                await using var context = await _dbContextFactory.CreateDbContextAsync();

                var result = await context.EntityNames.AsNoTracking().FirstOrDefaultAsync(x => x.Id == othernameId);

                result!.IsActive = result.IsActive == true ? false : true;

                context.EntityNames.Update(result);

                await context.SaveChangesAsync();

                return Result.Success("Other Name was updated successfully!");
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }


        public async Task<Result<VOtherNames>> UpdateStatusAsync(string? othernameId, bool state, CancellationToken cancellation = default)
        {
            try
            {
                if (string.IsNullOrEmpty(othernameId))
                {

                    return Result.Failure<VOtherNames>(ErrorResponse.Validation($"Invalid Guarantor ID#{othernameId}"));
                }

                await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

                var existing = await context.EntityNames.FindAsync(othernameId, cancellation);

                if (existing is null)
                {
                    return Result.Failure<VOtherNames>(ErrorResponse.NotFound(Messages.DataNotFound));
                }

                existing.IsActive = state;

                await context.SaveChangesAsync(cancellation);

                var updateGuarant = await context.VOtherNames
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.Id == othernameId, cancellation);

                return Result.Success<VOtherNames>(updateGuarant!);
            }
            catch (Exception ex)
            {
                return Result.Failure<VOtherNames>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }
    }
}


//public async Task<IEnumerable<EntityNames>> GetNamesAsync(string? searchTerm, string sortField = "AltName",
//    string sortOrder = "ASC", bool isActive = false)
//{
//    IQueryable<EntityNames> entities =
//        _context.EntityNames.Where(x => x.EntType == "Other Name").SortBy(sortField, sortOrder);

//    if (isActive == false)
//    {
//        entities = entities.Where(x => x.IsActive == true);
//    }

//    if (!string.IsNullOrWhiteSpace(searchTerm))
//    {
//        entities = entities.Where(x =>
//                x.FirstName!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
//                x.Name!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
//                x.AltName!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
//                x.MiddleName!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
//                x.LastName!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
//                x.Gender!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
//                x.Phone!.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase))
//            .SortBy(sortField, sortOrder);
//    }

//    var items = await entities.AsNoTracking().ToListAsync() ?? [];

//    return items;
//}
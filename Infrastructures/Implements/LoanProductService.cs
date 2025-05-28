using Applications.Extensions;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Products;
using Domains.Models.Loans;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class LoanProductService : ILoanProductService
    {
        private readonly DataContext _context;
        private readonly IDbContextFactory<DataContext> contextFactory;
        public LoanProductService(DataContext context, IDbContextFactory<DataContext> contextFactory)
        {
            this._context = context;
            this.contextFactory = contextFactory;
        }

        public async Task<Result<string>> CreateAsync(LoanProduct model)
        {
            try
            {
                _context.LoanProducts.Add(model);
                await SaveAsync();
                return Result.Success("Loan Product was Added");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));

            }
        }

        public async Task<Result<string>> DeleteAsync(int? id)
        {
            try
            {
                var result = await _context.LoanProducts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.LoanProductId == id);
                if (result is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("Loan Product is empty"));

                }

                _context.LoanProducts.Remove(result);
                await SaveAsync();

                return Result.Success("Loan Product was updated");

            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));

            }
        }

        public async Task<Result<string>> UpdateAsync(int? id, bool state)
        {
            try
            {
                var existing = await _context.LoanProducts.FindAsync(id);

                if (existing is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("Loan Product is empty"));
                }

                int editSeq = existing.EditSeq + 1;
                
                _context.LoanProducts.Where(x => x.LoanProductId == id && x.EditSeq == existing.EditSeq)
                    .ExecuteUpdate(props => props.SetProperty(x => x.IsActive, state)
                                                 .SetProperty(x => x.EditSeq, editSeq)
                                                 .SetProperty(x => x.ModifiedDt, DateTime.Now));

                return Result.Success("Loan Product was updated");

            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));

            }
        }

        public async Task<Result<string>> UpdateAsync(LoanProduct model)
        {
            try
            {
                var result = await _context.LoanProducts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(context => context.LoanProductId == model.LoanProductId && context.EditSeq == model.EditSeq);

                if (result is null) return Result.Failure<string>(ErrorResponse.NotFound("Loan Product is empty"));
                _context.LoanProducts.Update(model);
                await SaveAsync();
                return Result.Success("Loan product was modified");
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));

            }
        }

        public async Task<Result<IEnumerable<LoanProduct>>> GetAllAsyc(string? searches = null)
        {
            IQueryable<LoanProduct> results = _context.LoanProducts.AsNoTracking().Where(x => x.IsActive == true);

            if (searches != null)
            {
                results = results.Where(context => EF.Functions.Like(context.LoanProductName, $"%{searches}%") ||
                                                   EF.Functions.Like(context.LoanProductCode, $"%{searches}%"));
            }

            var data = await results.ToListAsync() ?? Enumerable.Empty<LoanProduct>();

            return Result.Success(data);
        }

        public async Task<Pager<T>> GetLoanProductsAsync<T>(QueryParam param) where T : class
        {

            IQueryable<VlProductList> queries = _context.Database.SqlQuery<VlProductList>($@"SELECT * FROM VLProductList");

            if (param.IsIncludeInActive == false)
            {
                queries = queries.Where(x => x.IsActive == true);
            }

            if (string.IsNullOrEmpty(param.Filter))
            {
                queries = queries.Where(e => EF.Functions.Like(e.LoanProductName, $"%{param.Filter}%") ||
                                             EF.Functions.Like(e.LoanProductCode, $"%{param.Filter}%"));
            }

            if (param.SortDescending)
            {
                queries = queries.OrderByDescending(p => EF.Property<object>(p, param.SortColumn));
            }

            var counts = queries.Count();

            if (param.PageSize > 0)
            {
                queries = queries.Skip(param.Skip).Take(param.PageSize);
            }

            var items = await queries.Skip(param.Skip)
                                        .Take(param.PageSize)
                                        .ProjectToType<T>()
                                        .AsNoTracking()
                                        .ToListAsync();

            return new Pager<T>(items, counts);
        }


        public async Task<Pager<VlProductList>> GetLoanProductsAsync(QueryParam param)
        {

            await using var context = await contextFactory.CreateDbContextAsync();

            IQueryable<VlProductList> queries = context.Database
                                                        .SqlQuery<VlProductList>($@"SELECT * FROM VLProductList")
                                                        .WhereIf(!param.IsIncludeInActive, x => x.IsActive == true)
                                                        .WhereIf(!string.IsNullOrEmpty(param.Filter),e=> EF.Functions.Like(e.LoanProductName, $"%{param.Filter}%") ||
                                                                                                         EF.Functions.Like(e.LoanProductCode, $"%{param.Filter}%"));
            if (param.SortDescending)
            {
                queries = queries.OrderByDescending(queries => EF.Property<object>(queries, param.SortColumn));
            }

            var counts = queries.Count();

            if (param.PageSize > 0)
            {
                queries = queries.Skip(param.Skip).Take(param.PageSize);
            }

            var items = await queries.AsNoTracking()
                                     .ToListAsync();

            return new Pager<VlProductList>(items, counts);
        }






        public async Task<Result<LoanProduct>> GetByFieldAsync(string? fields = null)
        {
            try
            {
                fields ??= string.Empty;

                var result = await _context.LoanProducts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.LoanProductName!.Contains(fields) ||
                                              c.LoanProductCode!.Contains(fields));
                if (result is null)
                {
                    return Result.Failure<LoanProduct>(ErrorResponse.NotFound("No Data Found"));
                }

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure<LoanProduct>(ErrorResponse.ServerError(ex.Message));
            }
        }

        public async Task<Result<LoanProduct>> GetByIdAsync(int? id)
        {
            try
            {
                var result = await _context.LoanProducts
                                            .Include(x => x.Currency)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(p => p.LoanProductId == id);

                if (result is null)
                {
                    return Result.Failure<LoanProduct>(ErrorResponse.NotFound("No Data Found"));
                }

                return Result.Success(result);

            }
            catch (Exception ex)
            {
                return Result.Failure<LoanProduct>(ErrorResponse.ServerError(ex.Message));

            }
        }

        public async Task<Result<T>> GetByIdAsync<T>(int? id) where T : class
        {
            try
            {
                var result = await _context.Database.SqlQuery<VlProductList>($"SELECT * FROM VLProductList WHERE LoanProductId={id}")
                                            .ProjectToType<T>()
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();
                if (result is null)
                {
                    return Result.Failure<T>(ErrorResponse.NotFound("No Data Found"));
                }

                return Result.Success(result);

            }
            catch (Exception ex)
            {
                return Result.Failure<T>(ErrorResponse.ServerError(ex.Message));

            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

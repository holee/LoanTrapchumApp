using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Products;
using Domains.Models.Loans;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreBussiness.Implements
{
    public class LoanProductService : ILoanProductServiceImpl
    {
        private readonly ApplicationDbContext _context;

        public LoanProductService(ApplicationDbContext context)
        {
            this._context = context;
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

        public async Task<Result<string>> DeleteAsync(int id)
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

        public async Task<Result<IEnumerable<LoanProduct>>> GetAllAsyc(string? searches = null)
        {
            IQueryable<LoanProduct> results = _context.LoanProducts.AsNoTracking().Where(x => x.IsActive == true);

            if (searches != null)
            {
                results = results.Where(context => EF.Functions.Like(context.LoanProductName, $"{searches}")
                  ||
                   EF.Functions.Like(context.LoanProductCode, $"{searches}"));
            }

            var data = await results.ToListAsync() ?? Enumerable.Empty<LoanProduct>();

            return Result.Success(data);
        }

        public async Task<Result<IEnumerable<VlProductList>>> GetAllListAsyc(bool includeinactive = false, string? searches = null)
        {
            IQueryable<VlProductList> results = _context.Database.SqlQuery<VlProductList>($@"SELECT * FROM VLProductList");

            if (includeinactive == false) results = results.Where(x => x.IsActive == true);

            if (searches != null)
            {
                results = results.Where(context => EF.Functions.Like(context.LoanProductName, $"{searches}") ||
                                                   EF.Functions.Like(context.LoanProductCode, $"{searches}"));
            }
            var data = await results.AsNoTracking().ToListAsync();

            return Result.Success<IEnumerable<VlProductList>>(data);
        }

        public async Task<PaginatedList<VlProductList>> GetAllPagingAsyc(QueryParam param, bool isActive = false, string? searches = null)
        {
            IQueryable<VlProductList> results = _context.Database.SqlQuery<VlProductList>($@"SELECT * FROM VLProductList");
            if (isActive == false)
                results = results.Where(x => x.IsActive == true);
            if (searches != null)
            {
                results = results.Where(context => EF.Functions.Like(context.LoanProductName, $"{searches}")
               ||
                EF.Functions.Like(context.LoanProductCode, $"{searches}"));
            }
            //results = results.OrderByDynamic(param.SortField, param.SortDescending.ToUpper());
            return await PaginatedList<VlProductList>.CreateAsync(results, param.PageIndex, param.PageSize);
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

        public async Task<Result<LoanProduct>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.LoanProducts.Include(x => x.Currency).AsNoTracking().FirstOrDefaultAsync(context => context.LoanProductId == id);

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

        public async Task<Result<string>> ModifyAsync(LoanProduct model)
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

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

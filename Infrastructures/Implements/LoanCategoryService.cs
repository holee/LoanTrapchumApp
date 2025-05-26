using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Categories;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Categories;
using Applications.Responses.ViewModels.Options;
using Domains.Models.Loans;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class LoanCategoryService(DataContext context) : ILoanCategoryService
    {


        public async Task<Result<VlCategoryList>> GetCategoryAsync(int? id)
        {
            var item = await context.Database
                            .SqlQuery<VlCategoryList>($@"SELECT * FROM VLCategoryList WHERE  LoanCategoryId={id}")
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
            if (item is null)
            {
                return Result.Failure<VlCategoryList>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            return Result.Success(item);
        }


        public async Task<Pager<VlCategoryList>> GetCategoriesAsync(QueryParam param)
        {
            IQueryable<VlCategoryList> query = context.Database.SqlQuery<VlCategoryList>($@"SELECT * FROM VLCategoryList");

            if (param.IsIncludeInActive == false)
            {
                query = query.Where(x => x.IsActive == true);
            }

            if (!string.IsNullOrEmpty(param.Filter))
            {
                query = query.Where(x => EF.Functions.Like(x.LoancategoryCode!, $"%{param.Filter}%") ||
                                         EF.Functions.Like(x.Code!, $"%{param.Filter}%") ||
                                         EF.Functions.Like(x.DesInKhmer!, $"%{param.Filter}%") ||
                                         EF.Functions.Like(x.Description!, $"%{param.Filter}%"));
            }

            if (param.PageSize > 0)
            {
                query = query.Skip((param.PageNumber) * param.PageSize).Take(param.PageSize);
            }

            if (param.SortDescending)
            {
                query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
            }

            var counts = query.Count();

            var items = await query.AsNoTracking().ToListAsync();

            return new Pager<VlCategoryList>(items, counts);
        }

        public async Task<Pager<LoanCategory>> GetCategoriesAsync(string? q = null)
        {
            IQueryable<LoanCategory> query = context.LoanCategories;

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => EF.Functions.Like(x.LoanCategoryCode!, $"%{q}%") ||
                                        EF.Functions.Like(x.Code!, $"%{q}%"));
            }
            var counts = query.Count();
            var items = await query.AsNoTracking().ToListAsync();
            return new Pager<LoanCategory>(items, counts);
        }

        public async Task<Result<LoanCategory>> GetLoanCategoryByIdAsync(int? id)
        {
            var item = await context.LoanCategories.FindAsync(id);

            if (item is null)
            {
                return Result.Failure<LoanCategory>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            return Result.Success(item);
        }


        public async Task<Result<LoanCategoryDtoUpdate>> GetEditCategoryByIdAsync(int? id) 
        {
            var item = await context.LoanCategories.FindAsync(id);

            if (item is null)
            {
                return Result.Failure<LoanCategoryDtoUpdate>(ErrorResponse.NotFound(Messages.DataNotFound));
            }
            
            var mapper=item.Adapt<LoanCategoryDtoUpdate>();

            return Result.Success(mapper);
        }


        public async Task<Result<string>> CreateAsync(LoanCategory category)
        {
            try
            {
                if (await NameExist(category.LoanCategoryCode))
                {
                    return Result.Failure<string>(ErrorResponse.Conflict(Messages.DataExist));
                }
                if (await CodeExist(category.Code))
                {
                    return Result.Failure<string>(ErrorResponse.Conflict("Loan code is already exist!"));
                }
                context.LoanCategories.Add(category);
                await context.SaveChangesAsync();
                return Result.Success(Messages.DataCreated);
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
            }

        }

        public async Task<Result<string>> UpdateAsync(LoanCategory cat)
        {
            try
            {

                var category = await context.LoanCategories.AsNoTracking()
                                        .Where(x => x.LoanCategoryId == cat.LoanCategoryId
                                                && x.EditSeq == cat.EditSeq).FirstOrDefaultAsync();

                if (category is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
                }

                context.LoanCategories.Update(cat);
                await context.SaveChangesAsync();
                return Result.Success(Messages.DataUpdated);
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

                var category = await context.LoanCategories.AsNoTracking().Where(x => x.LoanCategoryId == id).FirstOrDefaultAsync();

                if (category is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
                }

                context.LoanCategories.Remove(category);

                await context.SaveChangesAsync();

                return Result.Success(Messages.DataDeleted);
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));

            }
        }


        public async Task<bool> NameExist(string? name)
        {
            if (await context.LoanCategories.AnyAsync(x => x.LoanCategoryCode!.ToLower().Equals(name!.ToLower())))
                return true;
            return false;
        }

        public async Task<bool> CodeExist(string? code)
        {
            if (await context.LoanCategories.AnyAsync(x => x.Code!.ToLower().Equals(code!.ToLower())))
                return true;
            return false;
        }

        public async Task<Result<string>> UpdateStatus(int? id)
        {
            try
            {

                var result = await context.LoanCategories.AsNoTracking()
                                                    .IgnoreQueryFilters()
                                                    .FirstOrDefaultAsync(emp => emp.LoanCategoryId == id);
                if (result == null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("No Loan category found."));
                }
                result.EditSeq = result.EditSeq + 1;
                result.IsActive = result.IsActive!.Value ? false : true;
                result.ModifiedDt = DateTime.Now;
                context.LoanCategories.Update(result);
                var rowEffect = await context.SaveChangesAsync();
                if (rowEffect > 0)
                {
                    return Result.Success<string>(Messages.DataUpdated);
                }

                return Result.Failure<string>(ErrorResponse.ServerError("Cannot update category"));

            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
            }
        }

    }
}


//public async Task<IEnumerable<VlCategoryList>> GetListCategories(bool includeinactive = false, string? q = null)
//{
//    IQueryable<VlCategoryList> query = context.Database.SqlQuery<VlCategoryList>($@"SELECT * FROM VLCategoryList");
//    if (includeinactive == false)
//        query = query.Where(x => x.IsActive == true);
//    if (!string.IsNullOrEmpty(q))
//    {
//        query = query.Where(x => x.LoancategoryCode != null && x.LoancategoryCode.StartsWith(q) ||
//                          x.Code != null && x.Code.StartsWith(q));
//    }
//    return await query.AsNoTracking().ToListAsync();
//}

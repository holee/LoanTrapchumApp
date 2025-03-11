using Applications.Extensions.Identity;
using Applications.Interfaces.setting;
using Applications.Responses;
using Domains.Models.Loans;
using Domains.Models.Settings;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreBussiness.Implements
{
    public class SettingService(ApplicationDbContext context) : ISettingService
    {
        public async Task<Result<string>> Delete(int id)
        {
            var setting = await context.AppSettings.FindAsync(id);

            if (setting is null) Result.Failure(ErrorResponse.NotFound("Not Found"));

            context.AppSettings.Remove(setting!);

            await context.SaveChangesAsync();

            return Result.Success("Setting was Deleted");
        }

        public async Task<Result<string>> Save(SysSettings entity)
        {
            context.AppSettings.Add(entity);

            await context.SaveChangesAsync();

            return Result.Success("New Setting was added.");

        }

        public async Task<Result<string>> Update(SysSettings entity)
        {
            context.AppSettings.Update(entity);

            await context.SaveChangesAsync();

            return Result.Success("New Setting was added.");
        }

        public async Task<Result<SysSettings>> GetSysSettingByCode(string code)
        {
            var setting = await context.AppSettings.Where(x => x.Code == code)
                                            .FirstOrDefaultAsync();
            return setting is null
               ? Result.Failure<SysSettings>(ErrorResponse.NotFound("Not Found"))
               : Result.Success(setting);
        }

        public async Task<Result<SysSettings>> GetSysSettingByCode(int id)
        {
            var setting = await context.AppSettings.Where(x => x.Id == id)
                                                    .FirstOrDefaultAsync();
            return setting is null
                ? Result.Failure<SysSettings>(ErrorResponse.NotFound("Not Found"))
                : Result.Success(setting);
        }

        public async Task<Result<IEnumerable<SysSettings>>> GetSysSettings(string? filterValue = null)
        {
            IQueryable<SysSettings> queries = context.AppSettings;

            if (!string.IsNullOrEmpty(filterValue))
            {
                queries = queries.Where(x => EF.Functions.Like(x.Value!, $"{filterValue}") ||
                                         EF.Functions.Like(x.Code, $"{filterValue}") ||
                                         EF.Functions.Like(x.Id.ToString(), $"{filterValue}"));
            }

            var result = await queries.AsNoTracking().ToListAsync();

            return Result.Success<IEnumerable<SysSettings>>(result);

        }


        public async Task<Result<IEnumerable<AprovePerson>>> GetAproveUserAsync(int branchId, string? filterValue = null)
        {
            IQueryable<AprovePerson> queries = context.Database.SqlQuery<AprovePerson>($"SELECT * FROM VAproveLevels");


            if (!string.IsNullOrEmpty(filterValue))
            {
                queries = queries.Where(x => EF.Functions.Like(x.UserName!, $"%{filterValue}%") ||
                                        EF.Functions.Like(x.FullName!, $"%{filterValue}%") ||
                                         EF.Functions.Like(x.BranchName, $"%{filterValue}%"));
            }

            if (branchId > 0)
            {
                queries = queries.Where(b => b.BranchId == branchId);
            }



            var result = await queries.AsNoTracking().ToListAsync();

            return Result.Success<IEnumerable<AprovePerson>>(result);

        }




        public async Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSize model)
        {
            try
            {
                context.UserLoanSizes.Add(model);

                await context.SaveChangesAsync();

                return Result.Success<string>(new("Apply success"));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }


        }

        public async Task<Result<string>> DeActivateUserLoanSizeAsync(long? id)
        {
            try
            {

                var updateRow = await context.UserLoanSizes.Where(x => x.Id == id)
                                         .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.IsActive, false)
                                                                         .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                                                                         .SetProperty(p => p.EditSeq, 1));

                return updateRow > 0 ? Result.Success<string>(new("Apply success"))
                                     : Result.Failure<string>(ErrorResponse.ServerError("Cannot changed."));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }


        }


    }
}

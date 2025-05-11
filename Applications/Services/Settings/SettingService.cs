using Applications.Interfaces.setting;
using Applications.Responses;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Applications.Services.Settings
{


    public sealed class SettingService(IAppSettingService sSeting) : ISettingServiceImpl
    {
        public async Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSize model)
        {
            return await sSeting.ApplyUserLoanSizeAsync(model);
        }


        public async Task<Result<string>> DeActivateUserLoanSizeAsync(Guid? id)
        {
            return await sSeting.DeActivateUserLoanSizeAsync(id);
        }



        public async Task<Result<string>> Delete(Guid? id)
        {
            return await sSeting.Delete(id);
        }

        // public async Task<Result<IEnumerable<AprovePerson>>> GetAproveUserAsync(int? branchId, string? filterValue = null)
        // {
        //     return await sSeting.GetAproveUserAsync(branchId, filterValue);
        // }

        public async Task<Result<AppSettings>> GetSysSettingByCode(string code)
        {
            return await sSeting.GetSysSettingByCode(code);
        }


        public async Task<Result<AppSettings>> GetSysSettingByCode(int? id)
        {
            return await sSeting.GetSysSettingByCode(id);
        }

        public async Task<Result<IEnumerable<AppSettings>>> GetSysSettings(string? filterValue = null)
        {
            return await sSeting.GetSysSettings(filterValue);
        }

        public async Task<Result<IEnumerable<T>>> GetSysSettings<T>(string? filterValue = null)
        where T:class
        {
            return await sSeting.GetSysSettings<T>(filterValue);
        }
        
        public async Task<Result<string>> Save(AppSettings entity)
        {
           return await sSeting.Save(entity);
        }

        public async Task<Result<string>> Update(AppSettings entity)
        {
            return await sSeting.Update(entity);
        }
    }
}

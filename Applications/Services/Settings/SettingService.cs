using Applications.Interfaces.setting;
using Applications.Responses;
using Domains.Models.Identity;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Applications.Services.Settings
{
    public sealed class SettingService(ISettingService sSeting) : ISettingService
    {
        public async Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSize model)
        {
            return await sSeting.ApplyUserLoanSizeAsync(model);
        }

        public async Task<Result<string>> DeActivateUserLoanSizeAsync(long? id)
        {
            return await sSeting.DeActivateUserLoanSizeAsync(id);
        }

        public async Task<Result<string>> Delete(int id)
        {
            return await sSeting.Delete(id);
        }

        public async Task<Result<IEnumerable<AprovePerson>>> GetAproveUserAsync(int branchId, string? filterValue = null)
        {
            return await sSeting.GetAproveUserAsync(branchId, filterValue);
        }

        public async Task<Result<SysSettings>> GetSysSettingByCode(string code)
        {
            return await sSeting.GetSysSettingByCode(code);
        }

        public async Task<Result<SysSettings>> GetSysSettingByCode(int id)
        {
            return await sSeting.GetSysSettingByCode(id);
        }

        public async Task<Result<IEnumerable<SysSettings>>> GetSysSettings(string? filterValue = null)
        {
            return await sSeting.GetSysSettings(filterValue);
        }

        public async Task<Result<string>> Save(SysSettings entity)
        {
           return await sSeting.Save(entity);
        }

        public async Task<Result<string>> Update(SysSettings entity)
        {
            return await sSeting.Update(entity);
        }
    }
}

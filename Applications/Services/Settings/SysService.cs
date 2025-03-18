using Applications.Interfaces.setting;
using Applications.Responses;
using Domains.Models.Identity;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Applications.Services.Settings
{
    public sealed class SysService : ISettingService
    {
        public Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSize model)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> DeActivateUserLoanSizeAsync(long? id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<AprovePerson>>> GetAproveUserAsync(int branchId, string? filterValue = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<SysSettings>> GetSysSettingByCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task<Result<SysSettings>> GetSysSettingByCode(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<SysSettings>>> GetSysSettings(string? filterValue = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> Save(SysSettings entity)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> Update(SysSettings entity)
        {
            throw new NotImplementedException();
        }
    }
}

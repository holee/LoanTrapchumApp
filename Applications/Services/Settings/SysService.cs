using Applications.Interfaces.setting;
using Applications.Responses;
using Domains.Models.Identity;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Applications.Services.Settings
{
    public interface ISettingServiceImpl
    {
        Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSize model);
        Task<Result<string>> DeActivateUserLoanSizeAsync(Guid? id);
        Task<Result<string>> Delete(Guid? id);
        // Task<Result<IEnumerable<AprovePerson>>> GetAproveUserAsync(int? branchId, string? filterValue = null);
        Task<Result<SysSettings>> GetSysSettingByCode(int? id);
        Task<Result<SysSettings>> GetSysSettingByCode(string code);
        Task<Result<IEnumerable<SysSettings>>> GetSysSettings(string? filterValue = null);
        Task<Result<string>> Save(SysSettings entity);
        Task<Result<string>> Update(SysSettings entity);
    }

    public sealed class SysService(ISettingService setting) : ISettingServiceImpl
    {
        public async Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSize model)
        {
            return await setting.ApplyUserLoanSizeAsync(model);
        }

        public Task<Result<string>> DeActivateUserLoanSizeAsync(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> Delete(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<AprovePerson>>> GetAproveUserAsync(int branchId, string? filterValue = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<AprovePerson>>> GetAproveUserAsync(int? branchId, string? filterValue = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<SysSettings>> GetSysSettingByCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task<Result<SysSettings>> GetSysSettingByCode(int? id)
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

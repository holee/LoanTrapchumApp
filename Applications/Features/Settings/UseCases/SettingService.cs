using Applications.Commons.Mappers;
using Applications.Extensions;
using Applications.Features.Settings.Requests;
using Applications.Interfaces.setting;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Settings;
using Domains.Models.Collaterals;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Applications.Features.Settings.UseCases
{

 
    public sealed class SettingService(IAppSettingService sSeting) : ISettingServiceImpl
    {
        public async Task<Result<string>> CreateUserLoanSizeAsync(UserLoanSizeDtoCreate req)
        {

            var model = req.MapToCreate();

            return await sSeting.CreateUserLoanSizeAsync(model);
        }


        public async Task<Result<string>> EditUserLoanSizeAsync(UserLoanSizeDtoCreate req, CancellationToken cancellation = default)
        {

            var model = req.MapToEdit();

            return await sSeting.EditUserLoanSizeAsync(model, cancellation);
        }

        public async Task<Result<VUserLoanSizes>> GetLoanSizeByUserId(string? userRefId, CancellationToken cancellation = default)
        {
            if (userRefId.IsNotNullOrEmpty())
            {
                return Result.Failure<VUserLoanSizes>(ErrorResponse.Validation("Please input user id.."));
            }

            return await sSeting.GetLoanSizeByUserId(userRefId, cancellation);
        }


        public async Task<Result<UserLoanSizeDtoCreate>> GetEditLoanSizeByUserId(long? id, string? userId, CancellationToken cancellation = default)
        {
            if (id <= 0)
            {
                return Result.Failure<UserLoanSizeDtoCreate>(ErrorResponse.Validation("Invalid Id....."));
            }

            var rsp = await sSeting.GetLoanSizeById(id, userId, cancellation);


            if (rsp.IsSuccess && rsp.Data is not null)
            {
                var editModel = rsp.Data.MapToEdit();

                return Result.Success(editModel);
            }

            return Result.Failure<UserLoanSizeDtoCreate>(ErrorResponse.NotFound(rsp.ErrMessage));
        }



        public async Task<Pager<VUserLoanSizes>> GetUserLoanSizes(string? userRefId, QueryParam q, CancellationToken cancellation = default)
        {
            return await sSeting.GetUserLoanSizes(userRefId, q, cancellation);
        }

        public async Task<Result<string>> DeActivateUserLoanSizeAsync(long? id, string userRefId, CancellationToken cancellation = default)
        {
            return await sSeting.DeActivateUserLoanSizeAsync(id, userRefId, cancellation);
        }



        public async Task<Result<string>> Delete(long? id)
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
        where T : class
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

        public async Task<Pager<CollateralSubType>> GetColleralSubTypes(QueryParam param, CancellationToken cancellation = default)
        {
            return await sSeting.GetCollateralSubTypes(param, cancellation);
        }


    }
}

//using Applications.Interfaces.Loans;
//using Applications.Responses.Auths;
//using Applications.Responses.ViewModels.Settings;
//using Domains.Models.Loans;

//namespace Applications.Services.Loans
//{
//    public sealed class LoanSettingService(ILoanSettingService service) : ILoanSettingServiceImpl
//    {
//        public async Task<IEnumerable<AprovePerson>> GetAprovedUserAsync(int? branchId, string? filter = null)
//        {
//            return await service.GetAprovedUserAsync(branchId, filter);
//        }

//        public async Task<IEnumerable<VUserLoanSizes>> GetAprovedUserAsync(int? branchId, int? currencyId, decimal? amount, string? filter = null)
//        {
//            return await service.GetAprovedUserAsync(branchId,currencyId,amount,filter);
//        }

//        public async Task<IEnumerable<LoanInstallmentType>> GetInstallmentTypes(string? q = null)
//        {
//            return await service.GetInstallmentTypes(q);
//        }

//        public async Task<IEnumerable<LoanCategory>> GetLoanCategories(string? q = null)
//        {
//            return await service.GetLoanCategories(q);
//        }

//        public async Task<IEnumerable<LoanRepaymentMode>> GetRepaymentModes(string? q = null)
//        {
//            return await service.GetRepaymentModes(q);
//        }

//        public async Task<IEnumerable<LoanSubTypeStatus>> GetSubSubTypeStatus(int? subTypeRefId, string? q = null)
//        {
//            return await service.GetSubSubTypeStatus(subTypeRefId, q);
//        }

//        public async Task<IEnumerable<LoanTypeStatus>> GetSubTypeStatus(string? q = null)
//        {
//           return await service.GetSubTypeStatus(q);
//        }
//    }



//}

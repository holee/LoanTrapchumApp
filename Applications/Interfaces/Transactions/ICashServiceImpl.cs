using Applications.Responses;
using Domains.Models.Transaction;

namespace Applications.Interfaces.Transactions
{
    public interface ICashServiceImpl
    {
        Task<CashStatusResult> CheckCashValueIsInOut(int branchRefId, DateTime bussinessDate);
        Task<IEnumerable<CashStatus>> GetsCashStatus(string? q = null);
    }
}

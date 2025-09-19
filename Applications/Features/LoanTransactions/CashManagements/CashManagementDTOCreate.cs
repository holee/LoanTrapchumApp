using Applications.Enumerations;
using Domains.Models.Currencies;

namespace Applications.Features.Transactions.CashManagements
{
    public sealed class CashManagementDtoCreate
    {
        public string? Id { get; set; } = string.Empty;
        public int? BankRefId { get; set; }
        public string? StaffRefId { get; set; }
        public int? CurrencyRefId { get; set; }
        public int? BranchRefId { get; set; }
        public long? TranRefId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }
        public int? CashStatusRefId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? BusinessDate { get; set; }
        public string? Reject { get; set; } = "N";
        public string? Receive { get; set; } = "Y";
        public string? Remark { get; set; }
        public decimal? TmpAmount { get; set; }
        public int? DelegateToStaffRefId { get; set; }
        public List<CashManagementDetailDtoCreate> CashManagementDetails { get; set; } = [];


        public static CashManagementDtoCreate Create(int? branchId, int? bankId, int? currencyId,
                    decimal? amount, decimal? balance, int? cashStatusId, string? remark,
                    List<CashManagementDetailDtoCreate> cashDetails)
        {
            return new CashManagementDtoCreate
            {
                Id = Guid.CreateVersion7().ToString(),
                BankRefId = bankId,
                CurrencyRefId = currencyId,
                BranchRefId = branchId,
                Amount = amount,
                Balance = balance,
                CashStatusRefId = cashStatusId,
                TransactionDate = DateTime.Now,
                BusinessDate = DateTime.Now,
                Remark = remark,
                CashManagementDetails = cashDetails,
            };
        }

    }
}

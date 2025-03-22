using Applications.Requests.Collaterals;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Loans;
using Applications.Responses.Loans.Disbursements;

namespace Applications.Responses.Loans.LoanRequests
{
    public sealed class LoanRequestResponse
    {
        public long Id { get; set; }
        public long? CustomerRefId { get; set; }
        public int? LoanRequestStatusRefId { get; set; }
        public int? BranchRefId { get; set; }
        public long? RequestUserRefId { get; set; }
        public int RepaymentModeRefId { get; set; }
        public int LoanSubTypeRefId { get; set; }
        public int LoanCategoryRefId { get; set; }
        public int BankRefId { get; set; }
        public int LoanProductRefId { get; set; }
        public long CreditAgentRefId { get; set; }
        public string? InstallmentName { get; set; }
        public int InstallmentTypeId { get; set; }
        public int PayEvery { get; set; } = 1;
        public int? CurrencyRefId { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? FirstPaymentDate { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public int? Installments { get; set; }
        public string? InstallmentType { get; set; }
        public string? RepaymentMode { get; set; }
        public decimal? Principle { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? CollectionFeeRate { get; set; }
        public int? Circle { get; set; } = 1;
        public long? AprovedUserId { get; set; }
        public DateTime? AprovedDate { get; set; }
        public decimal? AdminFeeRate { get; set; }
        public decimal? AdminFeeAmount { get; set; }
        public decimal? PenaltyRate { get; set; }
        public string? Remark { get; set; }
        public string? Memo { get; set; }
        public int? OtherChargId { get; set; } = 1;
        public string? OtherChargName { get; set; } = string.Empty;
        public ICollection<LoanDocumentsResponse>? LoanDocuments { get; set; } = [];
        public ICollection<OtherNameDto>? Guarantors { get; set; } = [];
        public ICollection<CollateralLineDto> CollateralLines { get; set; } = [];
    }
}

using Domains.Models.Bases;
using Domains.Models.Currencies;

namespace Application.Responses.Loans.Disbursements
{
    public sealed class LoanProductResponse : BaseEntity
    {
        public int LoanProductId { get; set; }
        public string? LoanProductCode { get; set; }
        public string? LoanProductName { get; set; }
        public string? LoanSize { get; set; }
        public int? CurrencyRefId { get; set; }
        public string? Installments { get; set; }
        public string? InstallmentType { get; set; }
        public string? InterestRate { get; set; }
        public string? RepaymentMode { get; set; }
        public string? OriginationFees { get; set; }
        public string? ArrearRate { get; set; }
        public int? LoanBflfolId { get; set; }
        public int? LoanAptId { get; set; }
        public bool? IsActive { get; set; }
        public Currency? Currency { get; set; }
    }
}

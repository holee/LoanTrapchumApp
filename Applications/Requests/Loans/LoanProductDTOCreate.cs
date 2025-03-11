using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Loans
{
    public class LoanProductDtoCreate
    {
        [Required(ErrorMessage ="Loan product code is required.",AllowEmptyStrings =false)]
        public string? LoanProductCode { get; set; }
        [Required(ErrorMessage = "Loan product name is required.", AllowEmptyStrings = false)]
        public string? LoanProductName { get; set; }
        public string? LoanSize { get; set; }
        [Required(ErrorMessage = "Currency is required.", AllowEmptyStrings = false)]
        public int? CurrencyRefId { get; set; }
        public string? Installments { get; set; }
        public string? InstallmentType { get; set; }
        public string? InterestRate { get; set; }
        public string? RepaymentMode { get; set; }
        public string? OriginationFees { get; set; }
        public string? ArrearRate { get; set; }
        public int? LoanBflfolId { get; set; }
        public int? LoanAptId { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}

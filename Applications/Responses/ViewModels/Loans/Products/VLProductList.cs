namespace Applications.Responses.ViewModels.Loans.Products
{
    public class VlProductList
    {
        public int LoanProductId { get; set; }
        public string? LoanProductCode { get; set; } = string.Empty;
        public string? LoanProductName { get; set; }=string.Empty;
        public string? LoanSize { get; set; }= string.Empty;
        public string? CurrencyCode { get; set; }
        public string? Installments { get; set; }
        public string? InstallmentType { get; set; }
        public string? InterestRate { get; set; }
        public string? RepaymentMode { get; set; }
        public string? OriginationFees { get; set; }
        public string? ArrearRate { get; set; }
        public int? EditSeq { get; set; }
        public bool IsActive { get; set; } = true;
        public string? IsInUse { get; set; }
    }
}

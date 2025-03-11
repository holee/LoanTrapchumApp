namespace Domains.Models.Loans
{
    public class LoanRepaymentMode
    {
        public int RepaymentModeId { get; set; }
        public string? RepaymentModeCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}

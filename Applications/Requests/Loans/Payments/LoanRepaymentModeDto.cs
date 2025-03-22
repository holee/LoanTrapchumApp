namespace Applications.Requests.Loans.Payments
{
    public class LoanRepaymentModeDto
    {
        public int RepaymentModeId { get; set; }
        public string? RepaymentModeCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}

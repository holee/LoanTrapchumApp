namespace Applications.Responses.Loans.Disbursements
{
    public sealed class LoanStatusResponse
    {
        public int LoanstatusId { get; set; }
        public string Loanstatuscode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

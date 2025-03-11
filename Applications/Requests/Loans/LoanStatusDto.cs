namespace Applications.Requests.Loans
{
    public class LoanStatusDto
    {
        public int LoanstatusId { get; set; }
        public string Loanstatuscode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

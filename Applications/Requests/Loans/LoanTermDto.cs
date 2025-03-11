namespace Applications.Requests.Loans
{
    public class LoanTermDto
    {
        public int LoanTermId { get; set; }
        public string? LoanTermCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}

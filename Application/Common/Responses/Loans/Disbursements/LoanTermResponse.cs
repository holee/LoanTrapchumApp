namespace Application.Responses.Loans.Disbursements
{
    public class LoanTermResponse
    {
        public int LoanTermId { get; set; }
        public string? LoanTermCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}

namespace Application.Features.Loans.Dtos.Requests
{
    public class LoanTermDto
    {
        public int LoanTermId { get; set; }
        public string? LoanTermCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}

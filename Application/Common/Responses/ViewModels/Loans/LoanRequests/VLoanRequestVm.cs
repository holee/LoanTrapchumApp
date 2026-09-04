namespace Application.Responses.ViewModels.Loans.LoanRequests
{
    public class VLoanRequestVm
    {
        public string? CustomerId { get; set; }
        public string? AltName { get; set; }
        public string? FullName { get; set; }
        public int NumberLoanRequest { get; set; } = 0;
        public bool IsActive { get; set; } = false;
    }
}

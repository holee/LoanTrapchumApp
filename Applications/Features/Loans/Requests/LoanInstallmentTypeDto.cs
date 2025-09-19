namespace Applications.Features.Loans.Requests
{
    public class LoanInstallmentTypeDto
    {
        public int InstallmentTypeId { get; set; }
        public string InstallmentTypeCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

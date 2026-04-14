namespace Applications.Features.Loans.Dtos.Requests
{
    public class LoanInstallmentTypeDto
    {
        public int InstallmentTypeId { get; set; }
        public string InstallmentTypeCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

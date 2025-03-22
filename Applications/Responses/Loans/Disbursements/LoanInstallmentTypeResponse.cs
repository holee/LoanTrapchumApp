namespace Applications.Responses.Loans.Disbursements
{
    public class LoanInstallmentTypeResponse
    {
        public int InstallmentTypeId { get; set; }
        public string InstallmentTypeCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

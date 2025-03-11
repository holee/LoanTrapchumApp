namespace Domains.Models.Loans;

public class LoanInstallmentType
{
    public int InstallmentTypeId { get; set; }
    public string InstallmentTypeCode { get; set; } = string.Empty;
    public string Description { get; set; }=string.Empty;   
}

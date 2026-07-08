using Humanizer;

namespace Applications.Responses.ViewModels.Loans.LoanRequests;

public sealed class VLoanRequestDescription 
{
    public int Id { get; set; }
    public string? LoanRequestRefId { get; set; }=string.Empty;
    public string Memo { get; set; } = string.Empty;
    public DateTime? CreatedDT { get; set; }
    public string? PretierDate => CreatedDT.Humanize(); 
    public string FullName { get; set; } = string.Empty;
}

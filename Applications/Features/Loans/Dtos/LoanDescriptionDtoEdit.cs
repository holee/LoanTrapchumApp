using Humanizer;

namespace Applications.Features.Loans.Dtos;

public sealed class LoanDescriptionDtoEdit 
{
    public int Id { get; set; }
    public string? LoanRequestRefId { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;
    public DateTime? CreatedDT { get; set; }
    public string? PretierDate => CreatedDT.Humanize();
    public string FullName { get; set; } = string.Empty;
}

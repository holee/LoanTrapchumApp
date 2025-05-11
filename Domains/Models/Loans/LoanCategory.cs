using Domains.Models.Bases;

namespace Domains.Models.Loans;

public sealed class LoanCategory:BaseEntity
{
    public int LoanCategoryId { get; set; }
    public string? LoanCategoryCode { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? DesInKhmer { get; set; }=string.Empty;
    public string? Code { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
}

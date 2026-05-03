namespace Applications.Responses.Loans.Disbursements;

public sealed class LoanCategoryDto 
{
    public int LoanCategoryId { get; set; }
    public string? LoanCategoryCode { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? DesInKhmer { get; set; } = string.Empty;
    public string? Code { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
    public int EditSeq { get; set; } = 0;
}

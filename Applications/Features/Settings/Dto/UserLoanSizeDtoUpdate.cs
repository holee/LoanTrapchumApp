namespace Applications.Features.Settings.Dto;

public sealed class UserLoanSizeDtoUpdate
{
    public long Id { get; set; }
    public string? UserRefId { get; set; }
    public decimal? MinUsdLoanSize { get; set; }
    public decimal? MaxUsdLoanSize { get; set; }
    public decimal? MinKhrLoanSize { get; set; }
    public decimal? MaxKhrLoanSize { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int EditSeq { get; set; }
}

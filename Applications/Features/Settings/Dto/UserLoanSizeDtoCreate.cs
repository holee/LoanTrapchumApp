namespace Applications.Features.Settings.Dto;

public sealed class UserLoanSizeDtoCreate
{
    public string? UserRefId { get; set; }
    public decimal? MinUsdLoanSize { get; set; } = 0.00M;
    public decimal? MaxUsdLoanSize { get; set; } = 0.00M;
    public decimal? MinKhrLoanSize { get; set; } = 0.00M;
    public decimal? MaxKhrLoanSize { get; set; } = 0.00M;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

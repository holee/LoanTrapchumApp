namespace Applications.Features.Settings.Dto;

public class UserLoanSizeDtoUpdate 
{
    public long Id { get; set; }
    public string? UserRefId { get; set; }
    public decimal? MinUsdLoanSize { get; set; } = 0.00M;
    public decimal? MaxUsdLoanSize { get; set; } = 0.00M;
    public decimal? MinKhrLoanSize { get; set; } = 0.00M;
    public decimal? MaxKhrLoanSize { get; set; } = 0.00M;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CreatedDt { get; set; }
    public DateTime? ModifiedDt { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int EditSeq { get; set; } = 0;
}

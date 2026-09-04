using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.Settings.Dto;

public sealed class VUserLoanSizes 
{
    public long? Id { get; set; }
    public string? UserRefId { get; set; }
    public string? FullName { get; set; }
    public int? BranchId { get; set; } 
    public string? BranchName { get; set; }
    public decimal? MinUsdLoanSize { get; set; }
    public decimal? MaxUsdLoanSize { get; set; }
    public decimal? MinKhrLoanSize { get; set; }
    public decimal? MaxKhrLoanSize { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int EditSeq { get; set; } = 0;
    public bool? IsDefault { get; set; } = false;
    [NotMapped]public int RowIndex { get; set; } = 0;
}

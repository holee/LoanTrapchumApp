using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Features.Loans.Dtos;

public sealed class LoanGuarantorDtoEdit
{
    public long Id { get; set; }
    public string? GuarantorId { get; set; }
    public string? LoanRequestRefId { get; set; }
    public int? BranchId { get; set; }
    public string? GuarantorName { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool? IsActive { get; set; } = false;
    [NotMapped] public int RowIndex { get; set; } = 0;
}
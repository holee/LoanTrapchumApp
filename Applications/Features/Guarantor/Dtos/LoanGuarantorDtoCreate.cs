namespace Applications.Features.Guarantor.Dtos;

public sealed class LoanGuarantorDtoCreate 
{
    public long? Id { get; set; } = 0;
    public int? BranchId { get; set; } 
    public string? LoanRequestRefId { get; set; }
    public string? EntityNameRefId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
}

namespace Applications.Requests.Lists.Entities.Guarantors;

public class LoanGuarantorDtoCreate 
{
    public long? Id { get; set; } = 0;
    public int? BranchId { get; set; } 
    public long? LoanRequestRefId { get; set; }
    public long? EntityNameRefId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
}

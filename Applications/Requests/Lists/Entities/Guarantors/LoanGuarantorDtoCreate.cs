namespace Applications.Requests.Lists.Entities.Guarantors;

public class LoanGuarantorDtoCreate 
{
    public long? Id { get; set; } = 0;
    public int? BranchId { get; set; } 
    public Guid? LoanRequestRefId { get; set; }
    public Guid? EntityNameRefId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
}

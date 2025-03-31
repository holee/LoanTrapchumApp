namespace Applications.Requests.Loans.LoanRequests;

public sealed class CollateralLinkDtoCreate
{
    public long? Id { get; set; }
    public int? BranchId { get; set; }
    public Guid? LoanRequestRefId { get; set; }
    public Guid? CollateralRefId { get; set; }
    public long? CollateralLineRefId { get; set; }
    public bool? IsActive { get; set; } = true;

}

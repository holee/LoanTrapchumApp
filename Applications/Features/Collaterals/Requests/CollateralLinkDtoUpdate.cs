namespace Applications.Features.Collaterals.Requests
{
    public sealed class CollateralLinkDtoUpdate 
    {
        public long Id { get; set; }
        public int? BranchId { get; set; }
        public long? CollateralLineRefId { get; set; }
        public string? LoanRequestRefId { get; set; }
        public string? CollateralRefId { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }
}

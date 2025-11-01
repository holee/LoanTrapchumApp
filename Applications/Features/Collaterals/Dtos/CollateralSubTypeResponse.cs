namespace Applications.Features.Collaterals.Dtos
{
    public sealed class CollateralSubTypeResponse
    {
        public int Id { get; set; }
        public string? CollateralSubTypeName { get; set; } 
        public string? CollateralSubTypeNameInKhmer { get; set; }
        public int? CollateralTypeRefId { get; set; }
        public decimal? ValueRate { get; set; }
        public decimal? LoanSizeAmuntMin { get; set; }
        public decimal? LoanSizeAmuntMax { get; set; } 
        public bool? IsActive { get; set; }
    }
}

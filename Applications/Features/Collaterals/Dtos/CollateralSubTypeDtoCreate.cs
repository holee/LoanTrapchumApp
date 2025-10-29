namespace Applications.Features.Collaterals.Dtos;

public sealed class CollateralSubTypeDtoCreate 
{
    public int Id { get; set; }
    public string? SubTypeName { get; set; }
    public string? SubTypeNameInKhmer { get; set; }
    public int? CollateralTypeRefId { get; set; }
    public decimal? ValueRate { get; set; }
    public decimal? LoanSizeAmuntMin { get; set; }
    public decimal? LoanSizeAmuntMax { get; set; }
    public bool? IsActive { get; set; }
}
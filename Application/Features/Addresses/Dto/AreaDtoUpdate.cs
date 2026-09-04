namespace Application.Features.Addresses.Dto;

public sealed class AreaDtoUpdate
{
    public long Id { get; set; }
    public string? AreaCode { get; set; } = string.Empty;
    public int? VillageId { get; set; }
    public int? CommuneId { get; set; }
    public int? DistrictId { get; set; }
    public int? ProvinceId { get; set; }
    public int? Mapping { get; set; }
    public int? BranchId { get; set; }
}
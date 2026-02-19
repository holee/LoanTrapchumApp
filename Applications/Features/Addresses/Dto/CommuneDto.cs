namespace Applications.Features.Addresses.Dto;

public sealed class CommuneDto
{
    public int? CommuneId { get; set; }
    public int? DistrictRefId { get; set; }
    public string? ComDescription { get; set; }
    public string? ComInKhmer { get; set; }
    public ICollection<VillageDto>? Villages { get; set; } = new List<VillageDto>();

}

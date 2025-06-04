namespace Applications.Requests.Lists.Addreses;

public class CommuneDto
{
    public int? CommuneId { get; set; }
    public int? DistrictRefId { get; set; }
    public string? ComDescription { get; set; }
    public string? ComInKhmer { get; set; }
    public ICollection<VillageDto>? Villages { get; set; }

}
public class CommuneDtoCreate
{
    public int? CommuneId { get; set; }
    public int? DistrictRefId { get; set; }
    public string? ComDescription { get; set; }
    public string? ComInKhmer { get; set; }

}
public class CommuneDtoUpdate
{
    public int? CommuneId { get; set; }
    public int? DistrictRefId { get; set; }
    public string? ComDescription { get; set; }
    public string? ComInKhmer { get; set; }

}

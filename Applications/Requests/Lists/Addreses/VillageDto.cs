namespace Applications.Requests.Lists.Addreses;

public class VillageDto
{
    public int? VillageId { get; set; }
    public int? CommuneRefId { get; set; }
    public string? VillDescription { get; set; }
    public string? VilInKhmer { get; set; }
}
public class VillageDtoCreate
{
    public int? VillageId { get; set; }
    public int? CommuneRefId { get; set; }
    public string? VillDescription { get; set; }
    public string? VilInKhmer { get; set; }
}
public class VillageDtoUpdate
{
    public int? VillageId { get; set; }
    public int? CommuneRefId { get; set; }
    public string? VillDescription { get; set; }
    public string? VilInKhmer { get; set; }
}
namespace Applications.Responses.Lists.Addreses; 

public class CommuneResponse
{
    public int? CommuneId { get; set; }
    public int? DistrictRefId { get; set; }
    public string? ComDescription { get; set; }
    public string? ComInKhmer { get; set; }
    public ICollection<VillageResponse>? Villages { get; set; }

}

namespace Applications.Responses.Lists.Addreses;

public class ProvinceResponse
{
    public int? ProvinceId { get; set; }
    public int? CountryId { get; set; }
    public string? ProvinceCode { get; set; }
    public string? ProDescription { get; set; }
    public string? ProInKhmer { get; set; }
    public ICollection<DistrictResponse>? Districts { get; set; }
}

namespace Applications.Requests.Lists.Addreses;

public class ProvinceDto
{
    public int? ProvinceId { get; set; }
    public int? CountryId { get; set; }
    public string? ProvinceCode { get; set; }
    public string? ProDescription { get; set; }
    public string? ProInKhmer { get; set; }
    public ICollection<DistrictDto>? Districts { get; set; }
}

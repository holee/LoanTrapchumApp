using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.Addresses.Dto;

public class ProvinceDto
{
    public int? ProvinceId { get; set; }
    public int? CountryId { get; set; }
    public string? ProvinceCode { get; set; }
    public string? ProDescription { get; set; }
    public string? ProInKhmer { get; set; }
    public ICollection<DistrictDto>? Districts { get; set; }
}
public class ProvinceDtoCreate
{
    
    public int? ProvinceId { get; set; }
    public string? ProvinceCode { get; set; }
    public string? ProDescription { get; set; }
    public string? ProInKhmer { get; set; }
    public int? CountryId { get; set; }
}
public class ProvinceDtoUpdate
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ProvinceId { get; set; }
    public string? ProvinceCode { get; set; }
    public string? ProDescription { get; set; }
    public string? ProInKhmer { get; set; }
    public int? CountryId { get; set; }
}
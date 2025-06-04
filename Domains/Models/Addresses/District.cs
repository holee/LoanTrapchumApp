using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Addresses;

public sealed class District
{

    [Key]
    public int? DistrictId { get; set; }
    public int? ProvinceRefId { get; set; }
    public string? DisDescription { get; set; }
    public string? DisInKhmer { get; set; }
}

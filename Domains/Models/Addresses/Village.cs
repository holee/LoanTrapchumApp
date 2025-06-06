using System.ComponentModel.DataAnnotations;

namespace Domains.Models.Addresses;

public class Village
{
    [Key]
    public int? VillageId { get; set; }
    public int? CommuneRefId { get; set; }

    public string? VillDescription { get; set; }
    public string? VilInKhmer { get; set; }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Addresses
{
    public class Commune
    {
        public int? CommuneId { get; set; }
        public string? ComDescription { get; set; }
        public string? ComInKhmer { get; set; }
        public int? DistrictRefId { get; set; }
    }
}

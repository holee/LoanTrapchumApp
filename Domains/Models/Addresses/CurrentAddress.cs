using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Addresses
{
    public class CurrentAddress 
    {
        public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? ProviceName { get; set; }=string.Empty;
        public string? ProviceKhmerName { get; set; } = string.Empty;
        public string? DistrictName { get; set; } = string.Empty;
        public string? DistrictKhmerName { get; set; } = string.Empty;
        public string? CommuneName { get; set; } = string.Empty;
        public string? CommuneKhmerName { get; set; } = string.Empty;
        public string? VillageName { get; set; } = string.Empty;
        public string? VillageKhmerName { get; set; } = string.Empty;
        [NotMapped]
        public string? FullKhmerAddress
        {
            get
            {
                return $"{ProviceKhmerName} {DistrictKhmerName} {CommuneKhmerName} {VillageKhmerName}";
            }
        }
        [NotMapped]
        public string? FullEngAddress
        {
            get
            {
                return $"{ProviceName} {DistrictName} {CommuneName} {VillageName}";
            }
        }
    }
}

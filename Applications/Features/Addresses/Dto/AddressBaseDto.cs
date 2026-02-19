using Domains.Models.Bases;

namespace Applications.Features.Addresses.Dto
{
    public abstract class AddressBaseDto: BaseEntity
    {
        public string? CountryName { get; set; }
        public string? City { get; set; }
        public int? ProvinceRefId { get; set; }
        public string? ProDescription { get; set; }
        public string? ProInKhmer { get; set; }
        public int? DistrictRefId { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
        public int? CommuneRefId { get; set; }
        public string? ComDescription { get; set; }
        public string? ComInKhmer { get; set; }
        public int? VillageRefId { get; set; }
        public string? VillDescription { get; set; }
        public string? VilInKhmer { get; set; }
    }
}

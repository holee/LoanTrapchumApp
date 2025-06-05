namespace Applications.Responses.ViewModels.Addresses
{
    public class VillageViewModel  
    {
        public int? VillageId { get; set; }
        public int? CommuneRefId { get; set; }
        public string? VilDescription { get; set; }
        public string? VilInKhmer { get; set; }
    }
    public class VVillages
    {
        public int? VillageId { get; set; }
        public string? VillDescription { get; set; }
        public string? VilInKhmer { get; set; }
        public string? ComDescription { get; set; }
        public string? ComInKhmer { get; set; }
    }
    public class VVillageDetails
    {
        public int? VillageId { get; set; }
        public string? VilDescription { get; set; }
        public string? VilInKhmer { get; set; }
        public string? ComDesctiption { get; set; }
        public string? ComInKhmer { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
        public string? ProvinceCode { get; set; }
        public string? ProDescription { get; set; }
        public string? ProInKhmer { get; set; }
        public string? Country { get; set; }
    }
}

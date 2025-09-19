namespace Applications.Features.Addresses.Responses
{
    public class DistrictDto
    {
        public int? DistrictId { get; set; }
        public int? ProvinceRefId { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
        public ICollection<CommuneDto>? Communes { get; set; } 
    }
    public class DistrictDtoCreate
    {
        public int? DistrictId { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
        public int? ProvinceRefId { get; set; }
    }
    public class DistrictDtoUpdate
    {
        public int? DistrictId { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
        public int? ProvinceRefId { get; set; }
    }
}

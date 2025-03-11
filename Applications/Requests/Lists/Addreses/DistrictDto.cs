namespace Applications.Requests.Lists.Addreses
{
    public class DistrictDto
    {
        public int? DistrictId { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
        public int? ProvinceId { get; set; }
        public ICollection<CommuneDto>? Communes { get; set; } 
    }
}

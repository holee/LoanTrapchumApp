namespace Application.Responses.Lists.Addreses
{
    public class DistrictResponse 
    {
        public int? DistrictId { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
        public int? ProvinceId { get; set; }
        public ICollection<CommuneResponse>? Communes { get; set; } 
    }
}

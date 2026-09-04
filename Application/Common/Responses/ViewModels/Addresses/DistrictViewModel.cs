namespace Application.Responses.ViewModels.Addresses
{
    public class DistrictViewModel
    {
        public int? DistrictId { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
    }
    public class VDistricts
    {
        public int? DistrictId { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
        public string? ProDescription{ get; set; }
        public string? ProInKhmer{ get; set; }
        public bool Used { get; set; }
    }
}


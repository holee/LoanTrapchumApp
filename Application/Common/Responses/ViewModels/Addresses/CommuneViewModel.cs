namespace Application.Responses.ViewModels.Addresses
{
    public class CommuneViewModel 
    {
        public int? CommuneId { get; set; }
        public int? DistrictRefId { get; set; }
        public string? ComDesctiption { get; set; }
        public string? ComInKhmer { get; set; }
    }
    public class VCommunes
    {
        public int? CommuneId { get; set; }
        public string? ComDescription { get; set; }
        public string? ComInKhmer { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
        public bool Used { get; set; }
    }
}

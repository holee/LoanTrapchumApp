namespace Applications.Features.Branches.Dtos
{
    public class BranchAddressDtoUpdate 
    {
        public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? ProviceName { get; set; } = string.Empty;
        public string? ProviceKhmerName { get; set; } = string.Empty;
        public string? DistrictName { get; set; } = string.Empty;
        public string? DistrictKhmerName { get; set; } = string.Empty;
        public string? CommuneName { get; set; } = string.Empty;
        public string? CommuneKhmerName { get; set; } = string.Empty;
        public string? VillageName { get; set; } = string.Empty;
        public string? VillageKhmerName { get; set; } = string.Empty;
    }
}

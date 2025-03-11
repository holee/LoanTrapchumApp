namespace Applications.Responses.ViewModels.OtherNames
{
    public class VEntityName
    {
        public long EntId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? AltName { get; set; } = string.Empty;
        public string? EntType { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? JobTitle { get; set; } = string.Empty;
        public string? Sex { get; set; }
        public string? Phone { get; set; } = string.Empty;
        public string? EmailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public int EditSeq { get; set; } = 0;
        public long AddressId { get; set; }
        public string? CountryName { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? ProInKhmer { get; set; } = string.Empty;
        public string? DisInKhmer { get; set; } = string.Empty;
        public string? ComInKhmer { get; set; } = string.Empty;
        public string? VilInKhmer { get; set; } = string.Empty;
    }
}

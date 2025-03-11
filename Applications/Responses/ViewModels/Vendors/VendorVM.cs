namespace Applications.Responses.ViewModels.Vendors
{
    public class VendorVm
    {
        public Guid VendorId { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(MiddleName))
                {
                    return $"{FirstName} {LastName}";
                }
                return $"{FirstName} {MiddleName} {LastName}";
            }
        }
        public string AltName { get; set; } = string.Empty;
        public string Salutation { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}

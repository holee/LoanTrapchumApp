namespace Applications.Requests.Lists.Entities
{
    public class EntVendorDtoUpdate
    {
        public long VendorId { get; set; }
        public string? CompanyName { get; set; }
        public EntityNameDtoUpdate? EntityName { get; set; } 
    }
}

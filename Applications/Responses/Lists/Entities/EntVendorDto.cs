namespace Applications.Requests.Lists.Entities
{
    public class EntVendorDto
    {
        public long VendorId { get; set; }
        public string? CompanyName { get; set; }
        public string? Remark { get; set; }
        public EntityNameDto? EntityName { get; set; }
    }
}

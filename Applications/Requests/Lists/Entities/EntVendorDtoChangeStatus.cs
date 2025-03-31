namespace Applications.Requests.Lists.Entities
{
    public class EntVendorDtoChangeStatus
    {
        public Guid? VendorId { get; set; }
        public bool IsActive { get; set; } = false;
        public int EditSeq { get; set; } = 0;
    }
}

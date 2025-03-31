namespace Applications.Requests.Lists.Customers
{
    public sealed class CustomerActivityDtoCreate
    {
        public int? Id { get; set; }
        public Guid? CustomerRefId { get; set; }
        public bool Status { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }
}

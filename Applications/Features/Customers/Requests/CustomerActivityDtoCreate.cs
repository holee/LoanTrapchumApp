namespace Applications.Features.Customers.Requests
{
    public sealed class CustomerActivityDtoCreate
    {
        public int? Id { get; set; }
        public string? CustomerRefId { get; set; }
        public bool Status { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }
}

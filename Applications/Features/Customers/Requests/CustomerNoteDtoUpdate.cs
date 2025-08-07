namespace Applications.Features.Customers.Requests
{
    public sealed class CustomerNoteDtoUpdate
    {
        public int NoteId { get; set; } 
        public string? CustomerRefId { get; set; }
        public string? CreditAgentRefId { get; set; }
        public string? Description { get; set; }
    }
}

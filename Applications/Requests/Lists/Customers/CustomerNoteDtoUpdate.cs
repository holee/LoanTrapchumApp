namespace Applications.Requests.Lists.Customers
{
    public class CustomerNoteDtoUpdate
    {
        public int NoteId { get; set; } 
        public long CustomerRefId { get; set; }
        public long CreditAgentRefId { get; set; }
        public string? Description { get; set; }
    }
}

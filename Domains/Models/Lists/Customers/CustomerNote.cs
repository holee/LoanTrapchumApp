namespace Domains.Models.Lists.Customers
{
    public class CustomerNote :BaseEntity
    {
        public int NoteId { get; set; }
        public long? CustomerRefId { get; set; }
        public long? CreditAgentRefId { get; set; } 
        public string? Description { get; set; }
    }
}

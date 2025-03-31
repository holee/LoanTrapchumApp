using Domains.Models.Bases;

namespace Domains.Models.Lists.Customers
{
    public class CustomerNote :BaseEntity
    {
        public int NoteId { get; set; }
        public Guid? CustomerRefId { get; set; }
        public Guid? CreditAgentRefId { get; set; } 
        public string? Description { get; set; }
    }
}

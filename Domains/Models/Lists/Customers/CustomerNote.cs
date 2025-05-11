using Domains.Models.Bases;

namespace Domains.Models.Lists.Customers
{
    public class CustomerNote :BaseEntity
    {
        public int NoteId { get; set; }
        public string? CustomerRefId { get; set; }
        public string? CreditAgentRefId { get; set; } 
        public string? Description { get; set; }
    }
}

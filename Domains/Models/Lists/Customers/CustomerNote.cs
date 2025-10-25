using Domains.Models.Bases;

namespace Domains.Models.Lists.Customers
{
    public class CustomerNote :BaseEntity
    {
        public int NoteId { get; set; }
        public DateTime? NoteDate { get; set; } = DateTime.Now;
        public string? CustomerRefId { get; set; }
        public string? CreditAgentRefId { get; set; } 
        public string? Description { get; set; }
    }
}

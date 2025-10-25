using Applications.Bases;

namespace Applications.Responses.ViewModels.Customers;

public sealed class VCustomerNotes : BasePropEntity
{
    public int NoteId { get; set; }
    public string? CustomerRefId { get; set; } 
    public string? CreditAgentName { get; set; } = string.Empty;
    public string? Description { get; set; }=string.Empty;
    public DateTime? NoteDate { get; set; } = DateTime.Now;
    public DateTime? Created { get; set; } 

}

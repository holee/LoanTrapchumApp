namespace Applications.Responses.ViewModels.Customers;

public class VCustomerNotes
{
    public int NoteId { get; set; }
    public long? CustomerRefId { get; set; } 
    public string? CreditAgentName { get; set; } = string.Empty;
    public string? Description { get; set; }=string.Empty;
    public DateTime? Created { get; set; } 

}

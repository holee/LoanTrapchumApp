namespace Application.Features.Customers.Requests;

public class CustomerNoteDtoCreate
{
    public DateTime? NoteDate { get; set; } = DateTime.Now;
    public string? CustomerRefId { get; set; }
    public string? CreditAgentRefId { get; set; }
    public string? Description { get; set; }    
}

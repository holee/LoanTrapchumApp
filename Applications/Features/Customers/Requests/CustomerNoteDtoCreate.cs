namespace Applications.Features.Customers.Requests;

public class CustomerNoteDtoCreate
{
    public string? CustomerRefId { get; set; }
    public string? CreditAgentRefId { get; set; }
    public string? Description { get; set; }    
}

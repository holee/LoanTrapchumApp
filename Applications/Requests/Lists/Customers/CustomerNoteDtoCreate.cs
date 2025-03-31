namespace Applications.Requests.Lists.Customers;

public class CustomerNoteDtoCreate
{
    public Guid? CustomerRefId { get; set; }
    public Guid? CreditAgentRefId { get; set; }
    public string? Description { get; set; }    
}

namespace Applications.Requests.Lists.Customers;

public class CustomerNoteDtoCreate
{
    public long CustomerRefId { get; set; }
    public long CreditAgentRefId { get; set; }
    public string? Description { get; set; }    
}

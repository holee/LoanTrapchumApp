namespace Applications.Responses.Lists.Entities;

public class CoPawnCustomerDto
{
    public int Id { get; set; }
    public Guid? CustomerRefId { get; set; }
    public Guid? LoanRefId { get; set; }
    public bool IsActive { get; set; }

}

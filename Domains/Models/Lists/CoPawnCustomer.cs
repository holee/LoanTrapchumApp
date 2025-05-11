namespace Domains.Models.Lists;

public sealed class CoPawnCustomer
{
    public int Id { get; set; }
    public string? CustomerRefId { get; set; }
    public string? LoanRefId { get; set; }
    public bool? IsActive { get; set; }

}

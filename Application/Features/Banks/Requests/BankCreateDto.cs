namespace Application.Features.Currencies;

public class BankCreateDto
{
    public int BankId { get; set; }
    public string Bankcode { get; set; } = null!;
    public string AddressRefId { get; set; } = null!;
    public int? BranchRefId { get; set; } = null!;
    public string? Description { get; set; }
    public string? Remark { get; set; }
    public string? IsClosed { get; set; }
    public string? IsHead { get; set; }
    public string? Khmer { get; set; }
    public string? Contact { get; set; }
    public string? DatabaseName { get; set; }
    public string? BranchManager { get; set; }
    public string? BranchManagerTel { get; set; }
}

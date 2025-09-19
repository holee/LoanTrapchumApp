namespace Applications.Features.Transactions.CashManagements;

public class CashVaultDtoCreate
{
    public CashVaultDtoCreate() { }
    public CashVaultDtoCreate(int? branchRefId, int? bankRefId, string? userRefId, int? cVStatusRefId, DateTime? cVDate)
    {
        BranchRefId = branchRefId;
        BankRefId = bankRefId;
        UserRefId = userRefId;
        CvStatusRefId = cVStatusRefId;
        CvDate = cVDate;
    }

    public CashVaultDtoCreate(string? cashVaultsId, int? branchRefId, int? bankRefId, string? userRefId, int? cVStatusRefId, DateTime? cVDate)
        : this(branchRefId, bankRefId, userRefId, cVStatusRefId, cVDate)
    {
        CashVaultsId = cashVaultsId;
    }
    public string? CashVaultsId { get; set; } = string.Empty;
    public int? BranchRefId { get; set; }
    public int? BankRefId { get; set; }
    public string? UserRefId { get; set; }
    public int? CvStatusRefId { get; set; }
    public DateTime? CvDate { get; set; }

    public static CashVaultDtoCreate Create(int? branchId, int? bankId, string? userId, int? cVStatusId, DateTime? cVDate = null)
    {
        cVDate ??= DateTime.Now;
        var cashVaultsId = Guid.CreateVersion7().ToString();
        return new CashVaultDtoCreate(cashVaultsId, branchId, bankId, userId, cVStatusId, cVDate);
    }

}

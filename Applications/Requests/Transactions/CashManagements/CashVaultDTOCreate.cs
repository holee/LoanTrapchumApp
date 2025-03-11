namespace Applications.Requests.Transactions.CashManagements;

public class CashVaultDtoCreate
{
    public CashVaultDtoCreate(){}
    public CashVaultDtoCreate(int? branchRefId, int? bankRefId, long? userRefId, int? cVStatusRefId, DateTime? cVDate)
    {
        BranchRefId = branchRefId;
        BankRefId = bankRefId;
        UserRefId = userRefId;
        CvStatusRefId = cVStatusRefId;
        CvDate = cVDate;
    }

    public int? BranchRefId { get; set; }
    public int? BankRefId { get; set; }
    public long? UserRefId { get; set; }
    public int? CvStatusRefId { get; set; }
    public DateTime? CvDate { get; set; }
}

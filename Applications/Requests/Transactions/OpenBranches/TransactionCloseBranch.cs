namespace Applications.Requests.Transactions.OpenBranches
{
    public class TransactionCloseBranch
    {
        public int TranTypeRefId { get; set; }
        public string? TransactionName { get; set; }
        public OpenBranchDtoCreate OpenBranch { get; set; } = new();

    }
}

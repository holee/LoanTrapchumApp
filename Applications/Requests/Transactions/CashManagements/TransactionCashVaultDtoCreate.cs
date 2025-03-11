namespace Applications.Requests.Transactions.CashManagements
{
    public class TransactionCashVaultDtoCreate
    {
        public TransactionCashVaultDtoCreate()
        {
            CashVault = new CashVaultDtoCreate();
        }

        public TransactionCashVaultDtoCreate(Guid? tranCode, int? tranTypeRefId,
                                             string transactionTypeName,
                                             long? userRefId,
                                             DateTime? transactionDate)
        {
            TranCode = tranCode;
            TranTypeRefId = tranTypeRefId;
            TransactionTypeName = transactionTypeName;
            UserRefId = userRefId;
            TransactionDate = transactionDate;
            CashVault = new CashVaultDtoCreate();
        }

        public TransactionCashVaultDtoCreate(Guid? tranCode, int? tranTypeRefId,
                                             string transactionTypeName,
                                             long? userRefId,
                                             DateTime? transactionDate,
                                             CashVaultDtoCreate cashVault,
                                             CashManagementDtoCreate cashManagements
                                             ) : this(tranCode, tranTypeRefId,
                                             transactionTypeName,
                                             userRefId,
                                             transactionDate)
        {
            CashVault = cashVault;
            CashManagements.Add(cashManagements);
        }

        public Guid? TranCode { get; set; }
        public int? TranTypeRefId { get; set; }
        public string TransactionTypeName { get; set; } = string.Empty;
        public long? UserRefId { get; set; }
        public DateTime? TransactionDate { get; set; } = DateTime.Now;
        public CashVaultDtoCreate? CashVault { get; set; }
        public ICollection<CashManagementDtoCreate> CashManagements { get; set; } = [];
    }
}

using Applications.Enumerations;

namespace Applications.Features.Transactions.CashManagements
{
    public class TrnxCashVaultDtoCreate
    {
        public TrnxCashVaultDtoCreate()
        {
            CashVault = new CashVaultDtoCreate();
        }

        public TrnxCashVaultDtoCreate(int? branchId, int? tranTypeRefId,
                                             string transactionTypeName,
                                             string? userRefId,
                                             DateTime? transactionDate)
        {
            TransactionId = Guid.CreateVersion7().ToString();
            BranchId = branchId;
            TranTypeRefId = tranTypeRefId;
            TransactionTypeName = transactionTypeName;
            UserRefId = userRefId;
            TransactionDate = transactionDate;
            CashVault = new CashVaultDtoCreate();
        }

        public TrnxCashVaultDtoCreate(string? trnxId, int? branchId, int? tranTypeRefId,
                                      string transactionTypeName, string? userRefId, DateTime? transactionDate)
            : this(branchId, tranTypeRefId,
                                             transactionTypeName,
                                             userRefId,
                                             transactionDate)
        {
            TransactionId = trnxId;
        }

        public TrnxCashVaultDtoCreate(int? branchId, int? tranTypeRefId,
                                             string transactionTypeName,
                                             string? userRefId,
                                             DateTime? transactionDate,
                                             CashVaultDtoCreate cashVault,
                                             CashManagementDtoCreate cashManagements
                                             ) : this(branchId, tranTypeRefId,
                                             transactionTypeName,
                                             userRefId,
                                             transactionDate)
        {
            CashVault = cashVault;
            CashManagements.Add(cashManagements);
        }

        public string? TransactionId { get; set; }
        public int? BranchId { get; set; }
        public int? TranTypeRefId { get; set; }
        public string TransactionTypeName { get; set; } = string.Empty;
        public string? UserRefId { get; set; }
        public DateTime? TransactionDate { get; set; } = DateTime.Now;
        public CashVaultDtoCreate? CashVault { get; set; }
        public ICollection<CashManagementDtoCreate> CashManagements { get; set; } = [];

        public static TrnxCashVaultDtoCreate Create(int? branchId, int? tranTypeRefId, string trnxName, string? userRefId, DateTime? trnxDate = null)
        {

            trnxDate ??= DateTime.Now;

            var tranId = Guid.CreateVersion7().ToString();

            return new(tranId, branchId, tranTypeRefId, trnxName, userRefId, trnxDate);
        }


        public static TrnxCashVaultDtoCreate Create(int? branchId, TransactionTypes tranxType, string trnxName,
                        string? userRefId, CashVaultDtoCreate CashVault, CashManagementDtoCreate cashManagements, DateTime? trnxDate = null)
        {

            trnxDate ??= DateTime.Now;

            return new(branchId,(int)tranxType, trnxName, userRefId, trnxDate, CashVault, cashManagements);
        }


    }
}

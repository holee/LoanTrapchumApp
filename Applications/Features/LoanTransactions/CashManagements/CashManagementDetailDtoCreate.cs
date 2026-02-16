using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Features.Transactions.CashManagements
{
    public sealed class CashManagementDetailDtoCreate
    {
        public CashManagementDetailDtoCreate()
        { }
        public CashManagementDetailDtoCreate(int? bankRefId, int? branchRefId,
            int? currencyRefId, string? currencyRefName,
            int? currencyNoteRefId, int? currencyNoteValue, 
            int? quantity)
        {
            BankRefId = bankRefId;
            BranchRefId = branchRefId;
            CurrencyRefId = currencyRefId;
            CurrencyRefName = currencyRefName;
            CurrencyNoteRefId = currencyNoteRefId;
            CurrencyNoteValue = currencyNoteValue;
            Quantity = quantity;
        }

        //public string? CachmanagementRefId { get; set; }
        public string? TransactionsRefId { get; set; }
        public int? BankRefId { get; set; }
        public int? BranchRefId { get; set; }
        public int? CurrencyRefId { get; set; }
        public string? CurrencyRefName { get; set; }
        public string? CurrencyRefDesc { get; set; } 
        public int? CurrencyNoteRefId { get; set; }
        public int? CurrencyNoteValue { get; set; }
        [NotMapped] public int? QOnHand { get; set; } = 0;
        [NotMapped] public int? Quantity { get; set; } = 0;
        [NotMapped] public int? TotalQuantity => Quantity == 0 ? QOnHand : Quantity + QOnHand;
        [NotMapped] public decimal? TotalValue { get; set; } = 0;     
        [NotMapped]
        public decimal? Value
        {
            get
            {
                var totalQuantity = Quantity == 0 ? QOnHand : QOnHand + Quantity;

                var total = CurrencyNoteValue * totalQuantity;

                return total;

            }

        }

    }
}

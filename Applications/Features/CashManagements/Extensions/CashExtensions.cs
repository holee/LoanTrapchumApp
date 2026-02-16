using Applications.Features.Transactions.CashManagements;

namespace Applications.Features.CashManagements.Extensions
{
    public static class CashExtensions
    {
        public static List<CashManagementDetailDtoCreate> ToCashValutIn(this IEnumerable<CashManagementDetailDtoCreate> source, int? bankRefId, int? branchRefId)
        {
            var NewCashMDVIn = source.Where(x => x.Quantity > 0)
                                       .Select(x => new CashManagementDetailDtoCreate
                                         (
                                             bankRefId,
                                             branchRefId,
                                             x.CurrencyRefId,
                                             x.CurrencyRefName,
                                             x.CurrencyNoteRefId,
                                             x.CurrencyNoteValue,
                                             x.Quantity
                                         )).ToList();

            return NewCashMDVIn;
        }


        public static List<CashManagementDetailDtoCreate> ToCashVaultOut(this IEnumerable<CashManagementDetailDtoCreate> source, int? bankRefId, int? branchRefId)
        {
            return source.Select(x => new CashManagementDetailDtoCreate
            {
                BranchRefId = branchRefId,
                BankRefId = bankRefId,
                CurrencyRefId = x.CurrencyRefId,
                CurrencyRefName = x.CurrencyRefName,
                CurrencyNoteRefId = x.CurrencyNoteRefId,
                CurrencyNoteValue = x.CurrencyNoteValue,
                Quantity = x.Quantity,
                TotalValue = x.TotalValue,
                CurrencyRefDesc = x.CurrencyRefDesc,
            }).Where(x => x.Quantity > 0).ToList();
        }



    }
}

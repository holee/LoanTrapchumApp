using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.CashManagements;

namespace Applications.Features.Currencies.Extensions;

public static class CurrencyNoteExtension
{
    public static List<CashManagementDetailDtoCreate> ToCashManagementDetail(this IEnumerable<VCurrencyNotes> CurrencyNotes, int? currencyId, int? BranchId, int? BankId)
    {
        return CurrencyNotes!.Where(x => x.CurrencyId == currencyId)
                .Select(x =>
                new CashManagementDetailDtoCreate
                {
                    CurrencyNoteRefId = x.Id,
                    BranchRefId = BranchId,
                    BankRefId = BankId,
                    CurrencyRefId = x.CurrencyId,
                    CurrencyRefName = x.CurrencyCode,
                    CurrencyNoteValue = (int?)x.CurrencyValue,
                    QOnHand = x.Qty == null ? 0 : x.Qty,
                    Quantity = 0,
                    TotalValue = 0
                }).ToList();
    }
}

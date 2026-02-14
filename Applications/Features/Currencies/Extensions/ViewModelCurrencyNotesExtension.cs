using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.CashManagements;

namespace Applications.Features.Currencies.Extensions;

public static class ViewModelCurrencyNotesExtension
{
    public static List<CashManagementDetailDtoCreate> CashManagementsDtoCreate(this IEnumerable<VCurrencyCounter>? note, int? branchId, int? currencyId, int? bankId = 11)
    {
        return note!.Where(x => x.CurrencyId == currencyId).Select(x => new CashManagementDetailDtoCreate
        {
            BranchRefId = branchId,
            BankRefId = bankId,
            CurrencyRefId = x.CurrencyId,
            CurrencyRefName = x.CurrencyCode,
            CurrencyNoteRefId = x.Id,
            CurrencyNoteValue = (int?)x.CurrencyValue,
            QOnHand = x.TotalQty == null ? 0 : x.TotalQty,
            TotalValue = 0
        }).ToList();
    }


    public static List<CashManagementDetailDtoCreate> CashManagementsDtoCreate(this IEnumerable<VCurrencyNotes>? note, int? branchId, int? currencyId, int? bankId = 11)
    {
        return note!.Where(x => x.CurrencyId == currencyId).Select(x => new CashManagementDetailDtoCreate
        {
            BranchRefId = branchId,
            BankRefId = bankId,
            CurrencyRefId = x.CurrencyId,
            CurrencyRefName = x.CurrencyCode,
            CurrencyNoteRefId = x.Id,
            CurrencyNoteValue = (int?)x.CurrencyValue,
            QOnHand = x.Qty == null ? 0 : x.Qty,
            TotalValue = 0
        }).ToList();
    }



    public static CashManagementDetailDtoCreate CashManagementDtoCreate(this VCurrencyCounter? note, int? branchId, int? currencyId, int? bankId = 11)
    {
        return new CashManagementDetailDtoCreate
        {
            BranchRefId = branchId,
            BankRefId = bankId,
            CurrencyRefId = note!.CurrencyId,
            CurrencyRefName = note!.CurrencyCode,
            CurrencyNoteRefId = note!.Id,
            CurrencyNoteValue = (int?)note!.CurrencyValue,
            QOnHand = note!.TotalQty == null ? 0 : note!.TotalQty,
            //Quantity = 0,
            TotalValue = 0
        };
    }

}

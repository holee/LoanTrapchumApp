using Applications.Requests.Transactions.CashManagements;

namespace Applications.Responses.ViewModels.Currencies;

public static class ViewModelCurrencyNotesExtension
{
    public static List<CashManagementDetailDtoCreate> CashManagementsDtoCreate(this IEnumerable<ViewModelCurrencyNotes>? note, int? branchId, int? currencyId, int? bankId = 11)
    {
        return note!.Where(x => x.CurrencyRefId == currencyId).Select(x => new CashManagementDetailDtoCreate
        {
            BranchRefId = branchId,
            BankRefId = bankId,
            CurrencyRefId = x.CurrencyRefId,
            CurrencyRefName = x.CurrencyCode,
            CurrencyNoteRefId = x.Id,
            CurrencyNoteValue = (int?)x.CurrencyValue,
            QOnHand = x.Quantity == null ? 0 : x.Quantity,
            //Quantity = 0,
            TotalValue = 0
        }).ToList();
    }


    public static CashManagementDetailDtoCreate CashManagementDtoCreate(this ViewModelCurrencyNotes? note, int? branchId, int? currencyId, int? bankId = 11)
    {
        return new CashManagementDetailDtoCreate
        {
            BranchRefId = branchId,
            BankRefId = bankId,
            CurrencyRefId = note!.CurrencyRefId,
            CurrencyRefName = note!.CurrencyCode,
            CurrencyNoteRefId = note!.Id,
            CurrencyNoteValue = (int?)note!.CurrencyValue,
            QOnHand = note!.Quantity == null ? 0 : note!.Quantity,
            //Quantity = 0,
            TotalValue = 0
        };
    }

}

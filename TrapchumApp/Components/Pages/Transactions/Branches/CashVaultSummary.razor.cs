using Applications.Services.Currencies;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Branches;

public partial class CashVaultSummary(ICurrencyServiceImpl currencyService)
{
    // [Parameter] public int SelectedBranchId { get; set; }
    //
    // [Parameter] public int SelectedCurrencyId { get; set; }
    //
    // private IEnumerable<VCurrencyCounter> CurrencyNotes = [];
    //
    // protected override async Task OnInitializedAsync()
    // {
    //     CurrencyNotes = await currencyService.GetCurrencyNotes(SelectedCurrencyId, SelectedBranchId);
    //
    //     await base.OnInitializedAsync();
    // }
}
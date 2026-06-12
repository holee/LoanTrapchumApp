using Applications.Features.Transactions.CashManagements;

namespace TrapchumApp.Pages.Transactions.Branches
{
    public partial class VaultInRow
    {
        [Parameter] public CashManagementDetailDtoCreate Item { get; set; } = new();
        [Parameter] public EventCallback Calculate { get; set; }
        [Parameter] public int? CurrencyId { get; set; } = (int)CurrenciesType.KHR;
        [Parameter] public string Id { get; set; } = Guid.NewGuid().ToString("N");
        string sp = "";

        async void CalTotal()
        {
            await Calculate.InvokeAsync();
        }

        void CalculateByQuantity(int? qty)
        {
            qty ??= 0;

            if (qty > 0)
            {
                Item.TotalValue = (Item.CurrencyNoteValue) * (qty);
            }
            else
            {
                Item.TotalValue = 0;
            }

            sp = "#,##0.00";

            CalTotal();

        }

        void CalculateByAmount(decimal? value)
        {
            value ??= 0;

            sp = "";

            var qty = Convert.ToInt32(value / Item?.CurrencyNoteValue);

            if (qty > 0 && value > 0)
            {
                //Item!.Quantity = qty;

                Item!.TotalValue = value;
            }
            else
            {
                //Item!.Quantity = 0;
            }
            CalTotal();
        }

        void MouseLeaved()
        {
            Item.TotalValue = Item.CurrencyNoteValue * Item.Quantity;
            sp = "#,##0.00";
        }

        void Reset()
        {
            // Item!.Quantity = 0;
            Item!.TotalValue = 0;
        }

    }
}

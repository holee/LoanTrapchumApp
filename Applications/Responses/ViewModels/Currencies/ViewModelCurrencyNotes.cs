namespace Applications.Responses.ViewModels.Currencies
{
    public sealed class ViewModelCurrencyNotes
    {
        public int Id { get; set; }
        public int CurrencyRefId { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
        public decimal? CurrencyValue { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public bool IsActive { get; set; } = true;
    }

}

namespace Applications.Responses.ViewModels.Currencies
{
    public class VCashVaults
    {
        public int? BankRefId { get; set; }
        public int? BranchRefId { get; set; }
        public int? CurrencyRefId { get; set; }
        public string? CurrencyCode { get; set; }
        public decimal? StartCash { get; set; }
        public decimal? VaultOut { get; set; }
        public decimal? VaultIn { get; set; }
        public decimal? VaultBalance { get; set; }
        public decimal? UserBalance { get; set; }
    }
}

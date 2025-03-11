namespace Applications.Responses.ViewModels.Ledgers;

public class LedgerVm
{
    public int LedgerId { get; set; }
    public int? CurrencyRefId { get; set; }
    public string LedgerName { get; set; } = string.Empty;
}

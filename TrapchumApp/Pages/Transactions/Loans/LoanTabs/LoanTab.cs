namespace TrapchumApp.Pages.Transactions.Loans.LoanTabs;

public sealed class LoanTab
{
    public string TabId { get; set; } = string.Empty;
    public int TabId1 { get; set; } = 0;
    public long TabId2 { get; set; } = 0;
    public string Title { get; set; } = "Loan Tab";
    public RenderFragment? ChildContent { get; set; }
}

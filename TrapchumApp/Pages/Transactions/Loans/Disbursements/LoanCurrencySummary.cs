namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public sealed record LoanCurrencySummary(
    int? CurrencyId,
    string CurrencyName,
    int LoanCount,
    decimal TotalPrinciple,
    decimal TotalPaidPrinciple,
    decimal TotalOutstanding
);

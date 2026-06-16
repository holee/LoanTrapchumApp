namespace Applications.Calculations;

public interface ICalculatorBase
{
    int? Installments { get; set; }
    decimal? InterestRate { get; set; }
    decimal? Principle { get; set; }
    int? BranchId { get; set; }
    string? CreditAgentRefId { get; set; }
    int? RepaymentModeRefId { get; set; }
    int? CurrencyId { get; set; }
}

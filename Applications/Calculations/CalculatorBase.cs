namespace Applications.Calculations
{
    public abstract class CalculatorBase : ICalculatorBase
    {
        public int? Installments { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? Principle { get; set; }
        public int? BranchId { get; set; }
        public int? CurrencyId { get; set; } 
        public string? CreditAgentRefId { get; set; }
        public int? RepaymentModeRefId { get; set; }
    }
}

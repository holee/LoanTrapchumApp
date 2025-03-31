namespace Applications.Responses.ViewModels.Customers
{
    public sealed class VCustomerIncomes 
    {
        public long IncomeId { get; set; }
        public string? CustomerRefId { get; set; }
        public string? IncomeSource { get; set; }
        public decimal? IncomeAmount { get; set; }
        public string? InDescription { get; set; }
    }
}

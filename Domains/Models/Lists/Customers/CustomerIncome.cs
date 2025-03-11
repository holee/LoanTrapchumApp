namespace Domains.Models.Lists.Customers
{
    public class CustomerIncome
    {
        public long Id { get; set; }
        public long? CustomerRefId { get; set; }
        public string? IncomeSource { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public Customer? Customer { get; set; }
    }
}

namespace Applications.Features.Entities
{
    public class CustomerIncomeDtoUpdate
    {
        public long Id { get; set; }
        public int CustomerId { get; set; }
        public string? IncomeTitle { get; set; }
        public decimal? Income { get; set; }
        public string? Description { get; set; }
    }
}

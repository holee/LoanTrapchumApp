namespace Applications.Requests.Settings
{
    public sealed class UserLoanSizeDtoCreate
    {
        public long? UserRefId { get; set; }
        public decimal? MinLoanSize { get; set; } = 0.00M;
        public decimal? MaxLoanSize { get; set; } = 0.00M;
        public DateTime? StartDate { get; set; }=DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

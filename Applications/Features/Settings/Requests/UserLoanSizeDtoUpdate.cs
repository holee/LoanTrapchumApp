namespace Applications.Features.Settings.Requests
{
    public class UserLoanSizeDtoUpdate 
    {
        public long Id { get; set; }
        public long? UserRefId { get; set; }
        public decimal? MinLoanSize { get; set; }
        public decimal? MaxLoanSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

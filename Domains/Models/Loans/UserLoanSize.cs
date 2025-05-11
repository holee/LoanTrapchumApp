using Domains.Models.Bases;

namespace Domains.Models.Loans
{
    public class UserLoanSize:BaseEntity
    {
        public long Id { get; set; }
        public string? UserRefId { get; set; }
        
        public decimal? MinLoanSize { get; set; }
        public decimal? MaxLoanSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

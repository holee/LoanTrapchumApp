using Domains.Models.Bases;

namespace Applications.Responses.Loans.Disbursements
{
    public class LoanCategoryResponse : BaseEntity
    {
        public int LoanCategoryId { get; set; }
        public string? LoancategoryCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? DesInKhmer { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
    }
}

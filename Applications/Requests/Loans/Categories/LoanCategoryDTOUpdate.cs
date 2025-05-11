using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Loans.Categories
{
    public sealed class LoanCategoryDtoUpdate
    {
        public int LoanCategoryId { get; set; }
        public string? LoanCategoryCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? DesInKhmer { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
        public int? EditSeq { get; set; }
        public bool? IsActive { get; set; }
    }
}

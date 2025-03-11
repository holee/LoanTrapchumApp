using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Loans
{
    public class LoanCategoryDtoUpdate
    {
        public int LoanCategoryId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Loan category name is required.")]
        public string? LoancategoryCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? DesInKhmer { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
        public int? EditSeq { get; set; }
        public bool? IsActive { get; set; }
    }
}

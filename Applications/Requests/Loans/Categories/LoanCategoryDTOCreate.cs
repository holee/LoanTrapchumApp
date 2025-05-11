using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Loans.Categories
{
    public class LoanCategoryDtoCreate
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="Loan category name is required.")]
        public string? LoanCategoryCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? DesInKhmer { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
        public bool? IsActive { get; set; } = true;
    }
}

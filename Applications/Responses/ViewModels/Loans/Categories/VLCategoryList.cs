using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Loans.Categories
{
    public sealed class VlCategoryList
    {
        public int LoanCategoryId { get; set; }
        public string? LoancategoryCode { get; set; }
        public string? Description { get; set; }
        public string? DesInKhmer { get; set; }
        public string? Code { get; set; }
        public int? EditSeq { get; set; }
        public bool? IsActive { get; set; }
        public string? IsInUse { get; set; }
        [NotMapped]public int RowIndex { get; set; } = 0;

    }
}

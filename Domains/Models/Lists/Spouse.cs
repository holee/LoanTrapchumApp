namespace Domains.Models.Lists
{
    public abstract class Spouse
    {
        public string SpouseName { get; set; } = string.Empty;
        public decimal? Expenditure { get; set; }
        public DateTime? SpouseDateOfBirth { get; set; }
        public string? SpouseNationalityId { get; set; } = string.Empty;
        public string? SpousePassportId { get; set; } = string.Empty; 
        public string? SpousePhoneNo { get; set; } = string.Empty;
        public string? Remark { get; set; } = string.Empty;
        public string? Occupation { get; set; }

    }
}

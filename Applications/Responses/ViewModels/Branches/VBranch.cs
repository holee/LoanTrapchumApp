namespace Applications.Responses.ViewModels.Branches
{
    public class VBranch
    {
        public int BranchId { get; set; }
        public int? BankRefId { get; set; }
        public string? BranchName { get; set; }
        public int? EditSeq { get; set; } = 0;
        public bool? IsActive { get; set; } = true;
        public string? Description { get; set; }
        public string? CountryName { get; set; }
        public string? City { get; set; }
        public string? ProInKhmer { get; set; }
        public string? DisInKhmer { get; set; }
        public string? ComInKhmer { get; set; }
        public string? VilInKhmer { get; set; }
        public bool Used { get; set; } = false;
        public string? OpenStatus { get; set; }

        public override string ToString()
        {
            return $"{ProInKhmer} {DisInKhmer} {ComInKhmer} {VilInKhmer}";
        }
    }
}

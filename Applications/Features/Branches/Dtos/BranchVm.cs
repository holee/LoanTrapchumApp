namespace Applications.Features.Branches.Dtos
{
    public sealed class BranchVm
    {
        public int BranchId { get; set; }
        public int? BankRefId { get; set; }
        public string BranchName { get; set; } = default!;
        public string? Description { get; set; }
        public string? CountryName { get; set; }
        public string? City { get; set; }
        public string? ProDescription { get; set; }
        public string? ProInKhmer { get; set; }
        public string? DisDescription { get; set; }
        public string? DisInKhmer { get; set; }
        public string? ComDescription { get; set; }
        public string? ComInKhmer { get; set; }
        public string? VillDescription { get; set; }
        public string? VilInKhmer { get; set; }
        public bool IsActive { get; set; }
        public override string ToString()
        {
            return $"{ProInKhmer} {DisInKhmer} {ComInKhmer} {VilInKhmer}";
        }
    }
}

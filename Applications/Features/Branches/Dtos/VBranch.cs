using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Features.Branches.Dtos;

public sealed class VBranch
{
    public int? BranchId { get; set; }
    public int? BankRefId { get; set; }
    public string? BankName { get; set; }
    public string? BranchName { get; set; }
    public string? BranchCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Zone { get; set; }
    public bool? IsActive { get; set; } = true;
    public string? Description { get; set; }
    public string? CountryName { get; set; }
    public string? City { get; set; }
    public string? ProInKhmer { get; set; }
    public string? DisInKhmer { get; set; }
    public string? ComInKhmer { get; set; }
    public string? VilInKhmer { get; set; }
    public string? ImagePath { get; set; } = string.Empty;
    public bool Used { get; set; } = false;
    public int OpenStatus { get; set; } = 0;
    public override string ToString()
    {
        return $"{ProInKhmer} {DisInKhmer} {ComInKhmer} {VilInKhmer}";
    }

    [NotMapped] public int RowIndex { get; set; } = 1;
    public bool? IsHeadOffice { get; set; } = false;

}

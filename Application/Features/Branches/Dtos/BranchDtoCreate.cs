namespace Application.Features.Branches.Dtos;

public class BranchDtoCreate
{
    public int? BankRefId { get; set; } = 11;
    public string? BranchName { get; set; }
    public string? Description { get; set; }
    public string? CountryName { get; set; }
    public string? City { get; set; }
    public int? ProvinceRefId { get; set; }
    public string? ProDescription { get; set; }
    public string? ProInKhmer { get; set; }
    public int? DistrictRefId { get; set; }
    public string? DisDescription { get; set; }
    public string? DisInKhmer { get; set; }
    public int? CommuneRefId { get; set; }
    public string? ComDescription { get; set; }
    public string? ComInKhmer { get; set; }
    public int? VillageRefId { get; set; }
    public string? VillDescription { get; set; }
    public string? VilInKhmer { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid? ImagePathId { get; set; } = Guid.Empty;
    public string? ImagePath { get; set; } = string.Empty;
    public string? BranchCode { get; set; }
    public string? Zone { get; set; }
    public string? PhoneNumber { get; set; }
}

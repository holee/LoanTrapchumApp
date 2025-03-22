using Domains.Models.Bases;
using Domains.Models.Currencies;
using Domains.Models.Lists.Employees;

namespace Applications.Responses.Lists.Entities; 

public class BranchResponse:BaseEntity
{
    public int BranchId { get; set; }
    public int? BankRefId { get; set; }
    public string BranchName { get; set; } = default!;
    public string? Description { get; set; }
    public long? AddressRefId { get; set; }
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
    public bool IsActive { get; set; }
    public Bank? Bank { get; set; }
    public List<Employee>? Employees { get; set; }
}

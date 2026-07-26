using Domains.Models.Bases;
using Domains.Models.Currencies;
using Domains.Models.Lists.Employees;
using Domains.Models.Loans;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Domains.Models.Lists;

public class Branch : BaseEntity
{
    public int BranchId { get; set; }
    public int? BankRefId { get; set; }
    public string BranchName { get; set; } = default!;
    public string? Description { get; set; }
    public long? AddressRefId { get; set; }
    public string? CountryName { get; set; }
    public string? BranchCode { get; set; }
    public string? Zone { get; set; }
    public string? PhoneNumber { get; set; }
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
    public bool? IsHeadOffice { get; set; } = false;
    public Bank? Bank { get; set; }
    public Guid? ImagePathId { get; set; } = Guid.Empty;
    public LtDocument? LtDocument { get; set; } = null; 
    public List<Employee>? Employees { get; set; } = [];

}

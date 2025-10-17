using Domains.Models.Bases;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;

namespace Domains.Models.Lists;

public sealed class EntityNames : BaseEntity
{
    public string Id { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? AltName { get; set; }
    public string? Name { get; set; }
    public string? EntType { get; set; }
    public string? PassportId { get; set; }
    public string? Nationality { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Salutation { get; set; }
    public string? Gender { get; set; }
    public string? JobTitle { get; set; }
    public string? Phone { get; set; }
    public string? AltPhone { get; set; }
    public string? EmailAddress { get; set; }
    public string? IdCard { get; set; }
    public string? CompanyName { get; set; }
    public string? MarritalStatus { get; set; }
    public bool? IsActive { get; set; }
    public string? CountryName { get; set; }
    public string? City { get; set; }
    public string? Occupation { get; set; }
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
    public string? Description { get; set; }
    public bool? Converted { get; set; } = false;
    public string? CreatedBy { get; set; }
    public Customer? Customer { get; set; }
    public Employee? Employee { get; set; }
    public Vendor? Vendor { get; set; }
    public List<LoanGuarantorLink> GuarantorsLink { get; set; } = [];
}
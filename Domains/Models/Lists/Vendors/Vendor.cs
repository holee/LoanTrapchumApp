using Domains.Models.Bases;

namespace Domains.Models.Lists.Vendors;

public sealed class Vendor : BaseAddress
{
    public long VendorId { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Name { get; set; }
    public string? AltName { get; set; }
    public string? JobTitle { get; set; }
    public string? Salution { get; set; }
    public string? Phone { get; set; }
    public string? AltPhone { get; set; }
    public string? EmailAddress { get; set; }
    public string? CompanyName { get; set; }
    public string? EnType { get; set; }
    public string? IdCard { get; set; }
    public bool? IsActive { get; set; }
    public string? Description { get; set; }

}

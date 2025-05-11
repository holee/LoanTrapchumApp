using Domains.Models.Bases;

namespace Domains.Models.Lists.Vendors;

public sealed class Vendor 
{
    public string? VendorId { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public EntityNames? EntityNames { get; set; }
}

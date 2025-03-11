using Domains.Models.Lists;

namespace Domains.Models.Currencies;

public class Bank
{
    public int BankId { get; set; }
    public string BankCode { get; set; } = null!;
    public string BankName { get; set; } = null!;
    public string? AltName { get; set; } = string.Empty;
    public string? AltPhone { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? WebSite { get; set; } = string.Empty;
    public bool? IsClosed { get; set; }
    public bool? IsHead { get; set; }
    public string? Country { get; set; } = string.Empty;
    public string? ProvinceName { get; set; } = string.Empty;
    public string? DistrictName { get; set; } = string.Empty;
    public string? CommuneName { get; set; } = string.Empty;
    public string? VillageName { get; set; } 
    public string? Remark { get; set; }
    public List<Branch>? Branches { get; set; }
}



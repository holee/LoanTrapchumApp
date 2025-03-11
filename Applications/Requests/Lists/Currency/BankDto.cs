namespace Applications.Requests.Lists.Currency;

public class BankDto
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
    //public int BankID { get; set; }
    //public string Bankcode { get; set; } = null!;
    //public string AddressRefID { get; set; } = null!;
    //public int? BranchRefID { get; set; } = null!;
    //public string? Description { get; set; }
    //public string? Remark { get; set; }
    //public string? IsClosed { get; set; }
    //public string? IsHead { get; set; }
    //public string? Khmer { get; set; }
    //public string? Contact { get; set; }

}

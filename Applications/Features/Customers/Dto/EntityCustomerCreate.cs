using Applications.Features.OtherNames.Dto;

namespace Applications.Features.Customers.Dto;

public sealed class EntityCustomerCreate : BaseEntityNamesCreate
{
    public string? CustomerId { get; set; }
    public string? CustomerKey { get; set; }
    public int? Dependents { get; set; }
    public int? NoOfChildren { get; set; }
    public decimal? Income { get; set; }
    public decimal? Expenditure { get; set; }
    public string? SpouseName { get; set; }
    public DateTime? SpouseDateOfBirth { get; set; }
    public string? SpouseNationality { get; set; }
    public string? SpousePassport { get; set; }
    public string? SpouseIdCard { get; set; }
    public string? SpousePhoneNo { get; set; }
    public string? SpouseOccupation { get; set; }
    public string? Photo { get; set; } = "/assets/profiles/profile.png";
    public string? SpouseGender { get; set; } = string.Empty;

    public EmergencyContactDtoCreate EmergencyContact { get; set; } = new();
}

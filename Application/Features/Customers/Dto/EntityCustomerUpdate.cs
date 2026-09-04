using Application.Features.OtherNames.Dto;

namespace Application.Features.Customers.Dto;

public sealed class EntityCustomerUpdate : BaseEntityNamesCreate
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
    public string? SpousePhoneNo { get; set; }
    public string? SpouseOccupation { get; set; }
    public string? Photo { get; set; }
}

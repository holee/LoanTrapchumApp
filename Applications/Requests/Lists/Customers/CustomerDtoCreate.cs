using System.ComponentModel.DataAnnotations;
using Applications.Requests.Lists.Addreses;
namespace Applications.Requests.Lists.Customers;


public sealed class CustomerDtoMakeActive  
{
    [Required]
    public long CustomerId { get; set; }
    [Required]
    public int EditSeq { get; set; }
    [Required]
    public bool IsActive { get; set; } 
}
public sealed class CustomerDtoCreate : AddressBaseDto
{

    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? AltName { get; set; }
    public string? EnType { get; set; }
    public string? PassportId { get; set; }
    public string? Nationality { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; } 
    public string? Salutation { get; set; }
    public string? JobTitle { get; set; }
    public string? Phone { get; set; }
    public string? AltPhone { get; set; }
    public string? EmailAddress { get; set; }
    public string? IdCard { get; set; }
    public string? CompanyName { get; set; }
    public string? MarritalStatus { get; set; }
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
    public string? Occupation { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }

}

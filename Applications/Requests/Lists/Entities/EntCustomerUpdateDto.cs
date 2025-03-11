namespace Applications.Requests.Lists.Entities;

public class EntCustomerUpdateDto
{
    public long CustomerId { get; set; } 
    public string PassportId { get; set; } = string.Empty;
    public string NationalityId { get; set; } = string.Empty;
    public string? Occupation { get; set; } 
    public string CompanyName { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public int? Dependents { get; set; }
    public int? NoOfChildren { get; set; }
    public decimal? Income { get; set; }
    public decimal? Expenditure { get; set; }
    public DateTime? DateOfBirth { get; set; } 
    public string? SpouseName { get; set; } = string.Empty;
    public string? MarritalStatus { get; set; }=string.Empty;
    public DateTime? SpouseDateOfBirth { get; set; }
    public string? SpouseNationalityId { get; set; } = string.Empty;
    public string? SpousePassportId { get; set; } = string.Empty; 
    public string? SpousePhoneNo { get; set; } = string.Empty;
    public string? SpouseOccupation { get; set; } = string.Empty;
    public EntityNameDtoUpdate? EntityName { get; set; } 
}

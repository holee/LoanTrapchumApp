namespace Applications.Responses.ViewModels.OtherNames;

public sealed class NameListVm
{
    public string Id { get; set; } = string.Empty;
    public string? FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? FullName { get; set; } = string.Empty;
    public string? AltName { get; set; } = string.Empty;
    public string? Name { get; set; } = string.Empty;
    public string? EntType { get; set; } = string.Empty;
    public string? PassportId { get; set; } = string.Empty;
    public string? Nationality { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; } = DateTime.Now;
    public string? Salutation { get; set; } = string.Empty;
    public string? Gender { get; set; } = string.Empty;
    public string? JobTitle { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? AltPhone { get; set; } = string.Empty;
    public string? EmailAddress { get; set; } = string.Empty;
    public string? IdCard { get; set; } = string.Empty;
    public string? CompanyName { get; set; } = string.Empty;
    public string? MarritalStatus { get; set; } = string.Empty;
    public string? Occupation { get; set; } = string.Empty;
    public bool? IsActive { get; set; } = true;
    public int? EditSeq { get; set; } = 0;
    public DateTime? CreatedDt { get; set; }
    public DateTime? ModifiedDt { get; set; }
    public string? CountryName { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    //public int? ProvinceRefId { get; set; }
    //public string? ProDescription { get; set; } = string.Empty;
    public string? ProInKhmer { get; set; } = string.Empty;
    //public int? DistrictRefId { get; set; }
    //public string? DisDescription { get; set; } = string.Empty;
    public string? DisInKhmer { get; set; } = string.Empty;
    //public int? CommuneRefId { get; set; }
    //public string? ComDescription { get; set; } = string.Empty;
    public string? ComInKhmer { get; set; } = string.Empty;
    //public int? VillageRefId { get; set; }
    //public string? VillDescription { get; set; } = string.Empty;
    public string? VilInKhmer { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
}
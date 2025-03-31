namespace Applications.Requests.Lists.OtherNames;

public sealed class EntityNamesCreate
{
    public Guid Id { get; set; }
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
    public string? Occupation { get; set; }
    public string? MarritalStatus { get; set; }
    public bool? IsActive { get; set; }
    public string? CountryName { get; set; }
    public string? City { get; set; }
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
    public bool Converted { get; set; } = false;

    /// <summary>
    /// For Customer
    /// </summary>
    ///
    public Guid? CustomerId { get; set; } 
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

    /// <summary>
    /// For Employees
    /// </summary>
    public long? StaffId { get; set; }

    public Guid? UserRefId { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? DepartmentRefId { get; set; }
    public int? BranchRefId { get; set; }

    /// <summary>
    /// For Employees And Customer
    /// </summary>
    public string? Photo { get; set; }
}
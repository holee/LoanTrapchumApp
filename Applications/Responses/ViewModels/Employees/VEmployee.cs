namespace Applications.Responses.ViewModels.Employees;

public sealed class VEmployee 
{
    //EntityName
    public long EntId { get; set; }
    public long EmployeeId { get; set; }
    public string? Name { get; set; }
    public string? AltName { get; set; }
    public string? EntType { get; set; } = default!;
    public string? FirstName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string? LastName { get; set; } = default!;
    public string? JobTitle { get; set; }
    public string? Sex { get; set; }
    public string? EmailAddress { get; set; }
    public string? Phone { get; set; }
    //Employee
    public DateTime? Hiredate { get; set; }
    public DateTime? Enddate { get; set; }
    public int? DepartmentRefId { get; set; }
    public int? BranchRefId { get; set; }
    public string? Photo { get; set; }
    public string? AltPhone { get; set; }
    public string? MarritalStatus { get; set; }
    public string? Nationality { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Occupation { get; set; }
    public bool? IsActive { get; set; }
    //Department
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    //Branch
    public int BranchId { get; set; }
    public string BranchName { get; set; } = default!;
    //Address
    public long AddressId { get; set; }
    public string? CountryName { get; set; }
    public string? City { get; set; }
    public int? ProvinceRefId { get; set; }
    public string? ProDescription { get; set; }
    public string? ProInKhmer { get; set; }
    public int? DistrictRefId { get; set; }
    public string? DisInKhmer { get; set; }
    public string? DisDescription { get; set; }
    public int? CommuneRefId { get; set; }
    public string? ComDescription { get; set; }
    public string? ComInKhmer { get; set; }
    public int? VillageRefId { get; set; }
    public string? VillDescription { get; set; }
    public string? VilInKhmer { get; set; }
    public string? FullAddressEng
    {
        get
        { return $"{ProDescription} {DisDescription} {ComDescription} {VillDescription}"; }
    }
    public override string ToString()
    {
        return $"{ProInKhmer} {DisInKhmer} {ComInKhmer} {VilInKhmer}";
    }
}
public class EmployeeActive
{
    public bool? Active { get; set; }
}

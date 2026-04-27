namespace Applications.Features.Vendors.Dto.Responses;

public sealed class VendorDto 
{
    public string? VendorId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AltName { get; set; }
    public string? CountryName { get; set; }
    public string? City { get; set; }
    public string? ProInKhmer { get; set; }
    public string? DisInKhmer { get; set; }
    public string? ComInKhmer { get; set; }
    public string? VilInKhmer { get; set; }
    public string? AltPhone { get; set; }
    public string? EmailAddress { get; set; }
    public string? Phone { get; set; }
    public string? EntType { get; set; }
    public string? JobTitle { get; set; }
    public bool IsActive { get; set; }
    public string? MiddleName { get; set; }
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public string? Salutation { get; set; }
    public string? Description { get; set; }
    public int EditSeq { get; set; } = 0;
    public string? Occupation { get; set; } 
    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public override string ToString()
    {
        return $"{City} {ProInKhmer} {DisInKhmer} {ComInKhmer} {VilInKhmer}";
    }
}

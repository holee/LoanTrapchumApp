namespace Applications.Responses.ViewModels.OtherNames;

public sealed class VOtherNames  
{
    public long EntId { get; set; }
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
    public string? Sex { get; set; }
    public string? Description { get; set; }
    public override string ToString()
    {
        return $"{City} {ProInKhmer} {DisInKhmer} {ComInKhmer} {VilInKhmer}";
    }
}

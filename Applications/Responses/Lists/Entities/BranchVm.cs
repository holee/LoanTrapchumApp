namespace Applications.Responses.Lists.Entities;

public class BranchVm 
{
    public int BranchId { get; set; }
    public string? BranchName { get; set; } = default!;
    public string? Description { get; set; }
    public string? CountryName { get; set; }
    public string? City { get; set; }
    public string? ProInKhmer { get; set; }
    public string? DisInKhmer { get; set; }
    public string? ComInKhmer { get; set; }
    public string? VilInKhmer { get; set; }
    public bool IsActive { get; set; }
}

namespace Applications.Responses.ViewModels.Customers;

public sealed class VCreditAgent
{
    public int Id { get; set; }
    public Guid? EmployeeRefId { get; set; }
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AltName { get; set; }
    public string? Phone { get; set; }
    public string? AltPhone { get; set; }
    public string? EmailAddress { get; set; }
    public string? JobTitle { get; set; }
    public string? Gender { get; set; }
    public bool IsActive { get; set; } = false;
    public int BranchId { get; set; }
    public string? BranchName { get; set; }
    public bool IsRemove { get; set; } = false;
    public int EditSeq { get; set; } = 0;
}

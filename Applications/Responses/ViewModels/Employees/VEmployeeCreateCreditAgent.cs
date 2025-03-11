namespace Applications.Responses.ViewModels.Employees
{
    public class VEmployeeCreateCreditAgent
    {
        public long? EmployeeId { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AltName { get; set; }
        public string? Phone { get; set; }
        public string? AltPhone { get; set; }
        public string? EmailAddress { get; set; }
        public string? JobTitle { get; set; }
        public string? Gender { get; set; }
        public bool IsActive { get; set; } = true;
        public string? BranchName { get; set; }=string.Empty;   

    }
}

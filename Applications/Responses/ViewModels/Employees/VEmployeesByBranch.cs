using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Employees
{
    public class VEmployeesByBranch
    {
        public string? EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? AltName { get; set; }

        [NotMapped]
        public string? EmailAddress { get; set; }
        public string? Phone { get; set; }


        [NotMapped]
        public string? Sex { get; set; }
        public string? Gender { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
    }
}

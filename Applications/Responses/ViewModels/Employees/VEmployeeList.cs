using Applications.Features.Loans.Responses;
using Applications.Responses.ViewModels.Loans.Requests;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Employees;

public sealed class VEmployeeList
{
    public string? EmployeeId { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? AltName { get; set; } = string.Empty;
    public string? Gender { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? AltPhone { get; set; } = string.Empty;
    public string? EmailAddress { get; set; } = string.Empty;
    public string? IdCard { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int EditSeq { get; set; } = 0;
    public string? CountryName { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? ProInKhmer { get; set; } = string.Empty;
    public string? DisInKhmer { get; set; } = string.Empty;
    public string? ComInKhmer { get; set; } = string.Empty;
    public string? VilInKhmer { get; set; } = string.Empty;
    public DateTime? HireDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Photo { get; set; } = string.Empty;
    public string? Occupation { get; set; } = string.Empty;
    public string? MarritalStatus { get; set; } = string.Empty;
    public string? Nationality { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Description { get; set; } = string.Empty;
    public int? BranchId { get; set; }
    public string? BranchName { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public string? UserName { get; set; }

    public string? ManagerId { get; set; } = string.Empty;
    public DateTimeOffset? ModifiedDT { get; set; } 
    public override string ToString()
    {
        return $"{DisInKhmer!} {DisInKhmer!} {ComInKhmer!} {VilInKhmer!}";
    }

    [NotMapped]
    public string FullName
    {
        get
        {
            return $"{FirstName} {LastName}";
        }
    }


    [NotMapped]
    public int RowIndex { get; set; } = 1;
    public ICollection<VLoanDisbursments> LoanDisbursments { get; set; } = [];
    public ICollection<VLoanRequests> LoanRequests { get; set; } = [];
}

using Applications.Responses.Lists.Collaterals;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Customers;

public sealed class VCustomerDetails 
{
    public string? CustomerId { get; set; }
    public string? FullName { get; set; }
    public string? AltName { get; set; }
    public string? EntType { get; set; }
    public string? PassportId { get; set; }
    public string? Nationality { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Salutation { get; set; }
    public string? JobTitle { get; set; }
    public string? Phone { get; set; }
    public string? AltPhone { get; set; }
    public string? EmailAddress { get; set; }
    public string? IdCard { get; set; }
    public string? MarritalStatus { get; set; }
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
    public string? Occupation { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public bool IsLoanActive { get; set; } = false;
    public int? Circle { get; set; }
    public string? Photo { get; set; } = string.Empty;
    [NotMapped] public List<VCustomerExpenses> CustomerExpenses { get; set; } = [];
    [NotMapped] public List<VCustomerIncomes> CustomerIncomes { get; set; } = [];
    [NotMapped] public List<VCollateralLines> CollateralLines { get; set; } = [];
    [NotMapped] public List<VCustomerLoanRequests> LoanRequests { get; set; } = [];
    [NotMapped] public List<VCustomerLoans> Loans { get; set; } = [];
    [NotMapped] public List<VCustomerNotes> CustomerNotes { get; set; } = [];
    [NotMapped] public List<VCustomerActivities> CustomerActivities { get; set; } = [];
}

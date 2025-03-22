using Domains.Models.Bases;
using Domains.Models.Collaterals;
using Domains.Models.Loans;

namespace Domains.Models.Lists.Customers;


public sealed class Customer : BaseAddress
{
    public Guid Id { get; set; } 
    public long CustomerId { get; set; }
    public Guid? CustomerCode { get; set; } = Guid.NewGuid();
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? AltName { get; set; }
    public string? EnType { get; set; }
    public string? PassportId { get; set; }
    public string? Nationality { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Salutation { get; set; }
    public string? JobTitle { get; set; }
    public string? Phone { get; set; }
    public string? AltPhone { get; set; }
    public string? EmailAddress { get; set; }
    public string? IdCard { get; set; }
    public string? CompanyName { get; set; }
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
    public string? Photo { get; set; }
    public bool? IsActive { get; set; } = true;
    public bool? IsLoanActive { get; set; } = false;
    public Collateral? Collateral { get; set; }
    public ICollection<CustomerExpense> CustomerExpenses { get; set; } = [];
    public ICollection<CustomerIncome> CustomerIncomes { get; set; } = [];
    public ICollection<LoanRequest> LoanRequests { get; set; } = [];

}

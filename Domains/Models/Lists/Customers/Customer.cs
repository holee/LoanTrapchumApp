using Domains.Models.Collaterals;
using Domains.Models.Loans;

namespace Domains.Models.Lists.Customers;

public sealed class Customer
{
    public Guid? CustomerId { get; set; }
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
    public string? Photo { get; set; }
    public bool? IsLoanActive { get; set; } = false;
    public Collateral? Collateral { get; set; }
    public ICollection<CustomerExpense> CustomerExpenses { get; set; } = [];
    public ICollection<CustomerIncome> CustomerIncomes { get; set; } = [];
    public ICollection<LoanRequest> LoanRequests { get; set; } = [];
    public EntityNames? EntityNames { get; set; } 
}
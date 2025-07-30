using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;

namespace Applications.Responses.ViewModels.Customers;



public sealed class VCustomerList
{
    public string? CustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
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
    public int? EditSeq { get; set; }
    public string? CountryName { get; set; }
    public string? City { get; set; }
    public string? ProInKhmer { get; set; }
    public string? DisInKhmer { get; set; }
    public string? ComInKhmer { get; set; }
    public string? VilInKhmer { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsLoanActive { get; set; } = false;
    public int? Circle { get; set; }
    public string? Photo { get; set; }=string.Empty;

    public DateTime? ModifiedDT { get; set; }
    public override string ToString()
    {
        return $"{ProInKhmer} {DisInKhmer} {ComInKhmer} {VilInKhmer}";
    }


    public List<VCustomerExpenses> CustomerExpenses { get; set; } = [];
    public List<VCustomerIncomes> CustomerIncomes { get; set; } = [];
    public List<VCollateralLines> CollateralLines { get; set; } = [];
    public List<VLoanRequests> LoanRequests { get; set; } = [];
    public List<VLoanDisbursments> Loans { get; set; } = [];
    public List<VCustomerNotes> CustomerNotes { get; set; } = [];
    public List<VCustomerActivities> CustomerActivities { get; set; } = [];
}

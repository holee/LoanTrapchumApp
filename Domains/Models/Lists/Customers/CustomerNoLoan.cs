namespace Domains.Models.Lists.Customers;

public class CustomerNoLoan
{
    public long CustomerId { get; set; }
    public string? FullName { get; set; }
    public string? AltName { get; set; }
    public string? Phone { get; set; }
    public string? Salutation { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? Circle { get; set; }
    public string? IdCard { get; set; }
    public bool? IsLoanActive { get; set; }
    public string? Address { get; set; } 
}

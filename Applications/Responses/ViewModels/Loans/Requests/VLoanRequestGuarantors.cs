namespace Applications.Responses.ViewModels.Loans.Requests;

public sealed class VLoanRequestGuarantors
{

    public long? Id { get; set; }
    public int? BranchId { get; set; }
    public Guid? LoanRequestId { get; set; }
    public Guid? CustomerId { get; set; } 
    public Guid? EntId { get; set; }
    public string? CurrencyId { get; set; }
    public string? FullName { get; set; }
    public string? AltName { get; set; }
    public string? Phone { get; set; }
    public string? VilInKhmer { get; set; }
    public string? ComInKhmer { get; set; }
    public string? DisInKhmer { get; set; }
    public string? ProInKhmer { get; set; }
    public bool? IsActive { get; set; }
    public int? NoOfLoan { get; set; }
    public override string ToString()
    {
        return $"{VilInKhmer} {ComInKhmer} {DisInKhmer} {ProInKhmer}";
    }
}

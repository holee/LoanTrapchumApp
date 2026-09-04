namespace Application.Responses.ViewModels.Loans.Requests;



public sealed class VLoanRequestGuarantors
{

    public long? Id { get; set; }
    public int? BranchId { get; set; }
    public string? LoanRequestId { get; set; }
    public string? CustomerId { get; set; }
    public string? GuarantorId { get; set; }//GuarantorId
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

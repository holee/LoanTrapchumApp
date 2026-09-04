using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.Loans.Dtos.Responses;

public sealed class VLoanDisbursementGuarantors 
{
    public long Id { get; set; }
    public string? GuarantorId { get; set; }
    public string? LoanId { get; set; }
    public string GuarantorName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? ProInKhmer { get; set; } = string.Empty;
    public string? DisInKhmer { get; set; } = string.Empty;
    public string? ComInKhmer { get; set; } = string.Empty;
    public string? VilInKhmer { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    [NotMapped] public int RowIndex { get; set; } = 1;

}

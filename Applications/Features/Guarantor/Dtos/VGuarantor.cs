using System.ComponentModel.DataAnnotations.Schema;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Responses.ViewModels.Loans.Disbursements;
using Applications.Responses.ViewModels.Loans.Requests;

namespace Applications.Features.Guarantor.Dtos;

public sealed class VGuarantor
{
    public string? Id { get; set; }
    public string? FullName { get; set; }
    public string? GuarantorName { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? ProInKhmer { get; set; }
    public string? DisInKhmer { get; set; }
    public string? ComInKhmer { get; set; }
    public string? VilInKhmer { get; set; }
    public bool IsActive { get; set; } = false;
    public int NumberOfLoan { get; set; }
    public int Requests { get; set; }  

    [NotMapped]
    public bool Selected { get; set; } = false;
    [NotMapped] public ICollection<VLoanGuarantors> LoanGuarantors { get; set; } = [];
    [NotMapped] public ICollection<VGuarantorsLoanRequest> LoanRequests { get; set; } = [];
    [NotMapped] public ICollection<VGuarantorLoan> GuarantorLoans { get; set; } = [];
    public override string ToString()
    {
        return $"{ProInKhmer} {DisInKhmer} {ComInKhmer} {VilInKhmer}";
    }

}

using System.ComponentModel.DataAnnotations.Schema;
using Applications.Responses.ViewModels.Loans;

namespace Applications.Responses.ViewModels.OtherNames;

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
    [NotMapped]
    public bool Selected { get;set; } = false;
    [NotMapped]
    public ICollection<VLoanGuarantors> LoanGuarantors { get; set; } = [];
    public override string ToString()
    {
        return $"{ProInKhmer} {DisInKhmer} {ComInKhmer} {VilInKhmer}";
    }

}

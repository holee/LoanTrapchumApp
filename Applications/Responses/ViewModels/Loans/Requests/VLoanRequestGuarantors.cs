using Applications.Requests.Lists.Entities.Guarantors;

namespace Applications.Responses.ViewModels.Loans.Requests;


public static class VLoanRequestGuarantorsExtensions
{
    public static LoanGuarantorDtoCreate ToGuarantorDtoCreate(this VLoanRequestGuarantors guarantor)
    {
        return new LoanGuarantorDtoCreate
        {
            Id = guarantor.Id,
            BranchId = guarantor.BranchRefId,
            LoanRequestRefId = guarantor.LoanRequestRefId,
            EntityNameRefId = guarantor.EmployeeRefId,
            IsActive = true
        };
    }
    public static LoanGuarantorDtoCreate ToGuarantorDtoCreate(this VLoanRequestGuarantors guarantor, int? branchId)
    {
        return new LoanGuarantorDtoCreate
        {
            Id = guarantor.Id,
            BranchId = branchId,
            LoanRequestRefId = guarantor.LoanRequestRefId,
            EntityNameRefId = guarantor.EmployeeRefId,
            IsActive = true
        };
    }

    public static IEnumerable<LoanGuarantorDtoCreate> ToGuarantorDtoCreate(this List<VLoanRequestGuarantors> guarantors, int? branchId)
    {
        return guarantors.Select(g => new LoanGuarantorDtoCreate
        {
            Id = g.Id,
            BranchId = branchId,
            LoanRequestRefId = g.LoanRequestRefId,
            EntityNameRefId = g.EmployeeRefId,
            IsActive = true
        }).ToList();
    }


}


public sealed class VLoanRequestGuarantors
{

    public long? Id { get; set; }
    public int? BranchRefId { get; set; }
    public string? LoanRequestRefId { get; set; }
    public string? CustomerRefId { get; set; }
    public string? EmployeeRefId { get; set; }
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

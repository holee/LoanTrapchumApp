using Applications.Features.Entities.Guarantors;
using Applications.Features.Entities.Guarantors;

namespace Applications.Responses.ViewModels.Loans.Requests;

public static class VLoanRequestGuarantorsExtensions
{
    public static LoanGuarantorDtoCreate ToGuarantorDtoCreate(this VLoanRequestGuarantors guarantor)
    {
        return new LoanGuarantorDtoCreate
        {
            Id = guarantor.Id,
            BranchId = guarantor.BranchId,
            LoanRequestRefId = guarantor.LoanRequestId,
            EntityNameRefId = guarantor.GuarantorId,
            IsActive = true
        };
    }

    public static LoanGuarantorDtoCreate ToGuarantorDtoCreate(this VLoanRequestGuarantor guarantor)
    {
        return new LoanGuarantorDtoCreate
        {
            Id = guarantor.Id,
            BranchId = guarantor.BranchId,
            LoanRequestRefId = guarantor.LoanRequestRefId,
            EntityNameRefId = guarantor.GuarantorId,
            IsActive = true
        };
    }



    public static LoanGuarantorDtoCreate ToGuarantorDtoCreate(this VLoanRequestGuarantors guarantor, int? branchId)
    {
        return new LoanGuarantorDtoCreate
        {
            Id = guarantor.Id,
            BranchId = branchId,
            LoanRequestRefId = guarantor.LoanRequestId,
            EntityNameRefId = guarantor.GuarantorId,
            IsActive = true
        };
    }

    public static IEnumerable<LoanGuarantorDtoCreate> ToGuarantorDtoCreate(this List<VLoanRequestGuarantor> guarantors, int? branchId)
    {
        return guarantors.Select(g => new LoanGuarantorDtoCreate
        {
            Id = g.Id,
            BranchId = branchId,
            LoanRequestRefId = g.LoanRequestRefId,
            EntityNameRefId = g.GuarantorId,
            IsActive = true
        }).ToList();
    }


}

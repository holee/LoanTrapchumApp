using Applications.Features.Branches.Dtos;
using Applications.Interfaces.Branches;
using ErrorOr;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Applications.Features.Branches.Queries.GetUpdateBranch;

public class GetUpdateBranchQueryHandler(IBranchService branchService) : IRequestHandler<GetUpdateBranchQuery, ErrorOr<BranchDtoUpdate>>
{
    public async Task<ErrorOr<BranchDtoUpdate>> Handle(GetUpdateBranchQuery request, CancellationToken cancellationToken)
    {

        var rsp = await branchService.GetBranchById(request.BranchId, cancellationToken);

        if (rsp.IsError)
        {
            return rsp.Errors;
        }

        var model = rsp.Value;

        var result = new BranchDtoUpdate
        {
            BranchId = model.BranchId,
            BranchName = model.BranchName,
            BankRefId = model.BankRefId,
            VilInKhmer = model.VilInKhmer,
            VillageRefId = model.VillageRefId,
            EditSeq = model.EditSeq,
            Description = model.Description,
            City = model.City,
            CommuneRefId = model.CommuneRefId,
            ComDescription = model.ComDescription,
            ComInKhmer = model.ComInKhmer,
            CountryName = model.CountryName,
            DistrictRefId = model.DistrictRefId,
            DisInKhmer = model.DisInKhmer,
            ProvinceRefId = model.ProvinceRefId,
            ProInKhmer = model.ProInKhmer,
            ImagePath = model.LtDocument is not null ? model.LtDocument.FileUrl : string.Empty,
            ImagePathId = model.ImagePathId,
            IsActive = model.IsActive,
        };

        return result;
    }

}
using Application.Features.LoanRequests.Requests;
using Domains.Models.Loans;
namespace Application.Commons.Mappers
{
    public static class LoanDocumentMapper
    {
        public static LoanDocuments DocumentDtoCreate(this LoanDocumentDtoCreate dto)
        {
            return new LoanDocuments
            {
                Id = dto.Id,
                BranchId = dto.BranchId,
                LoanRequestId = dto.LoanRequestId,
                OriginalName = dto.OriginalName,
                NewFileName = dto.NewFileName,
                AltText = dto.AltText,
                FileSize = dto.FileSize,
                FileUrl = dto.FileUrl,
                Description = dto.Description,
            };
        }

        public static List<LoanDocuments> DocumentsDtoCreate(this List<LoanDocumentDtoCreate> dto)
        {
            return dto.Select(x => x.DocumentDtoCreate()).ToList();
        }

        public static LoanDocumentDtoCreate ToDocumentDtoEdit(this LoanDocuments entity)
        {
            return new LoanDocumentDtoCreate
            {
                Id = entity.Id,
                BranchId = entity.BranchId,
                LoanRequestId = entity.LoanRequestId,
                OriginalName = entity.OriginalName,
                NewFileName = entity.NewFileName,
                AltText = entity.AltText,
                FileSize = entity.FileSize,
                FileUrl = entity.FileUrl,
                Description = entity.Description
            };
        }
    }
}

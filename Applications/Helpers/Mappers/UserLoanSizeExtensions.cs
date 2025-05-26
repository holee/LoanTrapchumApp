using Applications.Requests.Settings;
using Domains.Models.Loans;

namespace Applications.Helpers.Mappers
{
    public static class UserLoanSizeExtensions
    {
        public static UserLoanSize MapToCreate(this UserLoanSizeDtoCreate source)
        {
            return new UserLoanSize
            {
                 UserRefId=source.UserRefId,
                 MinUsdLoanSize = source.MinUsdLoanSize,
                 MaxUsdLoanSize= source.MaxUsdLoanSize,
                 MinKhrLoanSize= source.MinKhrLoanSize,
                 MaxKhrLoanSize=source.MaxKhrLoanSize,
                 IsActive=true,
                 Description=source.Description,
                 StartDate=source.StartDate,
                 EndDate=source.EndDate,
            };
        }

        public static UserLoanSize MapToEdit(this UserLoanSizeDtoCreate source)
        {
            return new UserLoanSize
            {
                Id=source.Id,
                UserRefId = source.UserRefId,
                MinUsdLoanSize = source.MinUsdLoanSize,
                MaxUsdLoanSize = source.MaxUsdLoanSize,
                MinKhrLoanSize = source.MinKhrLoanSize,
                MaxKhrLoanSize = source.MaxKhrLoanSize,
                IsActive = source.IsActive,
                Description = source.Description,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                EditSeq=source.EditSeq
            };
        }
    }
}

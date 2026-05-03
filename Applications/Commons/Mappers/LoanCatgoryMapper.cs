using Applications.Responses.Loans.Disbursements;
using Domains.Models.Loans;
using Applications.Features.Categories.Requests;



namespace Applications.Commons.Mappers
{
    public static class LoanCatgoryMapper
    {

        public static LoanCategory MapCreate(this LoanCategoryDtoCreate entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return new LoanCategory
            {
                LoanCategoryCode = entity.LoanCategoryCode ?? string.Empty,
                Description = entity.Description ?? string.Empty,
                DesInKhmer = entity.DesInKhmer ?? string.Empty,
                Code = entity.Code ?? string.Empty,
                IsActive = entity.IsActive,
            };
        }

        public static LoanCategoryDto Map(this LoanCategory entity) 
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return new LoanCategoryDto
            {
                LoanCategoryId = entity.LoanCategoryId,
                LoanCategoryCode = entity.LoanCategoryCode ?? string.Empty,
                Description = entity.Description ?? string.Empty,
                DesInKhmer = entity.DesInKhmer ?? string.Empty,
                Code = entity.Code ?? string.Empty,
                IsActive = entity.IsActive,
                EditSeq = entity.EditSeq // Assuming BaseEntity has EditSeq
            };
        }

        public static IEnumerable<LoanCategoryDto> Maps(this IEnumerable<LoanCategory> source)
        {
            return source.Select(x => x.Map()).ToList();
        }
        public static LoanCategory MapEdit(this LoanCategoryDto response) 
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return new LoanCategory
            {
                LoanCategoryId = response.LoanCategoryId,
                LoanCategoryCode = response.LoanCategoryCode ?? string.Empty,
                Description = response.Description ?? string.Empty,
                DesInKhmer = response.DesInKhmer ?? string.Empty,
                Code = response.Code ?? string.Empty,
                IsActive = response.IsActive,
                EditSeq = response.EditSeq // Assuming BaseEntity has EditSeq
            };
        }
    }
}

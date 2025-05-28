using Applications.Requests.Loans.Categories;
using Applications.Responses.Loans.Disbursements;
using Domains.Models.Loans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static LoanCategoryResponse Map(this LoanCategory entity) 
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return new LoanCategoryResponse
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

        public static IEnumerable<LoanCategoryResponse> Maps(this IEnumerable<LoanCategory> source)
        {
            return source.Select(x => x.Map()).ToList();
        }
        public static LoanCategory MapEdit(this LoanCategoryResponse response) 
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

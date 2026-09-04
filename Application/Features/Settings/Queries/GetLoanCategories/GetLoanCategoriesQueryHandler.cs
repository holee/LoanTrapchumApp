using Application.Responses.Loans.Disbursements;

namespace Application.Features.Settings.Queries.GetLoanCategories;

public sealed class GetLoanCategoriesQueryHandler(ILoanSettingService categoryService) : IRequestHandler<GetLoanCategoriesQuery, Pager<LoanCategoryDto>>
{
    public Task<Pager<LoanCategoryDto>> Handle(GetLoanCategoriesQuery request, CancellationToken cancellationToken = default)
    {
        var param = request.Param ?? new QueryParam();

        return categoryService.GetLoanCategories(request.Id, param);
    }
}

using Applications.Responses.Loans.Disbursements;

namespace Applications.Features.Settings.Queries.GetLoanCategories;

public sealed record GetLoanCategoriesQuery(int? Id, QueryParam? Param)
    : IRequest<Pager<LoanCategoryDto>>;

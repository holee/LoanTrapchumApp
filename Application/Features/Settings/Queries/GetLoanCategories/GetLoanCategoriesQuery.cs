using Application.Responses.Loans.Disbursements;

namespace Application.Features.Settings.Queries.GetLoanCategories;

public sealed record GetLoanCategoriesQuery(int? Id, QueryParam? Param)
    : IRequest<Pager<LoanCategoryDto>>;

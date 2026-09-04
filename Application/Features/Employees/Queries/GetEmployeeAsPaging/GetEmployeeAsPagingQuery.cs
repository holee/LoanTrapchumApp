using Application.Responses.ViewModels.Employees;

namespace Application.Features.Employees.Queries.GetEmployeeAsPaging;

public sealed record GetEmployeeAsPagingQuery(int? BranchId, string? EmployeeId, QueryParam param) : IRequest<Pager<VEmployeeList>>;

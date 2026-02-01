using Applications.Responses.ViewModels.Employees;

namespace Applications.Features.Employees.Queries.GetEmployeeAsPaging;

public sealed record GetEmployeeAsPagingQuery(int? BranchId, string? EmployeeId, QueryParam param) : IRequest<Pager<VEmployeeList>>;

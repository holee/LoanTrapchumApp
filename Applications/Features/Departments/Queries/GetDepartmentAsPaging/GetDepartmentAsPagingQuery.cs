namespace Applications.Features.Departments.Queries.GetDepartmentAsPaging;

public record GetDepartmentAsPagingQuery(int? DepartmentId, QueryParam Param) : IRequest<Pager<VDepartment>>;

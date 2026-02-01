using Applications.Responses.ViewModels.Employees;
using ErrorOr;

namespace Applications.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeDetailsByIdQuery(string? EmployeeId, int? BranchId, bool Include = true) : IRequest<ErrorOr<VEmployeeList>>;

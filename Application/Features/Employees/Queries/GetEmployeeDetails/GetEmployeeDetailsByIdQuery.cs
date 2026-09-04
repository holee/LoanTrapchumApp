using Application.Responses.ViewModels.Employees;
using ErrorOr;

namespace Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeDetailsByIdQuery(string? EmployeeId, int? BranchId, bool Include = true) : IRequest<ErrorOr<VEmployeeList>>;

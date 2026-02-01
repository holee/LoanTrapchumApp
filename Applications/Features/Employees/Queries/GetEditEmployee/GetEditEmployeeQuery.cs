using Applications.Features.Employees.Dto;
using ErrorOr;

namespace Applications.Features.Employees.Queries.GetEditEmployee;

public record GetEditEmployeeQuery(string? EmployeeId) : IRequest<ErrorOr<EntityEmployeeCreate>>;

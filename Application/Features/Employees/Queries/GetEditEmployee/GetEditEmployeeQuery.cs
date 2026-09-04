using Application.Features.Employees.Dto;
using ErrorOr;

namespace Application.Features.Employees.Queries.GetEditEmployee;

public record GetEditEmployeeQuery(string? EmployeeId) : IRequest<ErrorOr<EntityEmployeeCreate>>;

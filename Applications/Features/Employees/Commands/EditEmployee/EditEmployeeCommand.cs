using Applications.Features.Employees.Dto;
using Applications.Utils;
using ErrorOr;
namespace Applications.Features.Employees.Commands.EditEmployee;

public record EditEmployeeCommand(EntityEmployeeCreate Request) : IRequest<ErrorOr<string>>;

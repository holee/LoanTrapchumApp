using Applications.Features.Employees.Dto;
using Applications.Responses.ViewModels.Employees;
using Applications.Utils;
using ErrorOr;
namespace Applications.Features.Employees.Commands.EditEmployee;

public record EditEmployeeCommand(EntityEmployeeCreate Request) : IRequest<ErrorOr<VEmployeeList>>;

using Applications.Features.Employees.Dto;
using Applications.Responses.ViewModels.Employees;
using Applications.Utils;
using ErrorOr;
namespace Applications.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(EntityEmployeeCreate Request) : IRequest<ErrorOr<VEmployeeList>>;

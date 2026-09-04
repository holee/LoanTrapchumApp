using Application.Features.Employees.Dto;
using Application.Responses.ViewModels.Employees;
using Application.Utils;
using ErrorOr;
namespace Application.Features.Employees.Commands.EditEmployee;

public record EditEmployeeCommand(EntityEmployeeCreate Request) : IRequest<ErrorOr<VEmployeeList>>;

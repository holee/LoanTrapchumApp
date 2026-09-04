using ErrorOr;
namespace Application.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(EntityEmployeeCreate Request) : IRequest<ErrorOr<VEmployeeList>>;

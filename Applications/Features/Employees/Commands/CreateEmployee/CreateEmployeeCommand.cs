using ErrorOr;
namespace Applications.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(EntityEmployeeCreate Request) : IRequest<ErrorOr<VEmployeeList>>;

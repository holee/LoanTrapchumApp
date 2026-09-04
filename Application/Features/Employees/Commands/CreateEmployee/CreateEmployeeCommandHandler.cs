using Application.Interfaces.Employees;
using ErrorOr;
using Application.Commons.Mappers;
namespace Application.Features.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler(IEmployeeService empService) : IRequestHandler<CreateEmployeeCommand, ErrorOr<VEmployeeList>>
{
    public async Task<ErrorOr<VEmployeeList>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken = default)
    {
        var emp = request.Request.ToEmployeeCreate();

        return await empService.CreateAsync(emp);
    }
}

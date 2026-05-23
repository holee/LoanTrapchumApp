using Applications.Commons.Mappers;
using Applications.Interfaces.Employees;
using ErrorOr;
namespace Applications.Features.Employees.Commands.CreateEmployee;

public sealed class EditEmployeeCommandHandler(IEmployeeService empService) : IRequestHandler<EditEmployeeCommand, ErrorOr<VEmployeeList>>
{
    public async Task<ErrorOr<VEmployeeList>> Handle(EditEmployeeCommand request, CancellationToken cancellationToken = default)
    {
        
        var emp = request.Request.ToEmployeeEdit();

        return await empService.UpdateAsync(emp,cancellationToken); 

    }
}

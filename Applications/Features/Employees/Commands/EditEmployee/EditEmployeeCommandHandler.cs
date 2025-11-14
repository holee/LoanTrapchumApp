using Applications.Commons.Mappers;
using Applications.Interfaces.Employees;
using Applications.Utils;
using ErrorOr;
namespace Applications.Features.Employees.Commands.CreateEmployee;

public sealed class EditEmployeeCommandHandler(IEmployeeService empService) : IRequestHandler<CreateEmployeeCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken = default)
    {
        var emp = request.Request.ToEmployeeEdit();

        var rsp = await empService.UpdateAsync(emp); 

        if (rsp.IsSuccess && rsp.Data is not null)
        {
            return rsp.Data;
        }

        return Error.Unexpected(rsp.ErrMessage);
    }
}

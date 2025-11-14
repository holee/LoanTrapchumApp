using Applications.Interfaces.Employees;
using Applications.Utils;
using ErrorOr;
using Applications.Commons.Mappers;
namespace Applications.Features.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler(IEmployeeService empService) : IRequestHandler<CreateEmployeeCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken = default)
    {
        var emp = request.Request.ToEmployeeCreate();

        var rsp = await empService.CreateAsync(emp);

        if (rsp.IsSuccess && rsp.Data is not null)
        {
            return rsp.Data;
        }

        return Error.Unexpected(rsp.ErrMessage);
    }
}

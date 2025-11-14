using Applications.Interfaces.Employees;
using Applications.Utils;
using ErrorOr;
namespace Applications.Features.Employees.Commands.DeleteEmployee;

public sealed class DeleteEmployeeCommandHandler(IEmployeeService empService) : IRequestHandler<DeleteEmployeeCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken = default)
    {
        var rsp = await empService.DeleteAsync(request.EmployeeId);

        if (rsp.IsSuccess && rsp.Data is not null)
        {
            return rsp.Data;
        }

        return Error.Unexpected(rsp.ErrMessage);
    }
}

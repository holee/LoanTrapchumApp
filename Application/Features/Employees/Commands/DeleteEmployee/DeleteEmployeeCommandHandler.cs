using Application.Interfaces.Employees;
using ErrorOr;
namespace Application.Features.Employees.Commands.DeleteEmployee;

public sealed class DeleteEmployeeCommandHandler(IEmployeeService empService) : IRequestHandler<DeleteEmployeeCommand, ErrorOr<DeleteResult>>
{
    public async Task<ErrorOr<DeleteResult>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var rsp = await empService.DeleteAsync(request.EmployeeId, request.BranchId, cancellationToken);

            return rsp;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }


    }
}

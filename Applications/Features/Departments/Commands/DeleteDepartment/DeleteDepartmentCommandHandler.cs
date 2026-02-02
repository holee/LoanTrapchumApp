using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler(IDepartmentService departmentService) : IRequestHandler<DeleteDepartmentCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {

        return await departmentService.DeleteAsync(request.DepartmentId, cancellationToken);
    }
}

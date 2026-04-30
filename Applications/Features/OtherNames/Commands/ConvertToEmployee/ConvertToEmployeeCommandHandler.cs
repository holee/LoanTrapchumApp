using Applications.Interfaces.Lists;
using Domains.Models.Lists.Employees;
using ErrorOr;
using Mapster;

namespace Applications.Features.OtherNames.Commands.ConvertToEmployee;

public sealed class ConvertToEmployeeCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<ConvertToEmployeeCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(ConvertToEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = request.Dto.Adapt<Employee>();
        return await otherNameService.ConvertToEmployeeAsync(request.Id, employee, cancellationToken);
    }
}

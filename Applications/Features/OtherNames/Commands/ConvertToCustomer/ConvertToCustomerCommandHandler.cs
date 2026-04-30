using Applications.Interfaces.Lists;
using ErrorOr;
using Mapster;
using Domains.Models.Lists.Customers;

namespace Applications.Features.OtherNames.Commands.ConvertToCustomer;

public sealed class ConvertToCustomerCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<ConvertToCustomerCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(ConvertToCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = request.Dto.Adapt<Customer>();
        return await otherNameService.ConvertToCustomerAsync(request.Id, customer, cancellationToken);
    }
}

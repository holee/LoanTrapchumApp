using Applications.Features.LoanRequests.Dtos;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.DeleteCoPawnCustomer;

public record DeleteCoPawnCustomerCommand(CoPawnCustomerDto CoPawnRequest) : IRequest<ErrorOr<bool>>;

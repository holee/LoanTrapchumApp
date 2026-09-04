using Application.Features.LoanRequests.Dtos;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.DeleteCoPawnCustomer;

public record DeleteCoPawnCustomerCommand(CoPawnCustomerDto CoPawnRequest) : IRequest<ErrorOr<bool>>;

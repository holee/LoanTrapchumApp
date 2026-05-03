using Applications.Features.LoanRequests.Dtos;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.AddCoPawnCustomer;

public record AddCoPawnCustomerCommand(CoPawnRequestsDtoCreate Request) : IRequest<ErrorOr<CoPawnCustomerDto?>>;

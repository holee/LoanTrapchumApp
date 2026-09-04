using Application.Features.LoanRequests.Dtos;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.AddCoPawnCustomer;

public record AddCoPawnCustomerCommand(CoPawnRequestsDtoCreate Request) : IRequest<ErrorOr<CoPawnCustomerDto?>>;

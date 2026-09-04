using Application.Features.LoanRequests.Dtos;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.AddCoPawnCustomers;

public record AddCoPawnCustomersCommand(int? BranchId, string? RequestId, List<CoPawnRequestsDtoCreate> Request) : IRequest<ErrorOr<IEnumerable<CoPawnCustomerDto?>>>;

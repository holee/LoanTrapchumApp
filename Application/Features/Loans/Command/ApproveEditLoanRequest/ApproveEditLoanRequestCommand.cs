using ErrorOr;

namespace Application.Features.Loans.Command.ApproveEditLoanRequest;

public record ApproveEditLoanRequestCommand(RequestDeleteDtoCreate Reqeust) : IRequest<ErrorOr<AppResult>>;
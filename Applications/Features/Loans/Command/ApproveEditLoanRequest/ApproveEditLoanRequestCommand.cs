using ErrorOr;

namespace Applications.Features.Loans.Command.ApproveEditLoanRequest;

public record ApproveEditLoanRequestCommand(RequestDeleteDtoCreate Reqeust) : IRequest<ErrorOr<AppResult>>;
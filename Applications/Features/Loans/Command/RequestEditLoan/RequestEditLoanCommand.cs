using ErrorOr;

namespace Applications.Features.Loans.Command.RequestEditLoan;  

public record RequestEditLoanCommand(RequestDeleteDtoCreate Request) : IRequest<ErrorOr<AppResult>>;

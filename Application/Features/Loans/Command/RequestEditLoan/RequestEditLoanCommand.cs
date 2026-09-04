using ErrorOr;

namespace Application.Features.Loans.Command.RequestEditLoan;  

public record RequestEditLoanCommand(RequestDeleteDtoCreate Request) : IRequest<ErrorOr<AppResult>>;

using ErrorOr;

namespace Application.Features.Loans.Command.ApproveDeleteReuquest;

public record ApproveDeleteRequestCommand(RequestDeleteDtoCreate Request) : IRequest<ErrorOr<DisburseResult>>;

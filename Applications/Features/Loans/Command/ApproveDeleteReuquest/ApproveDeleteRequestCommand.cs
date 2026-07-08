using ErrorOr;

namespace Applications.Features.Loans.Command.ApproveDeleteReuquest;

public record ApproveDeleteRequestCommand(RequestDeleteDtoCreate Request) : IRequest<ErrorOr<DisburseResult>>;

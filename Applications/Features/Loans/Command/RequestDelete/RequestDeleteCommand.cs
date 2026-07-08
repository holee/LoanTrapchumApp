using ErrorOr;

namespace Applications.Features.Loans.Command.RequestDelete;

public record RequestDeleteCommand(RequestDeleteDtoCreate Request) : IRequest<ErrorOr<DeleteResult>>;


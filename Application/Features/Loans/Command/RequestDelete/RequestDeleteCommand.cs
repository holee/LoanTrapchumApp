using ErrorOr;

namespace Application.Features.Loans.Command.RequestDelete;

public record RequestDeleteCommand(RequestDeleteDtoCreate Request) : IRequest<ErrorOr<DeleteResult>>;


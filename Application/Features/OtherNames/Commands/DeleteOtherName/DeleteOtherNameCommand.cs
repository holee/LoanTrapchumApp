using ErrorOr;

namespace Application.Features.OtherNames.Commands.DeleteOtherName;

public record DeleteOtherNameCommand(string? Id) : IRequest<ErrorOr<bool>>;

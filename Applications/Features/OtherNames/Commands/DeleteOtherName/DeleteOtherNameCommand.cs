using ErrorOr;

namespace Applications.Features.OtherNames.Commands.DeleteOtherName;

public record DeleteOtherNameCommand(string? Id) : IRequest<ErrorOr<bool>>;

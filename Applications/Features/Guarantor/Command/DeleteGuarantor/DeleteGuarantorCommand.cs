using ErrorOr;

namespace Applications.Features.Guarantor.Command.DeleteGuarantor;

public record DeleteGuarantorCommand(string? GuarantorId) : IRequest<ErrorOr<Deleted>>;

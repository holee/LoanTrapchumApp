using ErrorOr;

namespace Application.Features.Guarantor.Command.DeleteGuarantor;

public record DeleteGuarantorCommand(string? GuarantorId) : IRequest<ErrorOr<Deleted>>;

using Application.Features.Guarantor.Dtos;
using ErrorOr;

namespace Application.Features.Guarantor.Command.UpdateGuarantor;

public record UpdateGuarantorCommand(EntityGuarantorCreate Request) : IRequest<ErrorOr<VGuarantor>>;

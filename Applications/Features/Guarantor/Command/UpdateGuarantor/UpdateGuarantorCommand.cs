using Applications.Features.Guarantor.Dtos;
using ErrorOr;

namespace Applications.Features.Guarantor.Command.UpdateGuarantor;

public record UpdateGuarantorCommand(EntityGuarantorCreate Request) : IRequest<ErrorOr<VGuarantor>>;

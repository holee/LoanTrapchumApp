using Applications.Features.Guarantor.Dtos;
using ErrorOr;

namespace Applications.Features.Guarantor.Command.CreateGuarantor;

public record CreateGuarantorCommand(EntityGuarantorCreate Request) : IRequest<ErrorOr<VGuarantor>>;

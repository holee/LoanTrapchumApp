using Application.Features.Guarantor.Dtos;
using ErrorOr;

namespace Application.Features.Guarantor.Command.CreateGuarantor;

public record CreateGuarantorCommand(EntityGuarantorCreate Request) : IRequest<ErrorOr<VGuarantor>>;

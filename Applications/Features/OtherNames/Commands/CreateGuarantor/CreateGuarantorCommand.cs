using Applications.Features.OtherNames.Dto;
using Applications.Features.Guarantor.Dtos;
using ErrorOr;

namespace Applications.Features.OtherNames.Commands.CreateGuarantor;

public record CreateOtherNameGuarantorCommand(EntityNamesCreate Dto) : IRequest<ErrorOr<VGuarantor>>;

using Application.Features.OtherNames.Dto;
using Application.Features.Guarantor.Dtos;
using ErrorOr;

namespace Application.Features.OtherNames.Commands.CreateGuarantor;

public record CreateOtherNameGuarantorCommand(EntityNamesCreate Dto) : IRequest<ErrorOr<VGuarantor>>;

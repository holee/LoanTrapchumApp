using Application.Features.Addresses.Dto;
using ErrorOr;

namespace Application.Features.Addresses.Command.UpdateCity;

public record UpdateCityCommand(CityDtoUpdate City) : IRequest<ErrorOr<CityDto>>;

using Applications.Features.Addresses.Dto;
using ErrorOr;

namespace Applications.Features.Addresses.Command.UpdateCity;

public record UpdateCityCommand(CityDtoUpdate City) : IRequest<ErrorOr<CityDto>>;

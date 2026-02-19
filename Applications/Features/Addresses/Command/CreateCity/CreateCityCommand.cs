using Applications.Features.Addresses.Dto;
using ErrorOr;

namespace Applications.Features.Addresses.Command.CreateCity;

public record CreateCityCommand(CityDtoCreate City) : IRequest<ErrorOr<CityDto>>;

using Application.Features.Addresses.Dto;
using ErrorOr;

namespace Application.Features.Addresses.Command.CreateCity;

public record CreateCityCommand(CityDtoCreate City) : IRequest<ErrorOr<CityDto>>;

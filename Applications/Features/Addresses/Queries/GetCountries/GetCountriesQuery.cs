using Applications.Interfaces.Addresses;

namespace Applications.Features.Addresses.Queries.GetCountries;

public record GetCountriesQuery(int? CountryId, QueryParam Query) : IRequest<PageMetaData<CountryDto>>
{
}

public sealed class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, PageMetaData<CountryDto>>
{
    private readonly IAddressService _addressService;
    public GetCountriesQueryHandler(IAddressService addressService)
    {
        _addressService = addressService;
    }
    public async Task<PageMetaData<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _addressService.GetCountries(request.CountryId, request.Query, cancellationToken);

        return countries;
    }
}
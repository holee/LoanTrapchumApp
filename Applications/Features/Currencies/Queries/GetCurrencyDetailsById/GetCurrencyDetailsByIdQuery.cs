using Applications.Features.Currencies.Responses;
using ErrorOr;

namespace Applications.Features.Currencies.Queries.GetCurrencyDetails;

public record GetCurrencyDetailsByIdQuery(int? Id) : IRequest<ErrorOr<VCurrenciesList>>;

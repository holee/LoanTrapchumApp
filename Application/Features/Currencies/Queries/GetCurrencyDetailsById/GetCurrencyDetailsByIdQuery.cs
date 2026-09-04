using Application.Features.Currencies.Responses;
using ErrorOr;

namespace Application.Features.Currencies.Queries.GetCurrencyDetails;

public record GetCurrencyDetailsByIdQuery(int? Id) : IRequest<ErrorOr<VCurrenciesList>>;

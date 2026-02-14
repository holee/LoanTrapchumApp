using Domains.Models.Currencies;
using ErrorOr;

namespace Applications.Features.Currencies.Queries.GetCurrencyDetails;

public record GetCurrencyByIdQuery(int? CurrencyId) : IRequest<ErrorOr<Currency>>;

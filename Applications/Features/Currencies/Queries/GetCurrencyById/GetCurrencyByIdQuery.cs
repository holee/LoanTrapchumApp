using ErrorOr;
using System.Linq.Expressions;

namespace Applications.Features.Currencies.Queries.GetCurrencyDetails;

public record GetCurrencyByIdQuery(int? CurrencyId, params Expression<Func<Currency, object>>[] includes) : IRequest<ErrorOr<CurrencyDto>>;

using ErrorOr;
using System.Linq.Expressions;

namespace Application.Features.Currencies.Queries.GetCurrencyDetails;

public record GetCurrencyByIdQuery(int? CurrencyId, params Expression<Func<Currency, object>>[] includes) : IRequest<ErrorOr<CurrencyDto>>;

using Application.Features.Currencies.Responses;
using Application.Interfaces.Currencies;

namespace Application.Features.Currencies.Queries.GetCurrencyNotes;

public record GetCurrencyNotesQuery(int? BranchId, int? CurrencyId, QueryParam Param) : IRequest<Pager<VCurrencyNotes>>;

public sealed class GetCurrencyNotesQueryHandler(ICurrencyService currencyService) : IRequestHandler<GetCurrencyNotesQuery, Pager<VCurrencyNotes>>
{
    public Task<Pager<VCurrencyNotes>> Handle(GetCurrencyNotesQuery request, CancellationToken cancellationToken = default)
    {
        return currencyService.GetCurrenciesNotes(request.BranchId, request.CurrencyId, request.Param, cancellationToken);
    }
}
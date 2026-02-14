using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Currencies;
using Domains.Models.Currencies;
using ErrorOr;
using Mapster;

namespace Applications.Features.Currencies.Command.AddCurrencyNote;

public sealed class AddCurrencyNoteCommandHandler(ICurrencyService currencyRepository) : IRequestHandler<AddCurrencyNoteCommand, ErrorOr<VCurrencyCounter>>
{ 
    public async Task<ErrorOr<VCurrencyCounter>> Handle(AddCurrencyNoteCommand request, CancellationToken cancellationToken)
    {

        var addedModel = request.CurrencyCounter.Adapt<CurrencyCounter>();

        return await currencyRepository.AddNote(addedModel);

    }
}

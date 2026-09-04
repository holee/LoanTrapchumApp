using Application.Features.Currencies.Responses;
using Application.Interfaces.Currencies;
using Domains.Models.Currencies;
using ErrorOr;
using Mapster;

namespace Application.Features.Currencies.Command.EditCurrencyNote;

public sealed class EditCurrencyNoteCommandHandler(ICurrencyService currencyRepository) : IRequestHandler<EditCurrencyNoteCommand, ErrorOr<VCurrencyCounter>>
{ 
    public async Task<ErrorOr<VCurrencyCounter>> Handle(EditCurrencyNoteCommand request, CancellationToken cancellationToken)
    {

        var addedModel = request.CurrencyCounter.Adapt<CurrencyCounter>();

        return await currencyRepository.AddNote(addedModel);

    }
}

using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using ErrorOr;

namespace Applications.Features.Currencies.Command.EditCurrencyNote;  
 
public record EditCurrencyNoteCommand(CurrencyNoteDtoUpdate CurrencyCounter) : IRequest<ErrorOr<VCurrencyCounter>>;

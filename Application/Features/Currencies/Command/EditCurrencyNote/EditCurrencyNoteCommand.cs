using Application.Features.Currencies.Dto;
using Application.Features.Currencies.Responses;
using ErrorOr;

namespace Application.Features.Currencies.Command.EditCurrencyNote;  
 
public record EditCurrencyNoteCommand(CurrencyNoteDtoUpdate CurrencyCounter) : IRequest<ErrorOr<VCurrencyCounter>>;

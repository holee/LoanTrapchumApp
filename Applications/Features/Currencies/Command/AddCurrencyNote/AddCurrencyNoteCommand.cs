using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using ErrorOr;

namespace Applications.Features.Currencies.Command.AddCurrencyNote; 
 
public record AddCurrencyNoteCommand(CurrencyNoteDtoCreate CurrencyCounter) : IRequest<ErrorOr<VCurrencyCounter>>;

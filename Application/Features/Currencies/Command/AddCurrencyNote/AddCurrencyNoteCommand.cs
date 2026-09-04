using Application.Features.Currencies.Dto;
using Application.Features.Currencies.Responses;
using ErrorOr;

namespace Application.Features.Currencies.Command.AddCurrencyNote; 
 
public record AddCurrencyNoteCommand(CurrencyNoteDtoCreate CurrencyCounter) : IRequest<ErrorOr<VCurrencyCounter>>;

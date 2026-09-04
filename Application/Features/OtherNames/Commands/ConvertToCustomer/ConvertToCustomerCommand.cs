using Application.Features.OtherNames.Dto;
using ErrorOr;

namespace Application.Features.OtherNames.Commands.ConvertToCustomer;

public record ConvertToCustomerCommand(string? Id, ConvertCreateCustomer Dto) : IRequest<ErrorOr<bool>>;

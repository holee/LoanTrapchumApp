using Applications.Features.OtherNames.Dto;
using ErrorOr;

namespace Applications.Features.OtherNames.Commands.ConvertToCustomer;

public record ConvertToCustomerCommand(string? Id, ConvertCreateCustomer Dto) : IRequest<ErrorOr<bool>>;

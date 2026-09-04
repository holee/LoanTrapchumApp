namespace Application.Features.LoanPurposes.Queries.GetLoanPurposeById;

public record GetLoanPurposeByIdQuery(int Id) : IRequest<LoanPurposeDto?>;

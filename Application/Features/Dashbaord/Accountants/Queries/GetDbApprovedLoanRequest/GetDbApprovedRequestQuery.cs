using ErrorOr;

namespace Application.Features.Dashbaord.Accountants.Queries.GetDbApprovedLoanRequest; 
 
public record GetDbApprovedRequestQuery(int? BranchId) : IRequest<ErrorOr<int>>; 

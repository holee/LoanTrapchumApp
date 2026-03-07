using ErrorOr;

namespace Applications.Features.Dashbaord.Accountants.Queries.GetDbApprovedLoanRequest; 
 
public record GetDbApprovedRequestQuery(int? BranchId) : IRequest<ErrorOr<int>>; 

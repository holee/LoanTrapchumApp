namespace TrapchumApp.Helpers.Constants;

public static class UrlHelper
{
    public const string GetRequestDetails = "app/loan/{LoanRequest?.LoanRequestId}/request/{BranchId}/branches/details?CreditAgentId={CreditAgentId}&CustomerId={LoanRequest?.CustomerId}";
    public const string GetRequestById = "app/loan/{LoanRequest?.LoanRequestId}/request/{BranchId}/branches/details?CreditAgentId={CreditAgentId}&CustomerId={LoanRequest?.CustomerId}";

}  
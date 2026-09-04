namespace Application.Features.LoanRequests.Requests
{
    public sealed class LoanRequestDocumentCreate
    {
        public int BranchId { get; set; }
        public string? LoanRequestRefId { get; set; }
        public string? CustomerRefId { get; set; }

    }
}

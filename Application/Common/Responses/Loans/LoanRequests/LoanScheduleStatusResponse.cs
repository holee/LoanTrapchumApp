namespace Application.Responses.Loans.LoanRequests
{
    public sealed class LoanScheduleStatusResponse
    {
        public int LoanScheduleStatusId { get; set; }
        public string? LoanScheduleStatusCode { get; set; }
        public string? Description { get; set; }
    }
}

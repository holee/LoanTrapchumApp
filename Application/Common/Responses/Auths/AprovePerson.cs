namespace Application.Responses.Auths
{


    public sealed class AprovePerson
    {
        public AprovePerson()
        {
        }

        public AprovePerson(
            string? userId, string? userName, int branchId, string? branchName, string? fullName, decimal? minLoanSize, decimal? maxLoanSize, DateTime? startDate, string? description)
        {
            UserId = userId;
            UserName = userName;
            BranchId = branchId;
            BranchName = branchName;
            FullName = fullName;
            MinLoanSize = minLoanSize;
            MaxLoanSize = maxLoanSize;
            StartDate = startDate;
            Description = description;
        }

        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? FullName { get; set; }
        public decimal? MinLoanSize { get; set; } = 0;
        public decimal? MaxLoanSize { get; set; } = 0;
        public DateTime? StartDate { get; set; }
        public string? Description { get; set; }
    }
}

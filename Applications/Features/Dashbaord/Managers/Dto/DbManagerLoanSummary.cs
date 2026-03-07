namespace Applications.Features.Dashbaord.Managers.Dto;

public sealed class DbManagerLoanSummary
{
    public DbManagerLoanSummary()
    {
        RowIndex = 0;
        NewRequest = 0;
        NumberApproved = 0;
        BranchId = 0;
    }
    public int RowIndex { get; set; }
    public int BranchId { get; set; }
    public int NewRequest { get; set; }
    public int NumberApproved { get; set; }
}
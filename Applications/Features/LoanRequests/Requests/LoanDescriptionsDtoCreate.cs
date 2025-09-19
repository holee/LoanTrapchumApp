namespace Applications.Features.LoanRequests.Requests;

public sealed class LoanDescriptionsDtoCreate
{
    public LoanDescriptionsDtoCreate()
    {
        Memo = string.Empty;
        LoanRequestRefId = string.Empty;
    }
    public LoanDescriptionsDtoCreate(string? memo,string? userRefId)
    {
        Memo = memo;
        UserRefId = userRefId;
    }

    public LoanDescriptionsDtoCreate(string? memo, string? userRefId,string? loanRequestId)
        :this(memo,userRefId)
    {
        LoanRequestRefId = loanRequestId;
    }

    public string? LoanRequestRefId { get; set; }
    public string? Memo { get; set; } = string.Empty;
    public string? UserRefId { get; set; }=string.Empty;
}

public sealed class LoanDescriptionsDtoUpdate
{
    public long Id { get; set; }
    public string? LoanRequestRefId { get; set; }
    public string? Memo { get; set; } = string.Empty;
    public string? UserRefId { get; set; } = string.Empty;
}
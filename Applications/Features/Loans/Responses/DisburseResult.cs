namespace Applications.Features.Loans.Responses
{
    public sealed record DisburseResult(bool success = false, string? message = null); 

}

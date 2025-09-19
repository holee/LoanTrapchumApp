namespace Applications.Features.Loans.Responses
{
    public sealed record DisburseResult(bool success = false, string? message = null);

    public sealed record RequestResult(bool Success = false, string? Message = null);  

}

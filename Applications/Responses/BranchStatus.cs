namespace Applications.Responses;

public sealed record BranchStatus(int BranchId, bool IsOpen, string Message);

namespace Application.Features.LoanRequests.Dtos;

public  class CoPawnRequestsDtoCreate
{
    public Guid? Id { get; set; } = null;
    public int? BranchId { get; set; }
    public string? CustomerId { get; set; }
    public string? LoanRequestId { get; set; } = string.Empty;
    public string? LoanId { get; set; } = string.Empty;
    public bool? Status { get; set; } = true;
    public string? CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; } = string.Empty;
    public string? CustomerAddress { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
}

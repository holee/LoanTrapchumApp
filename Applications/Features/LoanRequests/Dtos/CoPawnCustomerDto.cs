using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Applications.Features.LoanRequests.Dtos;

public sealed class CoPawnCustomerDto
{
    public Guid? Id { get; set; } = null;
    public int? BranchId { get; set; }
    public string? CustomerId { get; set; }
    public string? RequestId { get; set; } = string.Empty;
    public string? LoanId { get; set; } = string.Empty;
    public bool? Status { get; set; } = true;
    public string? CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; } = string.Empty;
    public string? CustomerAddress { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    [NotMapped] public int RowIndex { get; set; } = 0;
}

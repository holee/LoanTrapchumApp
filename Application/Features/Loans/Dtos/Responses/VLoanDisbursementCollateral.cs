using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.Loans.Dtos.Responses;

public sealed class VLoanDisbursementCollateral
{
    public long Id { get; set; }
    public long CollateralLineId { get; set; }
    public string? LoanRequestId { get; set; }

    public string? LoanId { get; set; }

    public string? CollateralRefId { get; set; }

    public int BranchId { get; set; }

    public string? CollateralNo { get; set; }

    public string? ReferenceNo { get; set; }

    public string? Location { get; set; }

    public string? OwnerName { get; set; }

    public int Quantity { get; set; }

    public decimal? MarketPrice { get; set; }

    public decimal? HotPrice { get; set; }

    public decimal? PawnPrice { get; set; }

    public string? TypeName { get; set; }

    public string? SubTypeName { get; set; }

    public bool InUsed { get; set; }

    public string? Description { get; set; }


    [NotMapped]
    public int RowIndex { get; set; }
}

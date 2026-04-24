using System.ComponentModel.DataAnnotations.Schema;
using Applications.Enumerations;
using Applications.Responses.ViewModels.Loans.LoanRequests;

namespace Applications.Responses.ViewModels.Loans.Requests;

public sealed class VLoanRequests
{
    public string LoanRequestId { get; set; } = null!;
    public int? BranchId { get; set; }
    public decimal? RequestAmount { get; set; }
    public decimal? AprovedAmount { get; set; }
    public decimal? InterestRate { get; set; }
    public int? Installments { get; set; }
    public string? InstallmentType { get; set; }
    public string? RepaymentMode { get; set; }
    public string? AprovedUserId { get; set; }
    public string? Summiter { get; set; }
    public string? Remark { get; set; }
    public int? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public int? StatusId { get; set; }
    public string? Status { get; set; }
    public string? CreditAgentId { get; set; }
    public string? UserRequestId { get; set; }
    public string? UserName { get; set; }
    public int? BankId { get; set; }
    public string? BranchName { get; set; }
    public string? CustomerId { get; set; }
    public string? AltName { get; set; }
    public string? FullName { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public int Circle { get; set; } = 0;
    public decimal? CollectionFeeRate { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? AprovedDate { get; set; }
    public DateTime? RequestDate { get; set; }
    public string? InstallmentName { get; set; }
    public int? InstallmentTypeId { get; set; }
    public int? RepaymentModeRefId { get; set; }
    public int? LoanCategoryRefId { get; set; }
    public int? LoanProductRefId { get; set; }
    public string? LoanCategoryName { get; set; } = string.Empty;
    public int? PayEvery { get; set; } = 1;
    public int? CollectionFeeModeId { get; set; } = 1;
    public string? CollectionFeeModeName { get; set; } = string.Empty;
    public string? AprovedBy { get; set; } = string.Empty;
    public string? LoanPurpose { get; set; }= string.Empty;
    [NotMapped] public bool IsCompleted => StatusId == (int)RequestStatus.Completed;
    [NotMapped] public bool IsApproved => StatusId == (int)RequestStatus.Approved;
    [NotMapped]
    public bool IsRequested
    {
        get
        {
            if(!StatusId.HasValue && StatusId == 0)
            {
                return false;
            }

            return StatusId == (int)RequestStatus.Requested ||
                   StatusId == (int)RequestStatus.Checking ||
                   StatusId == (int)RequestStatus.Rechecking;
        }
    }

    [NotMapped]
    public bool IsRequesting
    {
        get
        {
            if (!StatusId.HasValue && StatusId == 0)
            {
                return false;
            }

            return StatusId == (int)RequestStatus.New ||
                   StatusId == (int)RequestStatus.Rechecked;
        }
    }


    [NotMapped] public int RowIndex { get; set; } = 1;
    [NotMapped] public string? Memo { get; set; }
    [NotMapped] public IEnumerable<VLoanRequestGuarantor> Guarantors { get; set; } = [];
    [NotMapped] public IEnumerable<VLoanRequestCollateralLines> Collaterals { get; set; } = [];
    [NotMapped] public IEnumerable<VLoanRequestsDocuments> Documents { get; set; } = [];
    [NotMapped] public IEnumerable<VLoanRequestDescription> Descriptions { get; set; } = [];
    [NotMapped] public VCustomerDetails Customer { get; set; } = new();
}

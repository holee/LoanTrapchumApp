using Applications.Commons.Extensions;
using Applications.Enumerations;
using Applications.Extensions.NumberExtensions;
using Applications.Features.Settings.UseCases;
using Applications.Requests.Loans.Payments;
using Applications.Requests.Transactions.LoanTransactions;
using Applications.Services.Loans;
using Domains.Models.Identity;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments;
public partial class PaymentTab
{
    private readonly ISettingServiceImpl _settingService;

    private readonly ILoanPaymentServiceImpl _loanPaymentService;
    public PaymentTab(ISettingServiceImpl settingService, ILoanPaymentServiceImpl loanPaymentService)
    {
        _settingService = settingService;

        _loanPaymentService = loanPaymentService;
    }

    [Parameter] public int CurrencyId { get; set; }
    [Parameter] public LoanPaymentDtoCreate LoanPaymentCreate { get; set; } = new();
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [Parameter] public EventCallback CloseTab { get; set; } = default!;
    LoanPaymentDtoCreate LoanRepayment { get; set; } = new();
    LoanPaymentDtoCreate OriginalLoanPayment { get; set; } = new();

    bool isBusy = false;

    async Task CloseModal()
    {
        await CloseTab.InvokeAsync();
    }
    string? UserId = string.Empty;
    protected override async Task OnInitializedAsync()
    {
        var auth = await AuthenticationState;

        UserId = auth.User.GetId();

        await base.OnInitializedAsync();
    }

    protected override void OnInitialized()
    {
        OriginalLoanPayment = new LoanPaymentDtoCreate
        {
            RecieveAmount = LoanPaymentCreate.RecieveAmount,
            ChangeAmount = LoanPaymentCreate.ChangeAmount,
            LoanRefId = LoanPaymentCreate.LoanRefId,
            BranchRefId = LoanPaymentCreate.BranchRefId,
            CustomerName = LoanPaymentCreate.CustomerName,
            CurrencyRefId = LoanPaymentCreate.CurrencyRefId,
            CreditAgentRefId = LoanPaymentCreate.CreditAgentRefId,
            PayDate = LoanPaymentCreate.PayDate,
            SubTotal = LoanPaymentCreate.AmountWithPenalty,
            Principle = LoanPaymentCreate.PaidPrinciple,
            Interest = LoanPaymentCreate.PaidInterest,
            CollectionFeeAmount = LoanPaymentCreate.PaidFeeAmount,
            PenaltyAmount = LoanPaymentCreate.PaidPenaltyAmount,
            OfficialReceiptNo = LoanPaymentCreate.OfficialReceiptNo,
            LoanPaymentLines = LoanPaymentCreate.LoanPaymentLines.Select(x => new LoanPaymentLineDtoCreate
            {
                LoanRefId = x.LoanRefId,
                BranchRefId = x.BranchRefId,
                OfficialReceiptNo = LoanPaymentCreate.OfficialReceiptNo,
                InstallmentNo = x.InstallmentNo,
                PaidCollectionFee = x.PaidCollectionFee,
                PaidInterest = x.PaidInterest,
                PaidPrinciple = x.PaidPrinciple,
                PaidPenaltyAmount = x.PaidPenaltyAmount,
                SubTotal = x.TotalWithPenalty,
                LoanScheduleRefId = x.LoanScheduleRefId,
                PayDate = x.PayDate
            }).ToList()
        };

        DoCopyLoanPayment();

        base.OnInitialized();
    }


    void DoCopyLoanPayment()
    {
        LoanRepayment = new LoanPaymentDtoCreate
        {
            RecieveAmount = OriginalLoanPayment.RecieveAmount,
            ChangeAmount = OriginalLoanPayment.ChangeAmount,
            LoanRefId = OriginalLoanPayment.LoanRefId,
            BranchRefId = OriginalLoanPayment.BranchRefId,
            CustomerName = OriginalLoanPayment.CustomerName,
            CurrencyRefId = OriginalLoanPayment.CurrencyRefId,
            CreditAgentRefId = OriginalLoanPayment.CreditAgentRefId,
            PayDate = OriginalLoanPayment.PayDate,
            Principle = OriginalLoanPayment.PaidPrinciple,
            SubTotal = OriginalLoanPayment.AmountWithPenalty,
            Interest = OriginalLoanPayment.PaidInterest,
            CollectionFeeAmount = OriginalLoanPayment.PaidFeeAmount,
            PenaltyAmount = OriginalLoanPayment.PaidPenaltyAmount,
            OfficialReceiptNo = OriginalLoanPayment.OfficialReceiptNo,
            LoanPaymentLines = OriginalLoanPayment.LoanPaymentLines.Select(x => new LoanPaymentLineDtoCreate
            {
                LoanRefId = x.LoanRefId,
                BranchRefId = OriginalLoanPayment.BranchRefId,
                InstallmentNo = x.InstallmentNo,
                PaidCollectionFee = x.PaidCollectionFee,
                PaidInterest = x.PaidInterest,
                PaidPrinciple = x.PaidPrinciple,
                PaidPenaltyAmount = x.PaidPenaltyAmount,
                SubTotal = x.TotalWithPenalty,
                LoanScheduleRefId = x.LoanScheduleRefId,
                OfficialReceiptNo = OriginalLoanPayment.OfficialReceiptNo,
                PayDate = x.PayDate
            }).ToList()
        };

    }



    async Task OnSaveChanges()
    {
        var confirmOption = new ConfirmOptions
        {
            CancelButtonText = "Cancel",
            OkButtonText = "Yes",
        };
        try
        {
            isBusy = true;

            if (LoanRepayment.RecieveAmount <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please provide payment amount.");

                isBusy = false;

                return;
            }
            var tran = new LoanPaymentTransactionDtoCreate
            {
                TransactionsId = $"trx-{Guid.CreateVersion7()}",
                TransactionDate = DateTime.Now,
                TransactionTypeName = TransactionTypes.LP.GetDescription(),
                TranTypeRefId = 20,
                BranchRefId = LoanRepayment.BranchRefId,
                UserRefId = LoanRepayment.UserRefId
            };
            var tranxCashManagement = new TrxCashManagementDtoCreate()
            {
                Id = $"trx-{Guid.CreateVersion7()}",
                CurrencyRefId = CurrencyId,
                TransactionDate = DateTime.Now,
                BusinessDate = DateTime.Now,
                CashStatusRefId = 2,
                BranchRefId = LoanRepayment.BranchRefId,
                BankRefId = 11,
                Amount = LoanRepayment.SubTotal,
                Remark = TransactionTypes.LP.GetDescription()
            };

            tran.CashManagements?.Add(tranxCashManagement);

            LoanRepayment.UserRefId = UserId;

            tran.LoanPayment = LoanRepayment;

            string message = $"Receive money from {LoanRepayment.CustomerName} with amount {LoanRepayment.AmountWithPenalty}";

            var confirmPayment = await DialogService.Confirm(message, "Payment", confirmOption);

            if (confirmPayment == false)
            {
                isBusy = false;

                return;
            }

            var result = await _loanPaymentService.RecievePayment(tran, LoanRepayment.BranchRefId);

            if (result.IsSuccess)
            {
                await CloseTab.InvokeAsync();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
            }

            isBusy = false;
        }
        catch (Exception ex)
        {
            isBusy = false;

            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
    }

    void ReceivePaymentFocus(FocusEventArgs e)
    {
        if (LoanRepayment.RecieveAmount < OriginalLoanPayment.SubTotal)
        {
            LoanRepayment.RecieveAmount = 0;

            LoanRepayment.ChangeAmount = 0;
        }
    }

    void ReceivePaymentChanged(ChangeEventArgs e)
    {
        decimal amount = e.Value!.ToNumer<decimal>();

        if (amount <= 0)
        {
            LoanRepayment.RecieveAmount = OriginalLoanPayment.SubTotal;

            LoanRepayment.ChangeAmount = 0;

            return;
        }

        if (amount >= LoanRepayment.SubTotal)
        {
            LoanRepayment.ChangeAmount = amount - LoanRepayment.SubTotal;
        }

        StateHasChanged();
    }


    void CalculatePenaltyAmount(ChangeEventArgs e)
    {

        var amount = e.Value?.ToNumer<decimal>();

        if (amount < 0)
        {
            LoanRepayment.PenaltyAmount = OriginalLoanPayment.PenaltyAmount;

            return;
        }
        if (amount > OriginalLoanPayment.PenaltyAmount)
        {
            amount = OriginalLoanPayment.PenaltyAmount;

            NotificationService.Notify(NotificationSeverity.Error, "Amount must be less than penalty amount.");

            RestoreDefault();
        }
        else
        {
            LoanRepayment.SubTotal = OriginalLoanPayment.AmountWithoutPenalty + amount;

            LoanRepayment.RecieveAmount = OriginalLoanPayment.AmountWithoutPenalty + amount;
        }

    }

    void RestoreDefault()
    {
        DoCopyLoanPayment();

    }
    private async Task CalculateExhanged(MouseEventArgs args)
    {
        await js.InvokeVoidAsync("alert");
        Console.WriteLine("dsdsdsdsdsd");
    }
}


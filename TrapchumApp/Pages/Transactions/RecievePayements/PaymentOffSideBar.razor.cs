using Applications.Commons.Extensions;
using Applications.Enumerations;
using Applications.Extensions.Enums;
using Applications.Extensions.NumberExtensions;
using Applications.Features.LoanPayments.Command.PayOff;
using Applications.Features.LoanPayments.Command.RecievePayment;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.Transactions.LoanTransactions;
using Applications.Responses.ViewModels.Loans.Schedules;
using Domains.Models.Identity;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Pages.Transactions.Loans.Repayments;

namespace TrapchumApp.Pages.Transactions.RecievePayements;      

public partial class PaymentOffSideBar
{
    [Parameter] public int? CurrencyId { get; set; }
    [Parameter] public int? BranchId { get; set; }
    [Parameter] public string? LoanId { get; set; } = string.Empty;
    [Parameter] public string? CustomerName { get; set; } = string.Empty;
    [Parameter] public string? CreditAgentRefId { get; set; } = string.Empty;
    [Parameter] public VSchedule? Schedule { get; set; } = new VSchedule();
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    private LoanPaymentDtoCreate? LoanRepayment;

    private LoanPaymentDtoCreate? OriginalLoanPayment;

    private bool isBusy;

    private string? _userId = string.Empty;

    void CloseModal() => DialogService.CloseSide(false);

    protected override async Task OnInitializedAsync()
    {
        var auth = await AuthenticationState;

        _userId = auth.User.GetId();

        var loanOutstanding = Schedule?.LoanOutstanding ?? 0;

        var loanRepayment = new LoanPaymentDtoCreate
        {
            RecieveAmount = 0,
            ChangeAmount = 0,
            LoanRefId = LoanId,
            BranchRefId = BranchId,
            CustomerName = CustomerName,
            CurrencyRefId = CurrencyId,
            CreditAgentRefId = Schedule?.CreditAgentId,
            PayDate = DateTime.UtcNow,
            SubTotal = loanOutstanding,
            Principle = loanOutstanding,
            Interest = 0,
            CollectionFeeAmount = 0,
            PenaltyAmount = 0,
            OfficialReceiptNo = "",
            LoanPaymentLines =
            [
                new LoanPaymentLineDtoCreate
                {
                    LoanRefId = LoanId,
                    BranchRefId = BranchId,
                    OfficialReceiptNo = "",
                    InstallmentNo = Schedule?.InstallmentNo ?? 0,
                    PaidCollectionFee = 0,
                    PaidInterest = 0,
                    PaidPrinciple = loanOutstanding,
                    PaidPenaltyAmount = 0,
                    SubTotal = loanOutstanding,
                    LoanScheduleRefId = Schedule?.LoanScheduleId ?? 0,
                    PayDate = DateTime.UtcNow
                }
            ]
        };

        OriginalLoanPayment = CloneLoanPayment(loanRepayment);

        LoanRepayment = CloneLoanPayment(OriginalLoanPayment);

        await base.OnInitializedAsync();
    }

    private static LoanPaymentDtoCreate CloneLoanPayment(LoanPaymentDtoCreate source)
    {
        return new LoanPaymentDtoCreate
        {
            RecieveAmount = source.RecieveAmount,
            ChangeAmount = source.ChangeAmount,
            LoanRefId = source.LoanRefId,
            BranchRefId = source.BranchRefId,
            CustomerName = source.CustomerName,
            CurrencyRefId = source.CurrencyRefId,
            CreditAgentRefId = source.CreditAgentRefId,
            PayDate = source.PayDate,
            SubTotal = source.AmountWithPenalty,
            Principle = source.PaidPrinciple,
            Interest = source.PaidInterest,
            CollectionFeeAmount = source.PaidFeeAmount,
            PenaltyAmount = source.PaidPenaltyAmount,
            OfficialReceiptNo = source.OfficialReceiptNo,
            LoanPaymentLines = source.LoanPaymentLines.Select(line => new LoanPaymentLineDtoCreate
            {
                LoanRefId = line.LoanRefId,
                BranchRefId = line.BranchRefId,
                OfficialReceiptNo = source.OfficialReceiptNo,
                InstallmentNo = line.InstallmentNo,
                PaidCollectionFee = line.PaidCollectionFee,
                PaidInterest = line.PaidInterest,
                PaidPrinciple = line.PaidPrinciple,
                PaidPenaltyAmount = line.PaidPenaltyAmount,
                SubTotal = line.TotalWithPenalty,
                LoanScheduleRefId = line.LoanScheduleRefId,
                PayDate = line.PayDate
            }).ToList()
        };
    }

    void RestoreDefault()
    {
        if (OriginalLoanPayment is null) return;
        LoanRepayment = CloneLoanPayment(OriginalLoanPayment);
    }

    async Task HandleFormSubmit()
    {
        try
        {
            isBusy = true;

            if (LoanRepayment is null || LoanRepayment.RecieveAmount <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please provide payment amount.");
                return;
            }

            var confirmPayment = await DialogService.OpenAsync<ConfirmDialogue>(
                "បញ្ជាក់ការទូទាត់/Payment Confirmation",
                new Dictionary<string, object>
                {
                    { "CustomerName", CustomerName! },
                    { "Amount", LoanRepayment.AmountWithPenalty! },
                    { "CurrencyId", CurrencyId! }
                },
                new DialogOptions
                {
                    Left = "20%",
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = true,
                    CloseTabIndex = 10000
                });

            if (confirmPayment is bool confirmed && !confirmed)
                return;

            LoanRepayment.UserRefId = _userId;

            var transaction = BuildTransaction(LoanRepayment);

            var command = new PayOffCommand(transaction);

            var result = await mediator.Send(command);

            if (result.IsSuccess())
            {
                DialogService.CloseSide(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, result.GetMessage());
            }
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
        finally
        {
            isBusy = false;
        }
    }

    private LoanPaymentTransactionDtoCreate BuildTransaction(LoanPaymentDtoCreate payment)
    {
        var transaction = new LoanPaymentTransactionDtoCreate
        {
            TransactionsId = $"trx-{Guid.CreateVersion7()}",
            TransactionDate = DateTime.Now,
            TransactionTypeName = TransactionTypes.LP.GetDescription(),
            TranTypeRefId = (int)TransactionTypes.LP,
            BranchRefId = payment.BranchRefId ?? 0,
            UserRefId = payment.UserRefId
        };

        var cashManagement = new TrxCashManagementDtoCreate
        {
            Id = $"trx-{Guid.CreateVersion7()}",
            CurrencyRefId = CurrencyId,
            TransactionDate = DateTime.Now,
            BusinessDate = DateTime.Now,
            CashStatusRefId = (int)CashStatusEnum.OUT,
            BranchRefId = BranchId,
            BankRefId = 11,
            Amount = payment.SubTotal,
            Remark = TransactionTypes.LP.GetDescription()
        };

        transaction.CashManagements?.Add(cashManagement);
        transaction.LoanPayment = payment;

        return transaction;
    }

    void ReceivePaymentFocus(FocusEventArgs e)
    {
        if (LoanRepayment is null || OriginalLoanPayment is null) return;

        if (LoanRepayment.RecieveAmount < OriginalLoanPayment.SubTotal)
        {
            LoanRepayment.RecieveAmount = 0;
            LoanRepayment.ChangeAmount = 0;
        }
    }

    void ReceivePaymentChanged(ChangeEventArgs e)
    {
        if (LoanRepayment is null || OriginalLoanPayment is null) return;

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
        if (LoanRepayment is null || OriginalLoanPayment is null) return;

        var amount = e.Value?.ToNumer<decimal>();

        if (amount < 0)
        {
            LoanRepayment.PenaltyAmount = OriginalLoanPayment.PenaltyAmount;
            return;
        }

        if (amount > OriginalLoanPayment.PenaltyAmount)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Amount must be less than penalty amount.");
            RestoreDefault();
        }
        else
        {
            LoanRepayment.SubTotal = OriginalLoanPayment.AmountWithoutPenalty + amount;
            LoanRepayment.RecieveAmount = OriginalLoanPayment.AmountWithoutPenalty + amount;
        }
    }
}

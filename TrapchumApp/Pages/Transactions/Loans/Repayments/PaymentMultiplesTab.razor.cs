using Applications.Commons.Mappers;
using Applications.Responses.ViewModels.Loans.Schedules;
using Radzen.Blazor;
using Applications.Features.LoanPayments.Requests;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments;
public partial class PaymentMultiplesTab 
{
    [Parameter, EditorRequired] public IEnumerable<VSchedule> Schedules { get; set; } = [];
    [Parameter, EditorRequired] public LoanPaymentDtoCreate? LoanPayment { set; get; } = new();
    [Parameter, EditorRequired] public int? CurrencyId { get; set; } = (int)CurrenciesType.Khr;
    [Parameter] public EventCallback ClosedTab { get; set; } = default!;
    [Parameter] public EventCallback<LoanPaymentDtoCreate> CreatePayment { get; set; } = default!;

    RadzenDataGrid<VSchedule> gridPopup = default!;

    private List<VSchedule> _schedules = [];

    private IList<VSchedule> originalSchedules = [];

    string columnEditing = string.Empty;

    List<KeyValuePair<long, string>> editedFields = new List<KeyValuePair<long, string>>();

    List<VSchedule> scheduleToUpdate = [];

    IRadzenFormComponent editor = default!;

    bool editorFocused;

    protected override void OnInitialized()
    {
        CopyToTempSchedules();

        DeepCopySchedules();

        base.OnInitialized();
    }

    private void DeepCopySchedules()
    {
        // Store a deep copy of the original data
        originalSchedules = _schedules.Select(s => new VSchedule
        {
            LoanId = s.LoanId,
            InstallmentNo = s.InstallmentNo,
            PaymentDate = s.PaymentDate,
            LeftPrinciple = s.LeftPrinciple,
            LeftInterestAmount = s.LeftInterestAmount,
            LeftCollectionFeeAmount = s.LeftCollectionFeeAmount,
            PenaltyAmount = s.PenaltyAmount,
            LeftAmount = s.LeftAmount,
            LoanScheduleId = s.LoanScheduleId,
            CurrencyId = s.CurrencyId
        }).ToList();

        LoanPayment!.LoanPaymentLines = _schedules.MapsTo();
    }

    private void CopyToTempSchedules()
    {
        _schedules = Schedules.Select(s => new VSchedule
        {
            LoanId = s.LoanId,
            InstallmentNo = s.InstallmentNo,
            PaymentDate = s.PaymentDate,
            LeftPrinciple = s.LeftPrinciple,
            LeftInterestAmount = s.LeftInterestAmount,
            LeftCollectionFeeAmount = s.LeftCollectionFeeAmount,
            PenaltyAmount = s.PenaltyAmount,
            LeftAmount = s.LeftAmount,
            LoanScheduleId = s.LoanScheduleId,
            CurrencyId = s.CurrencyId
        }).ToList();
    }

    async Task OnCellClick(DataGridCellMouseEventArgs<VSchedule> args)
    {
        if (!gridPopup.IsValid || (scheduleToUpdate.Contains(args.Data) &&
            columnEditing == args.Column.Property))
        {
            return;
        }

        if (scheduleToUpdate.Any())
        {

            editedFields.Add(new(scheduleToUpdate.First().LoanScheduleId!.Value, columnEditing));

            await Update();
        }

        // This sets which column is currently being edited.
        columnEditing = args.Column.Property;

        // This sets the Item to be edited.
        await EditRow(args.Data);
    }

    async Task Update()
    {
        editorFocused = false;

        if (scheduleToUpdate.Any())
        {
            await gridPopup.UpdateRow(scheduleToUpdate.First());
        }
    }

    async Task EditRow(VSchedule schedule)
    {
        await Reset();

        scheduleToUpdate.Add(schedule);

        await gridPopup.EditRow(schedule);

    }


    async Task Reset(VSchedule? schedule = null)
    {
        editorFocused = false;

        //RestoreDefaultState();

        if (schedule != null)
        {
            scheduleToUpdate.Remove(schedule);
        }
        else
        {
            scheduleToUpdate.Clear();
        }


        // Clear edit state
        //scheduleToUpdate.Clear();

        editedFields.Clear();

        //columnEditing = string.Empty;

        // Cancel any edit mode and reload the grid
        gridPopup.CancelEditRow(null!); // Cancels all rows in edit mode

        await gridPopup.Reload();

        StateHasChanged();

    }

    async Task ResetIndividualSchedule(VSchedule schedule)
    {
        editorFocused = false;

        // Find the original schedule
        var originalSchedule = originalSchedules.FirstOrDefault(x => x.LoanScheduleId == schedule.LoanScheduleId);

        if (originalSchedule == null) return;

        // Find the schedule in _schedules to update
        var targetSchedule = _schedules.FirstOrDefault(x => x.LoanScheduleId == schedule.LoanScheduleId);

        if (targetSchedule == null) return;

        // Restore all properties from originalSchedule
        targetSchedule.LeftPrinciple = originalSchedule.LeftPrinciple;
        targetSchedule.LeftInterestAmount = originalSchedule.LeftInterestAmount;
        targetSchedule.LeftCollectionFeeAmount = originalSchedule.LeftCollectionFeeAmount;
        targetSchedule.PenaltyAmount = originalSchedule.PenaltyAmount;
        targetSchedule.LeftAmount = originalSchedule.LeftAmount;

        // Remove from editedFields for this schedule
        editedFields.RemoveAll(f => f.Key == schedule.LoanScheduleId);

        // Clear edit state
        scheduleToUpdate.Remove(schedule);

        columnEditing = string.Empty;

        // Cancel edit mode for the row

        gridPopup.CancelEditRow(schedule);

        // Refresh the grid to reflect changes
        await gridPopup.Reload();

        StateHasChanged();
    }

    private void RestoreDefaultState()
    {

        _schedules = originalSchedules.Select(s => new VSchedule
        {
            LoanId = s.LoanId,
            InstallmentNo = s.InstallmentNo,
            PaymentDate = s.PaymentDate,
            LeftPrinciple = s.LeftPrinciple,
            LeftInterestAmount = s.LeftInterestAmount,
            LeftCollectionFeeAmount = s.LeftCollectionFeeAmount,
            PenaltyAmount = s.PenaltyAmount,
            LeftAmount = s.LeftAmount,
            LoanScheduleId = s.LoanScheduleId,
            CurrencyId = s.CurrencyId
        }).ToList();

    }
    bool IsEditing(string columnName, VSchedule schedule)
    {
        // Comparing strings is quicker than checking the contents of a List, so let the property check fail first.
        return columnEditing == columnName && scheduleToUpdate.Contains(schedule);
    }

    string IsEdited(RadzenDataGridColumn<VSchedule> column, VSchedule schedule)
    {
        // In a real scenario, you might use IRevertibleChangeTracking to check the current column
        //  against a list of the object's edited fields.
        return editedFields.Where(c => c.Key == schedule.LoanScheduleId &&
                                       c.Value == column.Property).Any() ?
                                       "table-cell-edited" : string.Empty;
    }

    async Task OnUpdateRow(VSchedule schedule)
    {
        await Reset(schedule);
    }
    private async Task Close(bool isclosed = false)
    {
        await Reset();

        DialogService.CloseSide(isclosed);
    }


    private async Task CloseTab()
    {
        await Reset();

        await ClosedTab.InvokeAsync();
    }




    async Task OnPenaltyChange(decimal? value, VSchedule schedule)
    {

        await Update();

        if (value.HasValue)
        {
            schedule.PenaltyAmount = value.Value;

            UpdateLeftAmount(schedule);

            AddToEditedFields(schedule, nameof(schedule.LeftPrinciple));
        }


        LoanPayment!.LoanPaymentLines = _schedules.MapsTo();
    }

    void UpdateLeftAmount(VSchedule schedule)
    {
        // Calculate LeftAmount as the sum of LeftPrinciple, LeftInterestAmount, LeftCollectionFeeAmount, and PenaltyAmount
        //schedule.LeftAmount = (schedule.LeftPrinciple ?? 0) +
        //                     (schedule.LeftInterestAmount ?? 0) +
        //                     (schedule.LeftCollectionFeeAmount ?? 0) +
        //                     (schedule.PenaltyAmount ?? 0);

    }


    void AddToEditedFields(VSchedule schedule, string propertyName)
    {
        if (!editedFields.Any(f => f.Key == schedule.LoanScheduleId && f.Value == propertyName))
        {
            editedFields.Add(new KeyValuePair<long, string>(schedule.LoanScheduleId!.Value, propertyName));
        }

        if (!scheduleToUpdate.Contains(schedule))
        {
            scheduleToUpdate.Add(schedule);
        }
    }

    private async Task Resets()
    {
        _schedules = originalSchedules.ToList();

        DeepCopySchedules();

        await gridPopup.Reload();

        scheduleToUpdate.Clear();

        StateHasChanged();
    }

    private void ApplySchedulePayment()
    {
        if (!_schedules.Any())
        {

            NotificationService.Notify(NotificationSeverity.Error, "There is no any schedule to pay.");

            return;
        }

        LoanPayment!.LoanPaymentLines = _schedules.MapsTo();

        var create = _schedules.MapsTo();

        var options = new DialogOptions
        {
            CloseDialogOnOverlayClick = false,
            Left = "10%",
        };

        var parameters = new Dictionary<string, object?>
        {
            {"LoanPaymentCreate", LoanPayment},
            {"CurrencyId",CurrencyId }
        };

        DialogService.OpenAsync<PaymentSideBar>($"{LoanPayment.CustomerName}", parameters, options);

    }



    private async Task CreateSchedulePayment(LoanPaymentDtoCreate arg) 
    {
        await CreatePayment.InvokeAsync(arg);
    }


}

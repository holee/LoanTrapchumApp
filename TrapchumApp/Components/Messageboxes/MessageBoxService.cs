namespace TrapchumApp.Components.Messageboxes;

public sealed class MessageBoxService(DialogService dialogService)
{
    public async Task<MsgBoxResult> ShowAsync(
        string message,
        string title = "Message",
        MsgBoxButtons buttons = MsgBoxButtons.OK,
        MessageBoxIcon icon = MessageBoxIcon.None,
        MessageBoxOptions? options = null)
    {
        options ??= new();

        var parameters = new Dictionary<string, object?>
        {
            { nameof(MessageBox.Message), message },
            { nameof(MessageBox.Title), title },
            { nameof(MessageBox.Buttons), buttons },
            { nameof(MessageBox.Icon), icon },
            { nameof(MessageBox.ButtonColors), options.ButtonColors }
        };

        var dialogOptions = new DialogOptions
        {
            ShowTitle = false,
            ShowClose = false,
            Draggable = options.Draggable,
            CloseDialogOnOverlayClick = false,
            CloseDialogOnEsc = buttons != MsgBoxButtons.AbortRetryIgnore,
            Width = "auto",
            Style = "min-width:340px;max-width:520px;padding:0;border:none;background:transparent;box-shadow:none;overflow:visible;"
        };

        var result = await dialogService.OpenAsync<MessageBox>(title, parameters, dialogOptions);

        return result is MsgBoxResult mbr ? mbr : MsgBoxResult.None;
    }

    public Task<MsgBoxResult> InfoAsync(string message, string title = "Information", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MsgBoxButtons.OK, MessageBoxIcon.Information, options);

    public Task<MsgBoxResult> WarningAsync(string message, string title = "Warning", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MsgBoxButtons.OK, MessageBoxIcon.Warning, options);

    public Task<MsgBoxResult> ErrorAsync(string message, string title = "Error", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MsgBoxButtons.OK, MessageBoxIcon.Error, options);

    public Task<MsgBoxResult> SuccessAsync(string message, string title = "Success", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MsgBoxButtons.OK, MessageBoxIcon.Success, options);

    public Task<MsgBoxResult> ConfirmAsync(string message, string title = "Confirm", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MsgBoxButtons.YesNo, MessageBoxIcon.Question, options);

    public Task<MsgBoxResult> ConfirmCancelAsync(string message, string title = "Confirm", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MsgBoxButtons.YesNoCancel, MessageBoxIcon.Question, options);
}

using Radzen;

namespace TrapchumApp.Components.Messageboxes;

public sealed class MessageBoxService(DialogService dialogService)
{
    public async Task<MessageBoxResult> ShowAsync(
        string message,
        string title = "Message",
        MessageBoxButtons buttons = MessageBoxButtons.OK,
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
            CloseDialogOnEsc = buttons != MessageBoxButtons.AbortRetryIgnore,
            Width = "auto",
            Style = "min-width:340px;max-width:520px;padding:0;border:none;background:transparent;box-shadow:none;overflow:visible;"
        };

        var result = await dialogService.OpenAsync<MessageBox>(title, parameters, dialogOptions);

        return result is MessageBoxResult mbr ? mbr : MessageBoxResult.None;
    }

    public Task<MessageBoxResult> InfoAsync(string message, string title = "Information", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information, options);

    public Task<MessageBoxResult> WarningAsync(string message, string title = "Warning", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning, options);

    public Task<MessageBoxResult> ErrorAsync(string message, string title = "Error", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error, options);

    public Task<MessageBoxResult> SuccessAsync(string message, string title = "Success", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MessageBoxButtons.OK, MessageBoxIcon.Success, options);

    public Task<MessageBoxResult> ConfirmAsync(string message, string title = "Confirm", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, options);

    public Task<MessageBoxResult> ConfirmCancelAsync(string message, string title = "Confirm", MessageBoxOptions? options = null)
        => ShowAsync(message, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, options);
}

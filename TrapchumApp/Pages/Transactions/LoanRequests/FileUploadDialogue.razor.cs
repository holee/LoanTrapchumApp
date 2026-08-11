using Applications.Features.Documents.Command.DeleteDocument;
using Applications.Features.Documents.Dto;
using Radzen.Blazor;
using System.Text.Json;

namespace TrapchumApp.Pages.Transactions.LoanRequests;

public partial class FileUploadDialogue(ILogger<FileUploadDialogue> logger)
{

    // Dropzone UI state
    private bool _dz_isDragOver = false;
    private List<DocumentDto> _dz_selectedFiles = new();
    const long maxFileSize = 2L * 1024 * 1024 * 1024;
    RadzenUpload _dz_radzenUpload = default!;
    bool _dz_showProgress;
    private bool _dz_isError = false;
    private string? _dz_errorMessage = "";
    private int _progressValue = 0;
    void Dz_UploadComplete(UploadCompleteEventArgs args)
    {
        logger.LogInformation("Upload complete event triggered.");

        _dz_showProgress = false;

        if (string.IsNullOrEmpty(args.RawResponse))
        {
            return;
        }
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var fileModels = JsonSerializer.Deserialize<List<DocumentDto>>(args.RawResponse!, options);

            if (fileModels is not null)
            {
                _dz_selectedFiles = fileModels;
            }
        }
        catch (Exception ex)
        {
            _dz_errorMessage = $"Error parsing upload response: {ex.Message}";

            logger.LogError(ex, "Error parsing upload response: {Message}", ex.Message);
        }

    }

    async Task Dz_UploadProgress(UploadProgressArgs args)
    {

        _dz_showProgress = true;

        _progressValue = args.Progress;

        StateHasChanged();
    }

    void Dz_OnError(UploadErrorEventArgs args)
    {
        _dz_isError = true;

        _dz_showProgress = false;

        _dz_errorMessage = args.Message;
    }


    async Task Dz_HandleUploadFiles()
    {
        if (!_dz_selectedFiles.Any())
        {
            _toastService.ShowError("There is no any file to upload");

            Dz_CloseModal();

            return;
        }

        DialogService.Close(_dz_selectedFiles);

    }


    void Dz_CloseModal()
    {
        DialogService.Close(Enumerable.Empty<DocumentDto>());
    }

    private async Task Dz_RemoveFile(DocumentDto file)
    {
        if (!_dz_selectedFiles.Contains(file)) return;

        var command = new DeleteDocumentCommand(file.Id);

        var result = await mediator.Send(command);

        if (result)
        {

            _toastService.ShowSuccess($"File removed: {file.OriginalName}");

            _dz_selectedFiles.Remove(file);

            StateHasChanged();

        }

        await Task.CompletedTask;
    }

    // Helper to format file sizes for display
    private string Dz_FormatSize(long? size)
    {
        if (size == null) return "0 B";

        if (size < 1024) return $"{size} B";

        if (size < 1024 * 1024) return $"{size / 1024} KB";

        return $"{(size / (1024.0 * 1024.0)):F2} MB";
    }

    // Drag handlers used by the dropzone markup
    private void Dz_OnDragEnter(DragEventArgs e) => _dz_isDragOver = true;
    private void Dz_OnDragOver(DragEventArgs e) => _dz_isDragOver = true;
    private void Dz_OnDragLeave(DragEventArgs e) => _dz_isDragOver = false;
    private Task Dz_OnDrop(DragEventArgs e)
    {
        _dz_isDragOver = false;

        return Task.CompletedTask;
    }


}

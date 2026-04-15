using Applications.Services.Files;
using Domains.Models.Files;
using Radzen;
using Radzen.Blazor;
using System.Text.Json;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class FileUploadDialogue
    {

        // Dropzone UI state
        private bool _dz_isDragOver = false;

        private readonly IFileUploadServiceImp _fileUploadService;

        private List<FileModel> _dz_files = new List<FileModel>();

        private List<FileModel> _dz_selectedFiles = new();

        public FileUploadDialogue(IFileUploadServiceImp fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        IEnumerable<Radzen.FileInfo> UploadFiles = [];

        const long maxFileSize = 2L * 1024 * 1024 * 1024;

        RadzenUpload _dz_radzenUpload = default!;

        bool _dz_showProgress;

        private bool _dz_isError = false;

        private string? _dz_errorMessage = "";

        void Dz_UploadComplete(UploadCompleteEventArgs args)
        {

            _dz_showProgress = false;

            if (string.IsNullOrEmpty(args.RawResponse))
            {
                return;
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var fileModels = JsonSerializer.Deserialize<List<FileModel>>(args.RawResponse!, options);

            if (fileModels is not null)
            {
                _dz_selectedFiles = fileModels;
            }
            else
            {

            }
        }

        async Task Dz_UploadProgress(UploadProgressArgs args)
        {

            _dz_showProgress = true;

            if (args.Progress == 100)
            {
                _dz_showProgress = false;
            }
        }

        void Dz_OnError(UploadErrorEventArgs args)
        {
            _dz_isError = true;

            _dz_showProgress = false;

            _dz_errorMessage = args.Message;
        }



        async Task Dz_HandleUploadFiles()
        {
            if (!_dz_files.Any())
            {
                NotificationService.Notify(NotificationSeverity.Error, "There is no any file to upload");

                Dz_CloseModal();

                return;
            }

            var resp = await _fileUploadService.UploadFiles(_dz_files);

            if (!resp.IsSuccess)
            {

                NotificationService.Notify(NotificationSeverity.Info, resp.ErrMessage);

                DialogService.Close(Enumerable.Empty<FileModel>());

                return;
            }

            _dz_files = [];

            DialogService.Close(resp.Data);
        }


        void Dz_CloseModal()
        {
            DialogService.Close(Enumerable.Empty<FileModel>());
        }

        private async Task Dz_OnFileUpload(UploadChangeEventArgs args)
        {

            if (args.Files is null || !args.Files.Any()) return;

            try
            {
                foreach (var file in args.Files)
                {
                    var ext = Path.GetExtension(file.Name);

                    var originalName = file.Name;

                    if (file is null)
                    {
                        continue;
                    }
                    using var stream = new MemoryStream();

                    using (var fileStream = file.OpenReadStream(maxFileSize)) // 30 MB limit
                    {
                        await fileStream.CopyToAsync(stream); // ✅ Read the file content

                        _dz_files.Add(new FileModel(file.Name, originalName, file.Size, stream.ToArray()));
                    }

                }

                _dz_selectedFiles = _dz_files;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading files: {ex.Message}");
                // Handle error (e.g., show notification)
            }
        }

        // Remove a selected file from the lists
        private Task Dz_RemoveFile(FileModel file)
        {
            if (_dz_selectedFiles.Contains(file))
                _dz_selectedFiles.Remove(file);

            if (_dz_files.Contains(file))
                _dz_files.Remove(file);

            StateHasChanged();
            return Task.CompletedTask;
        }

        // Helper to format file sizes for display
        private string Dz_FormatSize(long size)
        {
            if (size < 1024) return $"{size} B";
            if (size < 1024 * 1024) return $"{size / 1024} KB";
            return $"{(size / (1024.0 * 1024.0)):F2} MB";
        }

        // Drag handlers used by the dropzone markup
        private void Dz_OnDragEnter(Microsoft.AspNetCore.Components.Web.DragEventArgs e) => _dz_isDragOver = true;
        private void Dz_OnDragOver(Microsoft.AspNetCore.Components.Web.DragEventArgs e) => _dz_isDragOver = true;
        private void Dz_OnDragLeave(Microsoft.AspNetCore.Components.Web.DragEventArgs e) => _dz_isDragOver = false;
        private Task Dz_OnDrop(Microsoft.AspNetCore.Components.Web.DragEventArgs e)
        {
            _dz_isDragOver = false;
            return Task.CompletedTask;
        }


    }
}

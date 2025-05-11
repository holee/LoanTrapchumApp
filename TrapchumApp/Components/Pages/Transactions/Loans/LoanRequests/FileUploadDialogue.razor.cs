using Applications.Services.Files;
using Domains.Models.Files;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class FileUploadDialogue
    {

        private readonly IFileUploadServiceImp _fileUploadService;

        List<FileModel> files = new List<FileModel>();

        public FileUploadDialogue(IFileUploadServiceImp fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        IEnumerable<Radzen.FileInfo> UploadFiles = [];
        const long maxFileSize = 2L * 1024 * 1024 * 1024;
        RadzenUpload radzenUpload = default!;
        void OnProgress(UploadProgressArgs args, string name)
        {
            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {

                }
            }
        }
        void SelectedsFilesChanged(UploadChangeEventArgs args)
        {
            UploadFiles = [];

            if (args.Files.Any())
            {
                UploadFiles = args.Files.Select(x => x).ToList();
            }
            StateHasChanged();
        }
        async Task HandleUploadFiles()
        {

            if (files.Any())
            {

                var resp = await _fileUploadService.UploadFiles(files);

                if (!resp.IsSuccess)
                {

                    NotificationService.Notify(NotificationSeverity.Info, resp.ErrMessage);

                    return;
                }

                files = [];

                DialogService.Close(resp.Data);

            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Info, "No file selected");
            }

        }

        private async Task OnFileUpload(UploadChangeEventArgs args)
        {
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

                        files.Add(new FileModel(file.Name, originalName, stream.ToArray()));
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading files: {ex.Message}");
                // Handle error (e.g., show notification)
            }
        }


    }
}

using Applications.Services.Files;
using Domains.Models.Files;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
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

        async Task HandleUploadFiles()
        {

            if (!files.Any())
            {
                NotificationService.Notify(NotificationSeverity.Error, "There is no any file to upload");

                CloseModal();

                return;
            }

            var resp = await _fileUploadService.UploadFiles(files);

            if (!resp.IsSuccess)
            {

                NotificationService.Notify(NotificationSeverity.Info, resp.ErrMessage);

                DialogService.Close(Enumerable.Empty<FileModel>());

                return;
            }

            files = [];

            DialogService.Close(resp.Data);
        }


        void CloseModal()
        {
            DialogService.Close(Enumerable.Empty<FileModel>());
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

                        files.Add(new FileModel(file.Name, originalName, file.Size, stream.ToArray()));
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

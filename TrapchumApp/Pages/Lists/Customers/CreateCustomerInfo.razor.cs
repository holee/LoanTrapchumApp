using Applications.Extensions;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Services.Files;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Files;
using Domains.Models.Lists;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CreateCustomerInfo
    {

        const long maxFileSize = 30_000_000;
        private readonly IFileUploadServiceImp _fileUploadService;
        private readonly INationalityServiceImpl _nationalityService;
        private readonly IOccupationServiceImpl _occupationService;
        private readonly ISalutationServiceImpl _salutationService;
        private string? tempUrl = string.Empty;
        public CreateCustomerInfo(IFileUploadServiceImp fileUploadService, INationalityServiceImpl ationalityService, IOccupationServiceImpl occupationService, ISalutationServiceImpl salutationService)
        {
            _fileUploadService = fileUploadService;
            _nationalityService = ationalityService;
            _occupationService = occupationService;
            _salutationService = salutationService;
        }
        [Parameter] public EntityCustomerCreate Model { get; set; } = new();


        protected override async Task OnInitializedAsync()
        {
            await GetAdditionalInformation();

            if (Model.Photo.IsNotNullOrEmpty())
            {
                tempUrl = Model.Photo;
            }

            await base.OnInitializedAsync();
        }












        void OnProgress(UploadProgressArgs args, string name)
        {
            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {

                }
            }
        }
        private async Task OnFileUpload(UploadChangeEventArgs args)
        {
            try
            {
                if (!args.Files.Any())
                {
                    if (!string.IsNullOrEmpty(tempUrl))
                    {
                        Model.Photo = tempUrl;
                    }
                    else
                    {
                        Model.Photo = string.Empty;
                    }
                    return;
                }

                var file = args.Files.FirstOrDefault();

                if (file is null || (file is not null && file.Size > maxFileSize))
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "File upload error",
                        Detail = "File is too large or not selected."
                    });
                    return;
                }
                var ext = Path.GetExtension(file!.Name);

                var originalName = file.Name;

                using var stream = new MemoryStream();

                using var fileStream = file.OpenReadStream(maxFileSize);// 30 MB limit

                await fileStream.CopyToAsync(stream); // ✅ Read the file content

                var fileModel = new FileModel(originalName, file.Name, file.Size, stream.ToArray());

                var rsp = await _fileUploadService.UploadFile(fileModel);

                if (rsp.IsSuccess)
                {
                    Model.Photo = rsp.Data?.FileUrl ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading files: {ex.Message}");
                // Handle error (e.g., show notification)
            }
        }








        IEnumerable<OcupationResponse> Occupations = [];
        IEnumerable<Nationality> Nationalities { get; set; } = [];

        IEnumerable<string> Salutations = [];

        async Task GetAdditionalInformation()
        {
            Nationalities = await _nationalityService.GetNationalities();

            Occupations = await _employeeService!.GetOccupationsAsync("Position");

            await GetSalutations();
        }

        private async Task GetSalutations()
        {
            var salutations = await _salutationService.GetSalutions();

            if (salutations.IsSuccess)
            {
                Salutations = salutations!.Data!.Select(x => x.Title!).ToList();
            }
            else
            {
                Salutations = [];
            }
        }
    }
}



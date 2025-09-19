using Applications.Extensions;
using Applications.Responses.Lists.Entities;
using Applications.Services.Files;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Files;
using Domains.Models.Lists;
using Domains.Shared.Enums;
using System.Threading.Tasks;
using Applications.Features.OtherNames;
using Applications.Features.Settings.Requests;

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
        IEnumerable<OcupationResponse> OriginalOccupations = [];
        IEnumerable<Nationality> Nationalities { get; set; } = [];

        IEnumerable<string> Salutations = [];
        IEnumerable<string> OriginalSalutations = [];
        async Task GetAdditionalInformation()
        {
            Nationalities = await _nationalityService.GetNationalities();
            await GetSalutations();
            await GetOccupations();
        }

        private async Task GetSalutations(string? filter = null)
        {
            var rsp = await _salutationService.GetSalutions(filter);

            Salutations = rsp!.Data!.Select(x => x.Title!) ?? [];

            OriginalSalutations = Salutations.Select(x => x) ?? [];

        }

        async Task AddOccupation()
        {
            if (!string.IsNullOrEmpty(Model.Occupation))
            {
                var occupation = new OccupationDtoCreate
                {
                    OccupationTitle = Model.Occupation,
                    Khmer = string.Empty,
                    TypeOf = TypeOfEnum.Position.ToString()
                };

                var rsp = await _occupationService.CreateAsync(occupation);

                if (!rsp.IsSuccess)
                {
                    Model.Occupation = string.Empty;
                }
                else
                {
                    await GetOccupations();
                }
            }
        }

        private async Task GetOccupations(string? filter = "Job")
        {
            Occupations = await _employeeService.GetOccupationsAsync(filter);
        }

        string occupationFilter = string.Empty;

        string _salFilter = string.Empty;
        private void OnSalutaionSearched(string value)
        {
            _salFilter = value;

            if (!string.IsNullOrEmpty(value))
            {
                Salutations = Salutations.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                Salutations = OriginalSalutations;
            }

            StateHasChanged();
        }

        private void OnOccupationSearched(string value)
        {
            //occupationFilter = value;

            if (!string.IsNullOrEmpty(value))
            {
                Occupations = OriginalOccupations.Where(x => x.OccupationTitle!.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                                                 .ToList();
            }
            else
            {
                Occupations = OriginalOccupations.ToList();
            }

            StateHasChanged();

        }

        async Task LoadOccupations(LoadDataArgs args)
        {

            occupationFilter = args.Filter?.ToString() ?? string.Empty;

            await GetOccupations(occupationFilter);
        }


    }
}




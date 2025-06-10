using Applications.Extensions;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Services.Files;
using Applications.Services.Lists;
using Domains.Models.Files;
using Domains.Models.Lists;

namespace TrapchumApp.Pages.Lists.Employees
{
    public partial class EmployeeComp
    {
        [Parameter, EditorRequired] public EntityEmployeeCreate model { get; set; } = new(); 
        [Parameter] public EventCallback<bool> OnClose { get; set; }

        private readonly ISalutationServiceImpl _salutationService;
        private readonly INationalityServiceImpl _nationalityService;
        private readonly IDepartmentServiceImpl _departmentService;
        public EmployeeComp(ISalutationServiceImpl salutationService, INationalityServiceImpl nationalityService, IDepartmentServiceImpl departmentService, IFileUploadServiceImp fileUploadService)
        {
            _salutationService = salutationService;
            _nationalityService = nationalityService;
            _departmentService = departmentService;
            _fileUploadService = fileUploadService;
        }
        long? fileSize;
        IEnumerable<int> relations = new List<int> { 1, 2, 3, 4 };
        IEnumerable<string> supervisors = new List<string> { "Supervisor", "Manager", "CEO" };
        IEnumerable<string> genders = new List<string> { "Male", "Female", "Other" };
        IEnumerable<string> maritalStatuses = new List<string> { "Single", "Married", "Divorced", "Widowed" };
        IEnumerable<VillageDto> Villages = [];
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<BranchDto> Branches = [];
        IEnumerable<DepartmentDto> Departments = [];
        IEnumerable<Nationality> Nationalities = [];
        IEnumerable<OcupationResponse> Occuptions = [];
        IEnumerable<string> Cities = [];
        protected FileModel? fileModelDTO;
        protected EditForm? editForm;
        protected string? strText = "";
        protected bool IsActive = false;
        private string tempPath = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            await GetAdditionalInformation();

            if (model.Photo.IsNotNullOrEmpty())
            {
                tempPath = model.Photo!;
            }

            await base.OnInitializedAsync();
        }


        async Task GetAdditionalInformation()
        {
            var results = await _branchService.GetBranchesAsync<BranchDto>(0, new QueryParam(0, 25, string.Empty));

            if (results.IsSuccess)
            {
                Branches = results!.Data!.Items;
            }
            var deptResponse = await _departmentService.GetDepartments<DepartmentDto>(0, 0);

            if (deptResponse.IsSuccess)
            {
                Departments = deptResponse.Data!;
            }

            Nationalities = await _nationalityService!.GetNationalities();

            Occuptions = await _employeeService!.GetOccupationsAsync("Position");

            await GetSalutations();

            StateHasChanged();
        }

        IEnumerable<string> Salutations { get; set; } = [];

        protected string? newSalution = "";

        protected async Task AddNewSalutation(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                await _salutationService.AddSalution(new Salutation
                {
                    Title = newSalution
                });
                await GetSalutations();
            }
        }

        private async Task GetSalutations()
        {
            var rsp = await _salutationService.GetsSalutions();

            if (rsp.IsSuccess)
            {
                Salutations = rsp!.Data ?? [];
            }
            else
            {
                Salutations = [];
            }

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

        private readonly IFileUploadServiceImp _fileUploadService;
        const long maxFileSize = 2L * 1024 * 1024 * 1024;
        private async Task OnFileUpload(UploadChangeEventArgs args)
        {
            try
            {
                if (!args.Files.Any())
                {
                    if (tempPath.IsNotNullOrEmpty())
                    {
                        model.Photo = tempPath;
                    }
                    else
                    {
                        model.Photo = string.Empty;
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
                    model.Photo = rsp.Data?.FileUrl ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading files: {ex.Message}");
                // Handle error (e.g., show notification)
            }
        }

        void SetFirstNameToName()
        {
            model.Name = model.FirstName;
        }
    }
}
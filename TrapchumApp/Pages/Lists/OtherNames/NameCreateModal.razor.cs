using Applications.Features.OtherNames.Dto;
using Applications.Services.Lists;
using Domains.Models.Bases;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class NameCreateModal(IOtherNameServiceImpl otherNameService)
    {
        //[Parameter] public IEnumerable<string> Salutations { get; set; } = [];

        private EntityNamesCreate model = new EntityNamesCreate();

        private bool isValid = true;

        protected List<string> salutations = ["Mr.", "Mrs.", "Miss.", "Others"];

        protected override async Task OnInitializedAsync()
        {
            model.Gender = "Mr";

            await base.OnInitializedAsync();
        }
        protected async Task OnSaveChanges()
        {
            model.Id = $"{Guid.CreateVersion7()}";

            model.EntType = EntType.OtherName.ToString();

            model.CountryName = "Cambodia";

            var rsp = await otherNameService!.CreateAsync(model);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data!);

                model = new();

                return;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }

        }
        protected string? newSalution = "";
        //protected async Task AddNewSalutation(KeyboardEventArgs e)
        //{
        //    if (e.Code == "Enter")
        //    {
        //        await salutationService.AddSalution(new Salutation
        //        {
        //            Title = newSalution
        //        });
        //        await GetSalutations();
        //    }
        //}
        //protected void OnTextChangedSalution(ChangeEventArgs e)
        //{
        //    newSalution = e?.Value?.ToString();
        //}
        //private async Task GetSalutations()
        //{
        //    var salutations = await salutationService.GetSalutions();

        //    if (salutations.IsSuccess)
        //    {
        //        Salutations = salutations!.Data!.Select(x => x.Title!).ToList();
        //    }
        //    else
        //    {
        //        Salutations = [];
        //    }

        //}
    }
}
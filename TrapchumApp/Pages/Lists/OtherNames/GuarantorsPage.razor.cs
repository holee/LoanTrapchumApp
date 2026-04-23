using Applications.Features.Guarantor.Dtos;
using Applications.Features.Guarantor.Queries.GetGuarantorsAsPaging;
using Applications.Features.OtherNames.Dto;
using Applications.Responses.Paginations;
using Applications.Services.Lists;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class GuarantorsPage
    {
        //private readonly IGuarantorServiceImpl _gService;
        //public GuarantorsPage(IGuarantorServiceImpl gService)
        //{
        //    _gService = gService;
        //}

        //bool includeInactive = true;

        //string? filterText = "";

        //int pageIndex = 0;

        //int pageSize = 10;

        //string sortColumn = "FullName";

        //bool sortDescending = false;

        //int counts = 0;

        //bool isLoading = false;

        //bool ShowAddTab = false;

        //bool ShowDetailTab = false;

        //bool ShowEditTab = false;

        RadzenTabs tabs = default!;

        int selectedTabIndex = 0;

        //string? SelectedGuarantorId = string.Empty;

        //IEnumerable<VGuarantor> guarantors = [];

        //EntityGuarantorCreate modelCreate = new EntityGuarantorCreate();

        //EntityNamesCreate modelEdit = new EntityNamesCreate();

    }
}
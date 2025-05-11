using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Applications.Requests.Lists.Currency;
using Applications.Responses.Lists.Currency;
using Applications.Responses.Paginations;
using Domains.Models.Currencies;
using Mapster;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Currencies
{
    public partial class CurrencyDetials
    {
        [Inject] ICurrencyService CurrencyServiceImpl { get; set; } = default!;
        [Inject] ISpotRateService spotRateService { get; set; } = default!;
        private CurrencyResponse Currency { get; set; } = new();

        private CurrencyDtoSpotRateCreate SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
        private List<CurrencyDtoSpotRate> currencySpotRates { get; set; } = default!;
        private PaginatedList<CurrencyDtoSpotRate> CurrencyPaginatedList { get; set; } = default!;
        private int pageIndex = 1;
        private DateTime? reqDate = DateTime.UtcNow;
        [Parameter] public int Id { get; set; }
        bool isActive = false;
        private CurrencyDtoSpotRate selectedSpotRate = new CurrencyDtoSpotRate();


        Task DisplayCurrencyDialogue()
        {
            return Task.CompletedTask;
            // return ModalService.Show<NoteListModal>(param =>
            // {
            //     param.Add(x => x.Id, Id);
            // }, new ModalInstanceOptions
            //     {
            //         UseModalStructure = false,
            //         FocusTrap = true,
            //         Size = ModalSize.ExtraLarge
            //     });
        }

        Task DisplayCurrencyNoteDialogue()
        {
            return Task.CompletedTask;
            // return ModalService.Show<NoteModalCreate>(param =>
            // {
            // }, new ModalInstanceOptions
            //     {
            //         UseModalStructure = false,
            //         FocusTrap = true,
            //         Size = ModalSize.Large
            //     });
        }


        void Close()
        {
            NavigationManager.NavigateTo("/Account/Currencies");
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                SpotRateDtoCreate.TransactionDate = DateTime.Now;
                var curResult = await CurrencyServiceImpl.GetCurrencyById(Id);
                if (curResult.IsSuccess)
                {
                    Currency = curResult.Data.Adapt<CurrencyResponse>();
                }
                await ChangePageIndex(pageIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            await base.OnInitializedAsync();
        }

        private void SelectedRow(CurrencyDtoSpotRate param)
        {
            selectedSpotRate = param;
            if (param is not null)
            {
                reqDate = param.TransactionDate;
                SpotRateDtoCreate.SpotRate = param.SpotRate;
                SpotRateDtoCreate.BuyingRate = param.BuyingRate;
                SpotRateDtoCreate.SellingRate = param.SellingRate;
                SpotRateDtoCreate.TransactionDate = reqDate;
                SpotRateDtoCreate.EditSeq = param.EditSeq;
            }
            else
            {
                SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
                reqDate = DateTime.Now;
            }
            StateHasChanged();
        }

        void SelectedValueChanged()
        {
            //var result = await spotRateService.GetSpotRate(Id, reqDate!.Value);

            //if (result.IsSuccess)
            //{
            //    SpotRateDtoCreate.SpotRate = result.Data!.SpotRate;
            //    SpotRateDtoCreate.BuyingRate = result.Data.BuyingRate;
            //    SpotRateDtoCreate.SellingRate = result.Data.SellingRate;
            //    SpotRateDtoCreate.TransactionDate = result.Data.TransactionDate;
            //    SpotRateDtoCreate.EditSeq = result.Data.EditSeq;
            //}
            //else
            //{
            //    SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
            //    SpotRateDtoCreate.TransactionDate = reqDate;
            //}

            //StateHasChanged();
        }

        async Task SelectPageChanged(int page)
        {
            try
            {
                pageIndex = page;
                await ChangePageIndex(page);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            StateHasChanged();
        }

        async Task FormSubmit()
        {
            try
            {

                var req = SpotRateDtoCreate.Adapt<CurrencySpotRate>();

                var message = await spotRateService.SetSpotRate(Id, req);

                if (message.IsSuccess)
                {

                    try
                    {
                        await ChangePageIndex(pageIndex);
                        SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //StateHasChanged();
        }


        private async Task ChangePageIndex(int pageIndex, int pageSize = 10)
        {
            var qparam = new QueryParam
            {
                PageNumber = pageIndex,
                PageSize = pageSize
            };
            CurrencyPaginatedList = await spotRateService.GetSpotRates(Id, qparam);
            currencySpotRates = CurrencyPaginatedList.Items;
        }




    }
}
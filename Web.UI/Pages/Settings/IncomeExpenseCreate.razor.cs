using Applications.Commons.Mappers.Incomes;
using Applications.Commons.Messages.Incomes;
using Applications.Enumerations;
using Applications.Features.Incomes.Dto;
using Domains.Enumerations;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Services;

namespace Web.UI.Pages.Settings
{
    public partial class IncomeExpenseCreate
    {
        [Inject] private ISettingApi SettingApi { get; set; } = default!;

        [Parameter] public int? Id { get; set; } = 0;

        private CreateIncomeExpenseDto _model = new CreateIncomeExpenseDto();

        private string[] _types = default(IncomeExpenseTypes).GetNamesExcluding(IncomeExpenseTypes.All);

        private bool _isEdit = false;

        protected override async Task OnInitializedAsync()
        {
            if (Id is > 0)
            {
                _isEdit = true;

                try
                {
                    var dto = await SettingApi.GetIncomeExpenseByIdAsync(Id.Value);

                    if (dto is not null)
                    {
                        _model = dto.ToIncomeExpenseEdit();
                    }
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
                }
            }

            await base.OnInitializedAsync();
        }

        private async Task OnSubmit()
        {
            await CreateOrUpdate(_isEdit);
        }

        private async Task CreateOrUpdate(bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    await SettingApi.UpdateIncomeExpenseAsync(_model.Id, _model);

                    NotificationService.Notify(NotificationSeverity.Success, "Success", IncomeExpenseMessages.Updated);
                }
                else
                {
                    await SettingApi.CreateIncomeExpenseAsync(_model);

                    NotificationService.Notify(NotificationSeverity.Success, "Success", IncomeExpenseMessages.Created);
                }

                DialogService.Close(true);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", ex.Message);
            }
        }

        private void OnCancel() => DialogService.Close(false);
    }
}

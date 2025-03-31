//using Applications.Extensions.DateTimes;
//using Applications.Extensions.Maths;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Authorization;
//using Radzen;
//using Radzen.Blazor;

//namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements
//{
//    public partial class LoanCreateTab
//    {
//        RadzenDropDownDataGrid<long?> DropDownCustomer = default!;
//        bool collapse = false;
//        string collapseIcon = "expand_more";
//        bool isVible = false;
//        int selectedTabIndex = 0;
//        bool isProgressing = false;
//        bool isCollateralTab = false;
//        bool isPreviewScheduleTab = false;
//        RadzenTabs tb = default!;
//        List<int> tabCounts = [0];
//        RadzenDropDownDataGrid<IEnumerable<VGuarantor>?> grid = default!;
//        RadzenDropDownDataGrid<IEnumerable<CollateralLineDtoCreate>?> gridCols = default!;
//        IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];
//        [CascadingParameter]
//        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
//        protected GuarantorDtoCreate guarantorDtoCreate = new GuarantorDtoCreate();
//        protected LoanDtoCreate model = new LoanDtoCreate();
//        protected IEnumerable<VGuarantor>? SelectedGuarantors = [];
//        protected IEnumerable<CollateralLineDtoCreate>? SelectedCollateralLines = [];
//        protected List<CollateralDto>? Collaterals { get; set; } = new();
//        protected DateTime? RegisterDate = DateTime.Now;
//        protected int SelectedCollectionFee = 1;
//        protected decimal? TotalAdminFee = 0;
//        protected int SelectedPeriodName = 1;
//        protected IEnumerable<VBranch> Branches = Enumerable.Empty<VBranch>();
//        int installId = 1;
//        string branchName = "";
//        string customerName = "";

//        async void SaveThenClose()
//        {
//            CloseTab();
//            await gridCols.Reload();
//        }
//        void SelectedTabChanged(int index)
//        {
//            selectedTabIndex = index;
//        }
//        void CollapseButton()
//        {
//            collapse = !collapse;
//            collapseIcon = collapse ? "expand_more" : "expand_less";
//        }
//        void CkeckedGuarantorsChanged(bool args)
//        {
//            SelectedGuarantors = args ? grid!.View.Cast<VGuarantor>().Select(c => c) : SelectedGuarantors = [];
//        }
//        void CkeckedCollateralsChanged(bool args)
//        {
//            SelectedCollateralLines = args ? gridCols!.View.Cast<CollateralLineDtoCreate>().Select(c => c) : SelectedCollateralLines = [];
//        }

//        void PrincipleTextChanged(decimal? principle)
//        {
//            if (principle is null || principle < 0)
//            {
//                NotificationService.Notify(NotificationSeverity.Warning, "Invalid principle input");
//                model.Principle = 0.00M;
//                return;
//            }

//            if (principle <= 50000)
//            {
//                model.CurrencyRefId = 2;
//                model.AdminFeeAmount = (model.Principle!.Value * (model.AdminFeeRate!.Value / 100)).RoundUp(2);
//            }
//            else
//            {
//                model.CurrencyRefId = 1;
//                model.AdminFeeAmount = (model.Principle!.Value * (model.AdminFeeRate!.Value / 100)).RoundUp(-2);
//            }
//        }

//        void TabCollateralTab()
//        {
//            if (selectedCustomer!.CustomerId == 0)
//            {
//                NotificationService.Notify(NotificationSeverity.Error, "Please selected customer first");
//                return;
//            }
//            if (!isCollateralTab)
//            {
//                isCollateralTab = true;
//                tabCounts.Add(2);
//            }
//            selectedTabIndex = 2;
//        }

//        void CloseTab()
//        {
//            if (selectedTabIndex == 0) return;

//            if (selectedTabIndex == 1)
//            {
//                isPreviewScheduleTab = false;

//                tabCounts.Remove(1);

//            }
//            if (selectedTabIndex == 2)
//            {
//                isCollateralTab = false;
//                tabCounts.Remove(2);
//            }
//            selectedTabIndex = tabCounts.Count - 1;
//            //tb.Reload();

//        }



//        void ShowLoanPreviews()
//        {
//            if (model.Principle == 0) return;
//            if (!isPreviewScheduleTab)
//            {
//                isPreviewScheduleTab = true;
//                tabCounts.Add(1);
//            }

//            selectedTabIndex = 1;
//            var calculator = SelectLoanType(model.RepaymentModeRefId!.Value);
//            calculator.PayEvery = model.PayEvery!.Value;
//            calculator.EffectiveRate = model.CollectionFeeRate!.Value;
//            calculator.StartDate = model.StartDate!.Value;
//            calculator.FirstPayment = model.FirstPaymentDate!.Value;
//            calculator.CollectionFeeMode = SelectedCollectionFee;
//            calculator.InstallmentTypeId = model.InstallmentTypeId!.Value;
//            calculator.CurrencyId = model.CurrencyRefId!.Value;
//            var schedule = calculator.Calculate(model.Installments!.Value, model.InterestRate!.Value, model.Principle!.Value);
//            previewSchedule = schedule.Select(x => new LoanScheduleDtoCreate
//            {
//                InstallmentNo = x.InstallmentNo,
//                PaymentDate = x.PaymentDate,
//                Principal = x.Principal,
//                Interest = x.Interest,
//                CollectionFee = x.CollectionFeeAmount,
//                LoanOutstanding = x.OutStanding,
//                TotalDay = x.TotalDay,
//                TotalPay = x.TotalPay,
//                AgentRefId = selectCreditAgent!.Id,
//                RepaymentModeRefId = model.RepaymentModeRefId,
//                ScheduleStatusRefId = 2

//            }).ToList();
//            //tb.Reload();
//            //StateHasChanged();
//        }

//        protected Task ShowGaurantorModal()
//        {
//            return Task.CompletedTask;

//        }
//        protected async Task ShowCollateralModal()
//        {
//            if (selectedCustomer!.CustomerId == 0)
//            {
//                NotificationService.Notify(NotificationSeverity.Error, "Please selected customer first");
//                return;
//            }
//            var options = new DialogOptions
//            {
//                Width = "50rem",
//                Height = "35rem",
//                Draggable = true,
//                Resizable = true
//            };
//            var parameters = new Dictionary<string, object>
//        {
//            {"SelectedCustomer",selectedCustomer! }
//        };

//            var result = await DialogService.OpenAsync<CollateralModalAdd>("Collateral", parameters, options);

//            if (bool.TryParse(result, out bool success) && success)
//            {
//                var collateral = await CollateralService.GetCollateralById(selectedCustomer!.CustomerId);

//                if (collateral is not null)
//                {
//                    CollateralLines = collateral.CollateralLines!.Select(x => new CollateralLineDtoCreate
//                    {
//                        CollateralRefId = x.Id,
//                        SubTypeName = x.SubTypeName,
//                        TypeName = x.TypeName,
//                        CollateralNo = x.CollateralNo,
//                        ReferenceNo = x.ReferenceNo
//                    });

//                    StateHasChanged();
//                }
//                ;
//            }
//        }

//        Task ShowCustomerModal()
//        {
//            return Task.CompletedTask;
//            // return ModalService.Show<CustomerCreateModal>
//            // (param =>
//            // {

//            // }, new ModalInstanceOptions
//            //     {
//            //         UseModalStructure = false,
//            //         Closing = OnModalClosing,
//            //         Size = ModalSize.Large,
//            //         Centered = true,
//            //     });
//        }
//        void ChangedDate()
//        {
//            if (installId == 1)
//            {
//                firstPayment = model.StartDate!.Value.AddMonths(1);
//            }
//            if (installId == 2)
//            {
//                firstPayment = model.StartDate!.Value.AddDays(14);
//            }
//            if (installId == 3)
//            {
//                firstPayment = model.StartDate!.Value.AddDays(7);
//            }
//            if (installId == 4)
//            {
//                firstPayment = model.StartDate!.Value.AddDays(1);
//            }

//            HolidayHelper.SetHoliday(Holidays, ref firstPayment);

//            model.FirstPaymentDate = firstPayment;

//        }

//        void SelectionChanged()
//        {
//            if (model.RepaymentModeRefId != 1)
//            {
//                model.PayEvery = 1;
//            }
//            else
//            {
//                model.PayEvery = 4;
//            }
//        }
//        void CalculateAdminFeeAmount()
//        {
//            if (model.CurrencyRefId == 1)
//            {
//                model.AdminFeeAmount = (model.Principle!.Value * (model.AdminFeeRate!.Value / 100)).RoundUp(-2);
//            }
//            else
//            {
//                model.AdminFeeAmount = (model.Principle!.Value * (model.AdminFeeRate!.Value / 100)).RoundUp(2);
//            }


//        }

//        void SelectedInstallmentChanged(object arg)
//        {
//            if (arg is not null)
//            {
//                var installType = (LoanInstallmentTypeDto)arg;
//                installId = installType.InstallmentTypeId;
//                model.PeriodName = installId switch
//                {
//                    1 => "Months",
//                    2 => "2 Weeks",
//                    3 => "Weeks",
//                    _ => "Days"
//                };
//                model.InstallmentType = installType.InstallmentTypeCode;
//                model.Period = model.Installments;
//                ChangedDate();
//            }


//        }



//        void SelectedPeriodChanged()
//        {
//            ChangedDate();
//        }
//        async Task PeriodChanged()
//        {
//            await GetHolidays(model.Installments!.Value);
//        }

//        async Task SearchGaurantors(LoadDataArgs arg)
//        {
//            var filter = arg.Filter ?? string.Empty;

//            var gResponse = await _gService!.GetGuarantors(0, false, filter);

//            if (gResponse.IsSuccess)
//            {
//                Guarantors = gResponse.Data!;
//            }
//            else
//            {
//                Guarantors = [];
//            }

//        }





//        void ChangedFirstPayment()
//        {
//            firstPayment = model.FirstPaymentDate!.Value;

//            if (firstPayment < model.StartDate || firstPayment < DateTime.Now)
//            {
//                NotificationService.Notify(NotificationSeverity.Error, "First Payment must greater than Current Date");
//                return;
//            }
//            HolidayHelper.SetHoliday(Holidays, ref firstPayment);
//            model.FirstPaymentDate = firstPayment;
//        }
//    }
//}
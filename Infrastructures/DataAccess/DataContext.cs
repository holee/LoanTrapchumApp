using Applications.Responses.ViewModels.Addresses;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Currencies;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Departments;
using Applications.Responses.ViewModels.Employees;
using Applications.Responses.ViewModels.Journals;
using Applications.Responses.ViewModels.Ledgers;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models;
using Domains.Models.Addresses;
using Domains.Models.Audits;
using Domains.Models.Bases;
using Domains.Models.Collaterals;
using Domains.Models.Currencies;
using Domains.Models.Journals;
using Domains.Models.Lists;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;
using Domains.Models.Loans;
using Domains.Models.Settings;
using Domains.Models.Transaction;
using Infrastructures.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace Infrastructures.DataAccess
{
    public sealed class DataContext : DbContext
    {
        private readonly AuditingSaveChangesInterceptor _auditInterceptor;
        public DataContext(DbContextOptions<DataContext> options, AuditingSaveChangesInterceptor auditInterceptor) : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var property in builder.Model.GetEntityTypes()
           .SelectMany(t => t.GetProperties())
           .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,3)");
            }


            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var prop = entityType.ClrType.GetProperties()
                    .Where(p => p.Name == "EditSeq" && p.PropertyType == typeof(int))
                    .FirstOrDefault();
                if (prop != null)
                {
                    builder.Entity(entityType.ClrType)
                       .Property(prop.Name)
                       .HasDefaultValue(1);
                }
            }


            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditInterceptor);
        }

        public IDbTransaction StartTransaction()
        {
            var transaction = Database.BeginTransaction();

            return transaction.GetDbTransaction();
        }

        public override int SaveChanges()
        {
            ProcessSaveChangedAsync();

            return base.SaveChanges();
        }

        private void ProcessSaveChangedAsync()
        {
            foreach (var item in ChangeTracker.Entries()
                                .Where(e => e.Entity is BaseAddress && e.State == EntityState.Added))
            {
                BaseAddress baseEntry = (BaseAddress)item.Entity;
                baseEntry.CreatedDt = DateTime.Now;
                baseEntry.ModifiedDt = DateTime.Now;
            }

            foreach (var item in ChangeTracker.Entries()
                               .Where(e => e.Entity is BaseAddress && e.State == EntityState.Modified))
            {
                var baseEntries = (BaseAddress)item.Entity;
                baseEntries.ModifiedDt = DateTime.Now;
                baseEntries.EditSeq += 1;
                item.Property(nameof(BaseAddress.CreatedDt)).IsModified = false;

            }
            //for entity inherti from base entity

            foreach (var item in ChangeTracker.Entries()
                                .Where(e => e.Entity is BaseEntity && e.State == EntityState.Added))
            {
                BaseEntity baseEntry = (BaseEntity)item.Entity;
                baseEntry.CreatedDt = DateTime.Now;
                baseEntry.ModifiedDt = DateTime.Now;
            }

            foreach (var item in ChangeTracker.Entries()
                               .Where(e => e.Entity is BaseEntity && e.State == EntityState.Modified))
            {
                var baseEntries = (BaseEntity)item.Entity;
                baseEntries.ModifiedDt = DateTime.Now;
                baseEntries.EditSeq += 1;
                item.Property(nameof(BaseEntity.CreatedDt)).IsModified = false;

            }

        }




        /// <summary>
        /// Handles the DbSet properties for the DataContext.
        /// </summary>

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{

        //    //var auditEntries = OnBeforeSaveChanges();

        //    //ProcessSaveChangedAsync();

        //    var result = await base.SaveChangesAsync(cancellationToken);

        //   // await OnAfterSaveChanges(auditEntries);

        //    return result;
        //}


        public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();
        public DbSet<Bank> Banks => Set<Bank>();
        public DbSet<Currency> Currencies => Set<Currency>();
        public DbSet<CurrencyCounter> CurrencyCounters => Set<CurrencyCounter>();
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<Collateral> Collaterals => Set<Collateral>();
        public DbSet<CollateralLink> CollateralsLinks => Set<CollateralLink>();
        public DbSet<CollateralLine> CollateralLines => Set<CollateralLine>();
        public DbSet<CollateralEvaluations> CollateralEvaluations => Set<CollateralEvaluations>();
        public DbSet<CollateralType> CollateralTypes => Set<CollateralType>();
        public DbSet<CollateralSubType> CollateralSubTypes => Set<CollateralSubType>();
        public DbSet<LoanPropertyType> LoanPropertyType => Set<LoanPropertyType>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<UserLoanSize> UserLoanSizes => Set<UserLoanSize>();
        public DbSet<Salutation> Salutations => Set<Salutation>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Vendor> Vendors => Set<Vendor>();
        public DbSet<LoanCreditAgent> LoanCreditAgents => Set<LoanCreditAgent>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<CustomerExpense> CustomerExpenses => Set<CustomerExpense>();
        public DbSet<CustomerIncome> CustomerIncomes => Set<CustomerIncome>();
        public DbSet<EntityNames> EntityNames => Set<EntityNames>();
        public DbSet<Occupation> Occupations => Set<Occupation>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<District> Districts => Set<District>();
        public DbSet<Commune> Communes => Set<Commune>();
        public DbSet<CustomerNote> CustomerNote => Set<CustomerNote>();
        public DbSet<CustomerActivity> CustomerActivites => Set<CustomerActivity>();
        public DbSet<Village> Villages => Set<Village>();
        public DbSet<Nationality> Nationalities => Set<Nationality>();
        public DbSet<Ledger> Ledgers => Set<Ledger>();
        public DbSet<LedgerType> LedgerTypes => Set<LedgerType>();
        public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
        public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();
        public DbSet<JournalType> JournalTypes => Set<JournalType>();
        public DbSet<LoanDisbursment> LoanDisbursments => Set<LoanDisbursment>();
        public DbSet<LoanPayment> LoanRepayments => Set<LoanPayment>();
        public DbSet<LoanPaymentLine> LoanRepaymentLine => Set<LoanPaymentLine>();
        public DbSet<LoanSchedule> LoanSchedules => Set<LoanSchedule>();
        public DbSet<LoanStatus> LoanStatuses => Set<LoanStatus>();
        public DbSet<LoanCategory> LoanCategories => Set<LoanCategory>();
        public DbSet<LoanProduct> LoanProducts => Set<LoanProduct>();
        public DbSet<LoanClarifyOfStaff> LoanClarifyOfStaff => Set<LoanClarifyOfStaff>();
        public DbSet<LoanRepaymentMode> LoanRepaymentModes => Set<LoanRepaymentMode>();
        public DbSet<LoanInstallmentType> LoanInstallmentTypes => Set<LoanInstallmentType>();
        public DbSet<LoanTransaction> LoanTransaction => Set<LoanTransaction>();
        public DbSet<LoanTransactionType> TransactionTypes => Set<LoanTransactionType>();
        public DbSet<LoanScheduleStatus> LoanScheduleStatuses => Set<LoanScheduleStatus>();
        public DbSet<LoanPaymentLocation> LoanPaymentLocations => Set<LoanPaymentLocation>();
        public DbSet<OpenBranch> OpenBranches => Set<OpenBranch>();
        public DbSet<CashVault> CashVaults => Set<CashVault>();
        public DbSet<CashManagement> CashManagements => Set<CashManagement>();
        public DbSet<CashManagementDetail> CashManagementDetails => Set<CashManagementDetail>();
        public DbSet<CashStatus> CashStatuses => Set<CashStatus>();
        public DbSet<ForeignExchange> ForeignExchanges => Set<ForeignExchange>();
        public DbSet<LoanTermType> LoanTerms => Set<LoanTermType>();
        public DbSet<LoanTypeStatus> LoanSubTypeStatuses => Set<LoanTypeStatus>();
        public DbSet<LoanSubTypeStatus> LoanSubTypeNoAcc => Set<LoanSubTypeStatus>();
        public DbSet<LoanGuarantorLink> Guarantors => Set<LoanGuarantorLink>();
        public DbSet<CoPawnCustomer> CoPawnCustomers => Set<CoPawnCustomer>();
        public DbSet<AppStatus> AppStatuses => Set<AppStatus>();
        public DbSet<CurrencySpotRate> SpotRates => Set<CurrencySpotRate>();
        public DbSet<ActivitiesStatus> ActivitiesStatuses => Set<ActivitiesStatus>();
        public DbSet<BankOther> BankOthers => Set<BankOther>();
        public DbSet<DayName> DayNames => Set<DayName>();
        public DbSet<Holiday> Holidays => Set<Holiday>();
        public DbSet<MonthName> MonthNames => Set<MonthName>();
        public DbSet<OtherType> OtherTypes => Set<OtherType>();
        public DbSet<MarritalStatus> MarritalStatuses => Set<MarritalStatus>();
        public DbSet<LoanDisbursment> LoanDisbursementRequests => Set<LoanDisbursment>();
        public DbSet<LoanRequest> LoanRequests => Set<LoanRequest>();
        public DbSet<LoanDocuments> LoanDocuments => Set<LoanDocuments>();
        public DbSet<LoanDescriptions> LoanDescriptions => Set<LoanDescriptions>();
        public DbSet<LoanChangedCa> LoanChangedCa => Set<LoanChangedCa>();
        public DbSet<AppSettings> AppSettings => Set<AppSettings>();
        public DbSet<VCurrenciesList> VCurrenciesList => Set<VCurrenciesList>();
        public DbSet<VCurrencyNotes> VCurrencyNotes => Set<VCurrencyNotes>();
        public DbSet<VBranchResponse> VBranchs => Set<VBranchResponse>();
        public DbSet<VDepartment> VDepartments => Set<VDepartment>();
        public DbSet<VSpotRate> VSpotRates => Set<VSpotRate>();
        public DbSet<VLoanDisbursments> VLoanDisbursements => Set<VLoanDisbursments>();
        public DbSet<VLoanRepaymentLine> VLoanRepaymentLines => Set<VLoanRepaymentLine>();
        public DbSet<VCollateral> VCollaterals => Set<VCollateral>();
        public DbSet<JournalEntryVm> VJournals => Set<JournalEntryVm>();
        public DbSet<VLedger> VLedgers => Set<VLedger>();
        public DbSet<VNameList> NameLists => Set<VNameList>();
        public DbSet<VendorVm> VVendor => Set<VendorVm>();
        public DbSet<VGuarantor> VGuarantors => Set<VGuarantor>();

        public DbSet<VOtherNames> VOtherNames => Set<VOtherNames>();
        public DbSet<VEmployeeList> EmployeeLists => Set<VEmployeeList>();
        public DbSet<VCustomerList> CustomerLists => Set<VCustomerList>();
        public DbSet<VCreditAgent> VCreditAgents => Set<VCreditAgent>();
        public DbSet<VLoanRequests> VLoanRequests => Set<VLoanRequests>();
        public DbSet<VDistricts> VDistricts => Set<VDistricts>();
        public DbSet<VCommunes> VCommunes => Set<VCommunes>();
        public DbSet<VVillages> VVillages => Set<VVillages>();
    }
}

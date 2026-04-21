using Applications;
using Applications.Commons.Authorizations;
using Applications.Validations;
using Domains.Models.Identity;
using Infrastructures;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QuestPDF.Infrastructure;
using TrapchumApp;
using TrapchumApp.Components;
using TrapchumApp.Components.Toasts;
using TrapchumApp.Hubs;
using TrapchumApp.Services;
var builder = WebApplication.CreateBuilder(args);


QuestPDF.Settings.License = LicenseType.Community;
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<NavigationService>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPolicyServices();
builder.Services.AddApplicationServices();
builder.Services.AddOtherServices();
builder.Services.AddValidationServiceInjections();
builder.Services.AddValidationServices();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddMapster();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthorizationHandler, SameUserRequestAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, SameUserApprovedAuthorizationHandler>();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
builder.Services.AddScoped<NotificationServices>();
builder.Services.AddScoped<NotificationHubService>();
builder.Services.AddToastService(options =>
{
    options.Position = ToastPosition.TopRight;
    options.ShowCloseButton = true;
    options.ShowProgressBar = true;
    options.NewestOnTop = true;
    options.CloseOnHover = true;
    options.PreventDuplicates = false;
});
builder.Services.AddSignalR();
builder.Services.AddScoped<TrapchumApp.Components.Messageboxes.MessageBoxService>();
builder.Services.AddSingleton<IAppNotificationService, AppNotificationService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{

    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    app.UseHsts();
}


//await app.SeedSystemData();
app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapControllers();

await app.MapAdditionalIdentityEndpoints();

app.MapUploadFileEndpoints();

app.MapHub<AppNotificationHub>("/hubs/notifications");


app.Run();

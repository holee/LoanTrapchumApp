using Applications;
using Applications.Validations;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Domains.Models.Identity;
using Infrastructures;
using Infrastructures.Authorizations;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QuestPDF.Infrastructure;
using TrapchumApp.Components;
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



builder.Services.AddSignalR();

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
app.MapStaticAssets();
app.UseStaticFiles();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapControllers();

await app.MapAdditionalIdentityEndpoints();

app.MapHub<AppNotificationHub>("/hubs/notifications");


app.Run();

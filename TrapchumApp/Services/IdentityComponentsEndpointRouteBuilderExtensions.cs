using Applications.Features.Loans.Services;
using Applications.Services.Loans;
using Domains.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace TrapchumApp.Services;

internal static class IdentityComponentsEndpointRouteBuilderExtensions
{
    // These endpoints are required by the Identity Razor components defined in the /Components/Account/Pages directory of this project.
    public static async Task<IEndpointConventionBuilder> MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {

        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/Account");


        accountGroup.MapPost("/Logout", async (ClaimsPrincipal user,
            [FromServices] SignInManager<ApplicationUser> signInManager,
            [FromForm] string returnUrl) =>
        {
            await signInManager.SignOutAsync();

            return TypedResults.LocalRedirect($"~/");

        });

        var manageGroup = accountGroup.MapGroup("/Manage").RequireAuthorization();

        var loggerFactory = endpoints.ServiceProvider.GetRequiredService<ILoggerFactory>();

        var downloadLogger = loggerFactory.CreateLogger("DownloadPersonalData");

        manageGroup.MapPost("/DownloadPersonalData", async (
            HttpContext context,
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] AuthenticationStateProvider authenticationStateProvider) =>
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                return Results.NotFound($"Unable to load user with ID '{userManager.GetUserId(context.User)}'.");
            }

            var userId = await userManager.GetUserIdAsync(user);
            downloadLogger.LogInformation("User with ID '{UserId}' asked for their personal data.", userId);

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();

            var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await userManager.GetLoginsAsync(user);

            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            personalData.Add("Authenticator Key", (await userManager.GetAuthenticatorKeyAsync(user))!);

            var fileBytes = JsonSerializer.SerializeToUtf8Bytes(personalData);

            context.Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");

            return TypedResults.File(fileBytes, contentType: "application/json", fileDownloadName: "PersonalData.json");


        });



        var reportGroup = endpoints.MapGroup("/Reports");

        reportGroup.MapGet("/Schedules/{loanId}/{branchId:int}/{fileName}", async ([FromServices] IPrintPdfServiceImpl printer, string loanId, int branchId) =>
        {
           
            var rsp = await printer.DownLoadScheduleAsPdf(branchId, loanId);

            if(rsp.IsSuccess == false)
            {
                return Results.Problem("Failed to generate PDF or no data returned.", statusCode: StatusCodes.Status500InternalServerError);
            }

            return TypedResults.File(rsp.Data!, contentType: "application/pdf",null, enableRangeProcessing: true);

            //, $"{Guid.CreateVersion7()}.pdf"

        });

        reportGroup.MapGet("/Schedules/Pdf/{loanId}/{branchId:int}/{fileName}", async ([FromServices] SchedulePrintService pdf,
            [FromServices] IWebHostEnvironment env, string loanId, int branchId) =>
        {

            var rsp = await pdf.DownLoadScheduleAsPdf(branchId, loanId);

            if (!rsp.IsSuccess) 
            {
                return Results.Problem("Failed to generate PDF or no data returned.", statusCode: StatusCodes.Status500InternalServerError);
            }

            return TypedResults.File(rsp.Data!, contentType: "application/pdf",null, enableRangeProcessing: true);

        });

        reportGroup.MapGet("/Schedules/Pdf/{loanId}/{branchId:int}/{scheduleId:int}", async ([FromServices] SchedulePrintService pdf,
           [FromServices] IWebHostEnvironment env, string loanId, int branchId, int scheduleId) =>
        {

            var rsp = await pdf.DownLoadScheduleAsPdf(branchId, loanId, scheduleId);

            if (!rsp.IsSuccess)
            {
                return Results.NoContent();
            }

            return TypedResults.File(rsp.Data!, contentType: "application/pdf", $"{Guid.CreateVersion7()}.pdf");

        });




        reportGroup.MapGet("/Schedules/{loanId}/{branchId:int}/Exel", async ([FromServices] IPrintPdfServiceImpl printer,
            [FromServices] IWebHostEnvironment env,
            [FromServices] IPrintExelServiceImpl exel,
            string loanId, int branchId) =>
        {

            var rsp = await exel.DownLoadScheduleAsExcel(loanId, branchId);

            if (!rsp.IsSuccess)
            {
                return Results.NoContent();
            }

            var ms = rsp.Data ?? Stream.Null;

            return Results.File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{loanId}.xlsx");

        });



        reportGroup.MapGet("/Schedules/{loanId}/{branchId:int}/Excel", async ([FromServices] IWebHostEnvironment env,
            [FromServices] IPrintExelServiceImpl exel,
            string loanId, int branchId) =>
        {

            var rsp = await exel.DownLoadScheduleAsExcel(loanId, branchId);

            if (!rsp.IsSuccess)
            {
                return Results.NoContent();
            }

            var ms = rsp.Data ?? Stream.Null;

            return Results.File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{loanId}.xlsx");

        });






        return accountGroup;
    }
}




//reportGroup.MapGet("/Schedules/{loanId}/{customerId}/{branchId:int}", async ([FromServices] ILoanScheduleServiceImpl _schedulesService,
//                                        string? loanId, string? customerId, int? branchId) =>
//{

//    var rsp = await _schedulesService.GetLoanSchedulesAsync(branchId, loanId, customerId);

//    var loanWithScedules = new LoanScheduleVm();

//    if (rsp.IsSuccess)
//    {
//        loanWithScedules = rsp.Data;
//    }

//    var doc = new SchedulePdfService(loanWithScedules!);

//    var stream = doc.GeneratePdf();

//    return TypedResults.File(stream, contentType: "application/pdf");

//});
//reportGroup.MapGet("/Schedules/Pdf/{loanId}/{customerId}/{branchId:int}", async ([FromServices] IWebHostEnvironment env,
//                                            [FromServices] ILoanScheduleServiceImpl _schedulesService,
//                                            string ? loanId, string? customerId, int? branchId) =>
//{


//        var scheduleResponse = await _schedulesService.GetLoanSchedulesAsync(branchId,loanId, customerId);


//        var loanWithSchedules = scheduleResponse.Data;

//        var html = await RazorTemplateEngine.RenderAsync("Views/Schedule.cshtml", loanWithSchedules);

//        var playwright = await Playwright.UpsertAsync();

//        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
//        {
//            Headless = true,
//        });

//        var path = Path.Combine(env.WebRootPath, "Files/Pdfs");

//        var page = await browser.NewPageAsync();

//        await page.SetContentAsync(html);

//        var filePath = Path.Combine(path, "schedule.pdf");

//        await page.PdfAsync(new PagePdfOptions
//        {
//            Format = "A4",
//            Path = filePath,
//        });

//        await page.CloseAsync();

//        var stream = new FileStream(filePath, FileMode.Open);

//        return TypedResults.File(stream, contentType: "application/pdf");

//});

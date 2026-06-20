using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Loans.Services;
using Applications.Features.Schedules;
using Applications.Features.Schedules.GetLoanScheduleBySchedule;
using Applications.Services.Loans;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace TrapchumApp;

public static class ScheduleEndpoint
{
    public static async Task<IEndpointConventionBuilder> MapScheduleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var reportGroup = endpoints.MapGroup("/Reports");

        reportGroup.MapGet("/Schedules/{loanId}/{branchId:int}/{fileName}", async ([FromServices] IPrintPdfServiceImpl printer,
            string loanId, int branchId) =>
            {


                var rsp = await printer.DownLoadScheduleAsPdf(branchId, loanId);

                if (rsp.IsError)
                {
                    return Results.Problem("Failed to generate PDF or no data returned.", statusCode: StatusCodes.Status500InternalServerError);
                }

                return TypedResults.File(rsp.Value, contentType: "application/pdf", null, enableRangeProcessing: true);

            });

        //reportGroup.MapGet("/Schedules/Pdf/{loanId}/{branchId:int}/{fileName}", async ([FromServices] SchedulePrintService pdf, IMediator mediator,
        //    [FromServices] IWebHostEnvironment env, string loanId, int branchId) =>
        //    {

        //        var command = new GetLoanByIdQuery(loanId, branchId, Applications.Enumerations.ScheduleStatus.All, true);

        //        var rsp = await mediator.Send(command);

        //        if (rsp.IsError)
        //        {
        //            return Results.Problem();
        //        }

        //        var doc = new ScheduleDocument(rsp.Value);

        //        var stream = doc.GeneratePdf();

        //        return TypedResults.File(stream, contentType: "application/pdf", null, enableRangeProcessing: true);


        //    });

        reportGroup.MapGet("/schedules/pdf/{loanId}/{branchId:int}/{scheduleId:int}", async ([FromServices] SchedulePrintService pdf,
            [FromServices] IWebHostEnvironment env, IMediator mediator, string loanId, int branchId, int scheduleId) =>
            {

                var command = new GetLoanScheduleByScheduleQuery(branchId, loanId, scheduleId);

                var rsp = await mediator.Send(command);

                if (rsp.IsError)
                {
                    return Results.NotFound();
                }

                int installmentNumber = 1;

                rsp.Value.Schedules.ForEach(x =>
                {
                    x.InstallmentNo = installmentNumber++;
                });


                var doc = new ScheduleByScheduleDocument(rsp.Value);

                var stream = doc.GeneratePdf();

                return TypedResults.File(stream, contentType: "application/pdf", null, enableRangeProcessing: true);

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


        return reportGroup;
    }
}
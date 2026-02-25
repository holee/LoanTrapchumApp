using Applications.Features.Loans.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;
using ClosedXML.Excel;
namespace TrapchumApp.Services;

public sealed class ExcelExportService(IWebHostEnvironment env)
{

    public MemoryStream? ScheduleExportAll(VLoanDisbursments? LoanSchedule,List<VSchedule>? schedule,string? sheetName = "Loan Schedule")
    {
        if (schedule == null) return null;

        var logoPath = Path.Combine(env.WebRootPath, "");

        try
        {
            using var workbook = new XLWorkbook();

            var sheet = workbook.Worksheets.Add(sheetName!);

            sheet.PageSetup.PageOrientation = XLPageOrientation.Portrait;

            sheet.PageSetup.PaperSize = XLPaperSize.A4Paper;
            // Add Top headers 
            var titleRange = sheet.Range("B2:J2").Merge();
            titleRange.Value = "ទ្រព្យជុំ ឃែស ផន សប";
            titleRange.Style.Font.FontSize = 16;
            titleRange.Style.Font.FontName = "Khmer OS Muol Light";
            var titleRangeEn = sheet.Range("B3:J3").Merge();
            titleRangeEn.Value = "TRAP CHUM CASH PAWN SHOP";
            titleRangeEn.Style.Font.FontSize = 16;
            titleRangeEn.Style.Font.FontName = "Khmer OS Battambang";
            var contentTitle = sheet.Range("B4:J4").Merge();
            contentTitle.Value = "តារាងកាលវិភាគសងប្រាក់";
            contentTitle.Style.Font.FontName = "Khmer OS Muol Light";
            contentTitle.Style.Font.FontSize = 14;
            contentTitle.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            var centered = sheet.Range("B2:J4");
            centered.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            centered.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            //Information Section 1
            sheet.Range("B6:C6").Merge().Value = "ឈ្មោះអតិថិជន :";
            sheet.Range("D6").Merge().Value = LoanSchedule?.AltName;
            sheet.Range("D6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Range("B7:C7").Merge().Value = "កិច្ចសន្យាលេខ :";
            sheet.Range("D7:E7").Merge().Value = LoanSchedule?.ContractNumber;
            sheet.Range("B8:C8").Merge().Value = "គណនីលេខ :";
            sheet.Range("D8:E8").Merge().Value = LoanSchedule?.LoanAccount;
            sheet.Range("B9:C9").Merge().Value = "ថ្ងៃខ្ចីប្រាក់ :";
            sheet.Range("D9:E9").Merge().Value = LoanSchedule?.RegisterDate?.ToString("dd-MM-yyyy");
            sheet.Range("B10:C10").Merge().Value = "រយៈពេលខ្ចីប្រាក់ :";
            sheet.Range("D10:E10").Merge().Value = LoanSchedule?.PeriodName;
            var infoSection1 = sheet.Range("B6:C10");
            infoSection1.Style.Font.FontName = "Khmer OS Battambang";
            infoSection1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            infoSection1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Range("D6:D10").Style.Font.Bold = true;
            sheet.Range("D6:D10").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            // Information Section 2 - Table Headers
            sheet.Cell("F6").Value = "អាសយដ្ឋានអតិថិជន :";
            sheet.Range("G6:J6").Merge().Value = LoanSchedule?.Address;
            sheet.Range("G6:J6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Cell("F7").Value = "ចំនួនទឹកប្រាក់ខ្ចី :";
            sheet.Cell("G7").Value = LoanSchedule?.Principle;
            var infoSectionCurrency = sheet.Cell("G7");
            if (LoanSchedule?.CurrencyId == 2)
            {
                infoSectionCurrency.Style.NumberFormat.Format = "#,##0.00 $";
                infoSectionCurrency.Style.Font.FontName = "Times New Roman";
            }

            else
            {
                infoSectionCurrency.Style.Font.FontName = "Khmer OS Battambang";
                infoSectionCurrency.Style.NumberFormat.Format = "#,##0 ៛";

            }

            sheet.Cell("F8").Value = "ថ្ងៃផុតកំណត់ :";
            sheet.Cell("G8").Value = LoanSchedule?.LastPaymentDate?.ToString("dd-MM-yyyy");
            sheet.Cell("F9").Value = "ភ្នាក់ងារ :";
            sheet.Cell("G9").Value = LoanSchedule?.AgentName ?? LoanSchedule?.CreditAgentKhmer;
            sheet.Cell("F10").Value = "ការិយាល័យ :";
            sheet.Cell("G10").Value = LoanSchedule?.BranchName;
            var infoSection2 = sheet.Range("F6:F10");
            infoSection2.Style.Font.FontName = "Khmer OS Battambang";
            infoSection2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            infoSection2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Range("G6:G10").Style.Font.Bold = true;
            sheet.Range("G6:G10").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            // Information Section 3 
            sheet.Cell("H7").Value = "ទូរស័ព្ទអតិថិជន :";
            sheet.Cell("I7").Value = LoanSchedule?.Phone;
            sheet.Cell("H8").Value = "ទូរស័ព្ទ :";
            sheet.Cell("I8").Value = LoanSchedule?.AgentPhone;
            sheet.Cell("H9").Value = ":";
            sheet.Cell("I9").Value = "";
            sheet.Cell("H10").Value = "ខ្ចីលើកទី :";
            sheet.Cell("I10").Value = LoanSchedule?.Circle;
            var infoSection3 = sheet.Range("H7:H10");
            infoSection3.Style.Font.FontName = "Khmer OS Battambang";
            infoSection3.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            sheet.Range("I7:I10").Style.Font.Bold = true;
            sheet.Range("I7:I10").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            // Table Headers
            sheet.Range("B12").Value = "ល.រ";
            sheet.Range("C12:D12").Merge().Value = "ថ្ងៃត្រូវបង់ប្រាក់";
            sheet.Range("E12").Value = "ប្រាក់ដើមត្រូវបង់";
            sheet.Range("F12").Value = "ការប្រាក់ត្រូវបង់";
            sheet.Range("G12").Value = "សរុបប្រាក់ដែលត្រូវបង់";
            sheet.Range("H12").Value = "ប្រាក់ដើមនៅសល់";
            var tableHeaders = sheet.Range("B12:H12");
            tableHeaders.Style.Font.FontName = "Khmer OS Battambang";
            tableHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            tableHeaders.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            tableHeaders.Style.Border.SetOutsideBorder(XLBorderStyleValues.Hair)
                .Border.SetInsideBorder(XLBorderStyleValues.Hair);
            // Table Content
            int currentRow = 13;
            foreach (var item in LoanSchedule!.Schedules)
            {
                sheet.Cell(currentRow, 2).Value = item.InstallmentNo;
                sheet.Cell(currentRow, 3).Value = item.PaymentDate?.ToString("dd-MM-yyyy");
                sheet.Cell(currentRow, 4).Value = item.TotalDay;
                sheet.Cell(currentRow, 5).Value = item.Principle;
                sheet.Cell(currentRow, 6).Value = item.TotalFee;//Total Fee = interest + CollectionFee + OtherFee
                sheet.Cell(currentRow, 7).Value = item.TotalPay;
                sheet.Cell(currentRow, 8).Value = item.LoanOutstanding;
                var tableRow = sheet.Range(currentRow, 2, currentRow, 4);
                tableRow.Style.Font.FontName = "Khmer OS Battambang";
                tableRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                tableRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                tableRow.Style.Border.SetOutsideBorder(XLBorderStyleValues.Hair)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);

                var currencyRow = sheet.Range(currentRow, 5, currentRow, 8);
                currencyRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                currencyRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                currencyRow.Style.Border.SetOutsideBorder(XLBorderStyleValues.Hair)
                    .Border.SetInsideBorder(XLBorderStyleValues.Hair);


                currentRow++;
            }
            // Total Row
            var totalLabelRange = sheet.Range($"B{currentRow}:C{currentRow}").Merge();
            totalLabelRange.Value = "សរុប";
            totalLabelRange.Style.Font.FontName = "Khmer OS Battambang";
            totalLabelRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            totalLabelRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Cell(currentRow, 4).Value = schedule.Sum(s => s.TotalDay);
            sheet.Cell(currentRow, 5).Value = schedule.Sum(s => s.Principle);
            sheet.Cell(currentRow, 6).Value = schedule.Sum(s => s.Interest);
            var currencyTotalRow = sheet.Range(currentRow, 4, currentRow, 6);
            currencyTotalRow.Style.Font.FontName = "Times New Roman";
            currencyTotalRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            currencyTotalRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Range(currentRow, 2, currentRow, 8).Style.Border.SetOutsideBorder(XLBorderStyleValues.Hair)
                .Border.SetInsideBorder(XLBorderStyleValues.Hair);
            //Format currency cells
            if (LoanSchedule!.CurrencyId == 2)
            {
                sheet.Range($"E13:H{currentRow}").Style.Font.FontName = "Times New Roman";
                sheet.Range($"E13:H{currentRow}").Style.NumberFormat.Format = "#,##0.00 $";

            }
            else
            {
                sheet.Range($"E13:H{currentRow}").Style.Font.FontName = "Khmer OS Battambang";
                sheet.Range($"E13:H{currentRow}").Style.NumberFormat.Format = "#,##0 ៛";

            }

            // signature Section
            var signatureRange = sheet.Range($"C{currentRow + 1}:B{currentRow + 1}").Merge();
            signatureRange.Value = "ស្នាមមេដៃស្តាំអតិថិជន";
            signatureRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            var signateDateRange = sheet.Range($"C{currentRow + 6}:B{currentRow + 6}").Merge();
            signateDateRange.Value = DateTime.Now.ToString("dd-MM-yyyy");
            signateDateRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            signateDateRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            var signatureAgentRange = sheet.Cell($"I{currentRow + 1}");
            signatureAgentRange.Value = "ហត្ថលេខាអ្នករៀបចំ";
            var signateAgentDateRange = sheet.Cell($"I{currentRow + 6}");
            signateAgentDateRange.Value = DateTime.Now.ToString("dd-MM-yyyy");
            signateAgentDateRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            signateAgentDateRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            // Adjust column widths
            sheet.Columns().AdjustToContents();

            sheet.Columns(2, 2).Width = 8; // Set width for column B


            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream;
        }
        catch (Exception)
        {
            return null;
        }
    }
}

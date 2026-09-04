using Application.Interfaces.Imports;
using MiniExcelLibs;
namespace Application.Services.Imports
{
    public sealed class ExcelImportServiceImpl : IExcelImportService 
    {
        //public async Task<(bool Success, string Message)> ImportExcelAsync(Stream fileStream)
        //{
        //    try
        //    {
        //        // Read Excel file using MiniExcel
        //        var rows = await MiniExcel.QueryAsync(fileStream, useHeaderRow: true).ToListAsync();

        //        if (!rows.Any())
        //            return (false, "Excel file is empty.");

        //        // Validate headers
        //        var firstRow = rows.First();
        //        if (!firstRow.ContainsKey("Name") || !firstRow.ContainsKey("Price") || !firstRow.ContainsKey("Quantity"))
        //        {
        //            return (false, "Invalid Excel format. Expected columns: Name, Price, Quantity.");
        //        }

        //        // Map to Product objects
        //        var products = new List<Product>();
               
        //        for (int i = 0; i < rows.Count; i++)
        //        {
        //            var row = rows[i];
        //            if (!decimal.TryParse(row["Price"]?.ToString(), out var price) ||
        //                !int.TryParse(row["Quantity"]?.ToString(), out var quantity))
        //            {
        //                return (false, $"Invalid data in row {i + 2}.");
        //            }

        //            products.Add(new Product
        //            {
        //                Name = row["Name"]?.ToString() ?? string.Empty,
        //                Price = price,
        //                Quantity = quantity
        //            });
        //        }

        //        // Save to database
        //        //await _context.Products.AddRangeAsync(products);
        //        //await _context.SaveChangesAsync();

        //        return (true, $"{products.Count} products imported successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return (false, $"Error importing Excel file: {ex.Message}");
        //    }
        //}
    
    
    }
}


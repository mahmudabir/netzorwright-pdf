using System.Diagnostics;

using NetzorwrightPdf.WebApi.Views;

namespace NetzorwrightPdf.WebApi;

public static class PdfService
{
    public static async Task<IResult> GeneratePdf(HttpResponse httpResponse, HttpContext httpContext)
    {
        var shopLogo = "";
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://tassistant.vercel.app/assets/layout/images/logo2x.png");

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var logoBytes = new byte[responseStream.Length];
                await responseStream.ReadAsync(logoBytes, 0, (int)responseStream.Length);


                shopLogo = Convert.ToBase64String(logoBytes);
            }
        }
        catch (Exception ex)
        {
        }

        var invoiceViewModel = new InvoiceViewModel
        {
            ShopLogo = shopLogo,
            ShopName = "Shop Name",
            ShopEmail = "demo@gmail.com",
            ShopPhone = "1234567890",
            InvoiceDate = DateTime.Now,
            InvoiceNumber = "12345",
            ShopAddress = "Dhaka",
            TaxRate = 15,
            Items = new List<InvoiceItem>
            {
                new InvoiceItem
                {
                    Description = "Milk",
                     UnitPrice = 90,
                     Quantity = 1
                },
                new InvoiceItem
                {
                    Description = "Egg",
                    UnitPrice = 13.75M,
                    Quantity = 4
                },
                new InvoiceItem
                {
                    Description = "Potato",
                    UnitPrice = 60,
                    Quantity = 1
                }
            }
        };

        var emptyInvoiceViewModel = invoiceViewModel;
        emptyInvoiceViewModel.Items = new List<InvoiceItem>();

        #region Benchmark

        // Razor-DinkToPdf
        {
            // PDF with data
            Stopwatch sw = Stopwatch.StartNew();
            Netzorwright.DinkToPdfGenerator.PdfFilePath("Razor-DinkToPdf\\person-list-with-data.pdf");
            var isSuccess1 = await Netzorwright.DinkToPdfGenerator.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", invoiceViewModel));
            Console.WriteLine((isSuccess1 ? "Success" : "Failed") + ": Razor-DinkToPdf\\person-list-with-data.pdf");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("============================================================================");

            // PDF without data
            sw = Stopwatch.StartNew();
            Netzorwright.DinkToPdfGenerator.PdfFilePath("Razor-DinkToPdf\\person-list-without-data.pdf");
            var viewData = new Dictionary<string, object> { { "Name", "Abir Mahmud" } };
            var isSuccess2 = await Netzorwright.DinkToPdfGenerator.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", emptyInvoiceViewModel, viewData));
            Console.WriteLine((isSuccess2 ? "Success" : "Failed") + ": Razor-DinkToPdf\\person-list-without-data.pdf");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("============================================================================");
        }

        // Razor-Playwright
        {
            // PDF with data
            Stopwatch sw = Stopwatch.StartNew();
            Netzorwright.PlaywrightGenerator.PdfFilePath("Razor-Playwright\\person-list-with-data.pdf");
            var isSuccess1 = await Netzorwright.PlaywrightGenerator.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", invoiceViewModel), null, null);
            Console.WriteLine((isSuccess1 ? "Success" : "Failed") + ": Razor-Playwright\\person-list-with-data.pdf");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("============================================================================");

            // PDF without data
            sw = Stopwatch.StartNew();
            Netzorwright.PlaywrightGenerator.PdfFilePath("Razor-Playwright\\person-list-without-data.pdf");
            var isSuccess2 = await Netzorwright.PlaywrightGenerator.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", emptyInvoiceViewModel), null, null);
            Console.WriteLine((isSuccess2 ? "Success" : "Failed") + ": Razor-Playwright\\person-list-without-data.pdf");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("============================================================================");
        }

        // Blazor-DinkToPdf
        {
            // PDF with data
            Stopwatch sw = Stopwatch.StartNew();
            var data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
            Netzorwright.DinkToPdfGenerator.PdfFilePath("Blazor-DinkToPdf\\person-list-with-data.pdf");
            var isSuccess1 = await Netzorwright.DinkToPdfGenerator.GeneratePdfAsync(await Netzorwright.BlazorRenderer.RenderViewToStringAsync(data, typeof(InvoiceView)));
            Console.WriteLine((isSuccess1 ? "Success" : "Failed") + ": Blazor-DinkToPdf\\person-list-with-data.pdf");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("============================================================================");

            // PDF without data
            sw = Stopwatch.StartNew();
            data = new Dictionary<string, object?> { { "Invoice", emptyInvoiceViewModel } };
            Netzorwright.DinkToPdfGenerator.PdfFilePath("Blazor-DinkToPdf\\person-list-without-data.pdf");
            var isSuccess2 = await Netzorwright.DinkToPdfGenerator.GeneratePdfAsync(await Netzorwright.BlazorRenderer.RenderViewToStringAsync(data, typeof(InvoiceView)));
            Console.WriteLine((isSuccess2 ? "Success" : "Failed") + ": Blazor-DinkToPdf\\person-list-without-data.pdf");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("============================================================================");
        }

        // Blazor-Playwright
        {
            // PDF with data
            Stopwatch sw = Stopwatch.StartNew();
            var data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
            Netzorwright.PlaywrightGenerator.PdfFilePath("Blazor-Playwright\\person-list-with-data.pdf");
            var isSuccess1 = await Netzorwright.PlaywrightGenerator.GeneratePdfAsync(await Netzorwright.BlazorRenderer.RenderViewToStringAsync(data, typeof(InvoiceView)), null, null);
            Console.WriteLine((isSuccess1 ? "Success" : "Failed") + ": Blazor-Playwright\\person-list-with-data.pdf");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("============================================================================");

            // PDF without data
            sw = Stopwatch.StartNew();
            data = new Dictionary<string, object?> { { "Invoice", emptyInvoiceViewModel } };
            Netzorwright.PlaywrightGenerator.PdfFilePath("Blazor-Playwright\\person-list-without-data.pdf");
            var isSuccess2 = await Netzorwright.PlaywrightGenerator.GeneratePdfAsync(await Netzorwright.BlazorRenderer.RenderViewToStringAsync(data, typeof(InvoiceView)), null, null);
            Console.WriteLine((isSuccess2 ? "Success" : "Failed") + ": Blazor-Playwright\\person-list-without-data.pdf");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            Console.WriteLine("============================================================================");
        }

        #endregion Benchmark

        return TypedResults.Ok(new { Message = "Successful" });
    }
}

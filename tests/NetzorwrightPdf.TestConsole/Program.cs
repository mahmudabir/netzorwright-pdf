// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

using NetzorwrightPdf;
using NetzorwrightPdf.TestConsole;
using NetzorwrightPdf.TestConsole.Views;

Console.WriteLine("Hello, NetzorwrightPdf!");

Netzorwright.Initialize();

var shopLogo = "";

try
{
    using var httpClient = new HttpClient();
    var response = await httpClient.GetAsync("https://tassistant.vercel.app/assets/layout/images/logo2x.png");

    if (response.IsSuccessStatusCode)
    {
        using var responseStream =await  response.Content.ReadAsStreamAsync();
        var logoBytes = new byte[responseStream.Length];
        await responseStream.ReadAsync(logoBytes, 0, (int)responseStream.Length);


        shopLogo = Convert.ToBase64String(logoBytes);
        // Console.WriteLine($"Base64 representation of the image: {logoBase64String}");
    }
    else
    {
        // Console.WriteLine($"Error: {response.StatusCode}");
    }
}
catch (Exception ex)
{
    // Console.WriteLine($"An error occurred: {ex.Message}");
}

{
    var items = new List<InvoiceItem>
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
    };

    var invoiceViewModel = new InvoiceViewModel
    {
        ShopLogo = shopLogo,
        ShopName = "Test",
        ShopEmail = "demo@gmail.com",
        ShopPhone = "1234567890",
        InvoiceDate = DateTime.Now,
        InvoiceNumber = "12345",
        ShopAddress = "Dhaka",
        TaxRate = 15,
        Items = items
    };

    Directory.CreateDirectory("Razor-DinkToPdf");
    var data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
    Netzorwright.DinkToPdfGenerator.PdfFilePath("Razor-DinkToPdf\\person-list-with-data-1.pdf");
    var isSuccess1 = await Netzorwright.DinkToPdfGenerator.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", invoiceViewModel));

    items.Clear();
    data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
    Netzorwright.DinkToPdfGenerator.PdfFilePath("Razor-DinkToPdf\\person-list-without-data-1.pdf");
    var isSuccess2 = await Netzorwright.DinkToPdfGenerator.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", invoiceViewModel));
}

{
    var items = new List<InvoiceItem>
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
    };

    var invoiceViewModel = new InvoiceViewModel
    {
        ShopLogo = shopLogo,
        ShopName = "Test",
        ShopEmail = "demo@gmail.com",
        ShopPhone = "1234567890",
        InvoiceDate = DateTime.Now,
        InvoiceNumber = "12345",
        ShopAddress = "Dhaka",
        TaxRate = 15,
        Items = items
    };

    Directory.CreateDirectory("Razor-DinkToPdf");

    Stopwatch sw = Stopwatch.StartNew();
    var data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
    Netzorwright.DinkToPdfGenerator.PdfFilePath("Razor-DinkToPdf\\person-list-with-data.pdf");
    var isSuccess1 = await Netzorwright.DinkToPdfGenerator.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", invoiceViewModel));
    Console.WriteLine((isSuccess1 ? "Success" : "Failed") + ": Razor-DinkToPdf\\person-list-with-data.pdf");
    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    Console.WriteLine("============================================================================");

    sw = Stopwatch.StartNew();
    items.Clear();
    data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
    Netzorwright.DinkToPdfGenerator.PdfFilePath("Razor-DinkToPdf\\person-list-without-data.pdf");
    var isSuccess2 = await Netzorwright.DinkToPdfGenerator.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", invoiceViewModel));
    Console.WriteLine((isSuccess2 ? "Success" : "Failed") + ": Razor-DinkToPdf\\person-list-without-data.pdf");
    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    Console.WriteLine("============================================================================");
}

{
    var items = new List<InvoiceItem>
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
    };

    var invoiceViewModel = new InvoiceViewModel
    {
        ShopLogo = shopLogo,
        ShopName = "Test",
        ShopEmail = "demo@gmail.com",
        ShopPhone = "1234567890",
        InvoiceDate = DateTime.Now,
        InvoiceNumber = "12345",
        ShopAddress = "Dhaka",
        TaxRate = 15,
        Items = items
    };

    Directory.CreateDirectory("Razor-Playwright");

    Stopwatch sw = Stopwatch.StartNew();
    var data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
    Netzorwright.PlaywrightGenerator.PdfFilePath("Razor-Playwright\\person-list-with-data.pdf");
    var isSuccess1 = await Netzorwright.PlaywrightGenerator.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", invoiceViewModel), null, null);
    Console.WriteLine((isSuccess1 ? "Success" : "Failed") + ": Razor-Playwright\\person-list-with-data.pdf");
    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    Console.WriteLine("============================================================================");

    sw = Stopwatch.StartNew();
    items.Clear();
    data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
    Netzorwright.PlaywrightGenerator.PdfFilePath("Razor-Playwright\\person-list-without-data.pdf");
    var isSuccess2 = await Netzorwright.PlaywrightGenerator.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", invoiceViewModel), null, null);
    Console.WriteLine((isSuccess2 ? "Success" : "Failed") + ": Razor-Playwright\\person-list-without-data.pdf");
    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    Console.WriteLine("============================================================================");
}


{
    var items = new List<InvoiceItem>
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
    };

    var invoiceViewModel = new InvoiceViewModel
    {
        ShopLogo = shopLogo,
        ShopName = "Test",
        ShopEmail = "demo@gmail.com",
        ShopPhone = "1234567890",
        InvoiceDate = DateTime.Now,
        InvoiceNumber = "12345",
        ShopAddress = "Dhaka",
        TaxRate = 15,
        Items = items
    };

    Directory.CreateDirectory("Blazor-DinkToPdf");

    Stopwatch sw = Stopwatch.StartNew();
    var data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
    Netzorwright.DinkToPdfGenerator.PdfFilePath("Blazor-DinkToPdf\\person-list-with-data.pdf");
    var isSuccess1 = await Netzorwright.DinkToPdfGenerator.GeneratePdfAsync(await Netzorwright.BlazorRenderer.RenderViewToStringAsync(data, typeof(InvoiceView)));
    Console.WriteLine((isSuccess1 ? "Success" : "Failed") + ": Blazor-DinkToPdf\\person-list-with-data.pdf");
    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    Console.WriteLine("============================================================================");

    sw = Stopwatch.StartNew();
    items.Clear();
    data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
    Netzorwright.DinkToPdfGenerator.PdfFilePath("Blazor-DinkToPdf\\person-list-without-data.pdf");
    var isSuccess2 = await Netzorwright.DinkToPdfGenerator.GeneratePdfAsync(await Netzorwright.BlazorRenderer.RenderViewToStringAsync(data, typeof(InvoiceView)));
    Console.WriteLine((isSuccess2 ? "Success" : "Failed") + ": Blazor-DinkToPdf\\person-list-without-data.pdf");
    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    Console.WriteLine("============================================================================");
}

{
    var items = new List<InvoiceItem>
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
    };

    var invoiceViewModel = new InvoiceViewModel
    {
        ShopLogo = shopLogo,
        ShopName = "Test",
        ShopEmail = "demo@gmail.com",
        ShopPhone = "1234567890",
        InvoiceDate = DateTime.Now,
        InvoiceNumber = "12345",
        ShopAddress = "Dhaka",
        TaxRate = 15,
        Items = items
    };

    Directory.CreateDirectory("Blazor-Playwright");

    Stopwatch sw = Stopwatch.StartNew();
    var data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
    Netzorwright.PlaywrightGenerator.PdfFilePath("Blazor-Playwright\\person-list-with-data.pdf");
    var isSuccess1 = await Netzorwright.PlaywrightGenerator.GeneratePdfAsync(await Netzorwright.BlazorRenderer.RenderViewToStringAsync(data, typeof(InvoiceView)), null, null);
    Console.WriteLine((isSuccess1 ? "Success" : "Failed") + ": Blazor-Playwright\\person-list-with-data.pdf");
    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    Console.WriteLine("============================================================================");

    sw = Stopwatch.StartNew();
    items.Clear();
    data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
    Netzorwright.PlaywrightGenerator.PdfFilePath("Blazor-Playwright\\person-list-without-data.pdf");
    var isSuccess2 = await Netzorwright.PlaywrightGenerator.GeneratePdfAsync(await Netzorwright.BlazorRenderer.RenderViewToStringAsync(data, typeof(InvoiceView)), null, null);
    Console.WriteLine((isSuccess2 ? "Success" : "Failed") + ": Blazor-Playwright\\person-list-without-data.pdf");
    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    Console.WriteLine("============================================================================");
}
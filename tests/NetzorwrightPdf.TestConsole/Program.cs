// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

using Microsoft.AspNetCore.Components.Web;

using NetzorwrightPdf;
using NetzorwrightPdf.TestConsole;
using NetzorwrightPdf.TestConsole.Views;

Console.WriteLine("Hello, NetzorwrightPdf!");

Netzorwright.Initialize();

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

InvoiceViewModel invoiceViewModel = new InvoiceViewModel
{
    InvoiceDate = DateTime.Now,
    InvoiceNumber = "12345",
    ShopAddress = "Dhaka",
    ShopEmail = "demo@gmail.com",
    ShopName = "Test",
    ShopPhone = "1234567890",
    TaxRate = 15,
    Items = items
};

Stopwatch sw = Stopwatch.StartNew();
var data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
Netzorwright.PdfFilePath("../../../Blazor/person-list-with-data.pdf");
var isSuccess1 = await Netzorwright.GeneratePdfAsync(await Netzorwright.BlazorRenderer.GenerateHtmlAsync(data, typeof(InvoiceView)));
Console.WriteLine((isSuccess1 ? "Success" : "Failed") + ": Blazor/person-list-with-data.pdf");
sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds + "ms");
Console.WriteLine("============================================================================");

sw = Stopwatch.StartNew();
items.Clear();
data = new Dictionary<string, object?> { { "Invoice", invoiceViewModel } };
Netzorwright.PdfFilePath("../../../Blazor/person-list-without-data.pdf");
var isSuccess2 = await Netzorwright.GeneratePdfAsync(await Netzorwright.BlazorRenderer.GenerateHtmlAsync(data, typeof(InvoiceView)));
Console.WriteLine((isSuccess2 ? "Success" : "Failed") + ": Blazor/person-list-without-data.pdf");
sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds + "ms");
Console.WriteLine("============================================================================");

items = new List<InvoiceItem>
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

invoiceViewModel = new InvoiceViewModel
{
    InvoiceDate = DateTime.Now,
    InvoiceNumber = "12345",
    ShopAddress = "Dhaka",
    ShopEmail = "demo@gmail.com",
    ShopName = "Test",
    ShopPhone = "1234567890",
    TaxRate = 15,
    Items = items
};

sw = Stopwatch.StartNew();
Netzorwright.PdfFilePath("../../../Razor/person-list-with-data.pdf");
var isSuccess3 = await Netzorwright.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", invoiceViewModel));
Console.WriteLine((isSuccess3 ? "Success" : "Failed") + ": Razor/person-list-with-data.pdf");
sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds + "ms");
Console.WriteLine("============================================================================");

sw = Stopwatch.StartNew();
items.Clear();
Netzorwright.PdfFilePath("../../../Razor/person-list-without-data.pdf");
var isSuccess4 = await Netzorwright.GeneratePdfAsync(await Netzorwright.RazorRenderer.RenderViewToStringAsync("InvoiceView", invoiceViewModel));
Console.WriteLine((isSuccess4 ? "Success" : "Failed") + ": Razor/person-list-without-data.pdf");
sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds + "ms");
Console.WriteLine("============================================================================");
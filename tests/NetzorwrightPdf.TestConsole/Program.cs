// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

using NetzorwrightPdf;
using NetzorwrightPdf.TestConsole;

Console.WriteLine("Hello, NetzorwrightPdf!");

Netzowright.Initialize();

var people = new List<Person>()
{
    new Person("Abir", 27, Gender.Male),
    new Person("Mahmud", 27, Gender.Male),
    new Person("Mahmud", 27, Gender.Male),
    new Person("Sadiya", 22, Gender.Female),
    new Person("Swapnil", 22, Gender.Female),
};

Stopwatch sw = Stopwatch.StartNew();

var data = new Dictionary<string, object?> { { "People", people } };
Netzowright.PdfFilePath("../../../person-list-with-data.pdf");
var isSuccess1 = Netzowright.GeneratePdfAsync(await Netzowright.GenerateHtmlAsync(data, typeof(PersonListView)), Netzowright.DefaultPagePdfOptions, Netzowright.DefaultBrowserTypeLaunchOptions);

people.Clear();
data = new Dictionary<string, object?> { { "People", people } };
Netzowright.PdfFilePath("../../../person-list-without-data.pdf");
var isSuccess2 = Netzowright.GeneratePdfAsync(await Netzowright.GenerateHtmlAsync(data, typeof(PersonListView)), Netzowright.DefaultPagePdfOptions);

Console.WriteLine((await isSuccess1 ? "Success" : "Failed") + ": person-list-with-data.pdf");
Console.WriteLine((await isSuccess2 ? "Success" : "Failed") + ": person-list-without-data.pdf");

sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds + "ms");

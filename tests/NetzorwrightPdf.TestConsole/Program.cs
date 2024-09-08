// See https://aka.ms/new-console-template for more information
using Microsoft.Playwright;

using NetzorwrightPdf;
using NetzorwrightPdf.TestConsole;

Console.WriteLine("Hello, NetzorwrightPdf!");

var netzowright = Netzowright.Initialize().IsHeadless();
PagePdfOptions pagePdfOptions = new PagePdfOptions { Format = "A4" };

var people = new List<Person>()
{
    new Person("Abir", 27, Gender.Male),
    new Person("Mahmud", 27, Gender.Male),
    new Person("Mahmud", 27, Gender.Male),
    new Person("Sadiya", 22, Gender.Female),
    new Person("Swapnil", 22, Gender.Female),
};

var data = new Dictionary<string, object?> { { "People", people } };
var htmlStringWithData = await netzowright.GenerateHtmlAsync(data, typeof(PersonListView));
var isSuccess = await netzowright.PdfFilePath("../../../person-list-with-data.pdf").GeneratePdfAsync(htmlStringWithData, pagePdfOptions);
Console.WriteLine((isSuccess ? "Success" : "Failed") + ": person-list-with-data.pdf");

people.Clear();
data = new Dictionary<string, object?> { { "People", people } };
var htmlStringWithoutData = await netzowright.GenerateHtmlAsync(data, typeof(PersonListView));
isSuccess = await netzowright.PdfFilePath("../../../person-list-without-data.pdf").GeneratePdfAsync(htmlStringWithoutData, pagePdfOptions);
Console.WriteLine((isSuccess ? "Success" : "Failed") + ": person-list-without-data.pdf");


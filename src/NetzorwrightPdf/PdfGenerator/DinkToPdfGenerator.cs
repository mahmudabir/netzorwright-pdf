using DinkToPdf;
using DinkToPdf.Contracts;

using Microsoft.Extensions.DependencyInjection;

namespace NetzorwrightPdf.PdfGenerator;
public class DinkToPdfGenerator
{
    private static string? _filePath;
    private readonly IServiceProvider _serviceProvider;

    public DinkToPdfGenerator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void PdfFilePath(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<bool> GeneratePdfAsync(string htmlString)
    {
        try
        {
            _filePath = _filePath ?? "./netzowright.pdf";
            var fullpath = Path.Combine(Directory.GetCurrentDirectory(), _filePath);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10 },
                    Out = fullpath,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = htmlString,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        //HeaderSettings = { FontSize = 10, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                        HeaderSettings = { FontSize = 10, Line = true, Spacing = 2.812 }
                    }
                }
            };

            IConverter converter = _serviceProvider.GetRequiredService<IConverter>();
            var pdfBytes = converter.Convert(doc);

            _filePath = null;
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }

}

using DinkToPdf;
using DinkToPdf.Contracts;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

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
            var fileProvider = _serviceProvider.GetRequiredService<IFileProvider>() as PhysicalFileProvider;
            _filePath = Path.Combine(fileProvider.Root, (_filePath ?? "./netzowright.pdf"));
            var fullPath = _filePath;
            CreateUnavailableDirectory(fullPath);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10 },
                    Out = fullPath,
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

    public async void CreateUnavailableDirectory(string fullPath)
    {
        // Extract the directory path from the file path
        string directoryPath = Path.GetDirectoryName(fullPath);

        // Ensure the directory exists, create if it does not
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

}

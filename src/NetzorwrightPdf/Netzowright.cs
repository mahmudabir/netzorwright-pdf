using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace NetzorwrightPdf;

public class Netzowright
{
    private ServiceProvider _services { get; set; }
    private ILoggerFactory _loggerFactory { get; set; }
    private HtmlRenderer _htmlRenderer { get; set; }

    private bool _isHeadless { get; set; }
    private string? _filePath { get; set; }

    public static Netzowright Initialize()
    {
        return new Netzowright();
    }

    public Netzowright()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();

        _services = services.BuildServiceProvider();
        _loggerFactory = _services.GetRequiredService<ILoggerFactory>();
        _htmlRenderer = new HtmlRenderer(_services, _loggerFactory);
    }

    public Netzowright IsHeadless()
    {
        _isHeadless = true;
        return this;
    }

    public Netzowright NotHeadless()
    {
        _isHeadless = false;
        return this;
    }

    public Netzowright PdfFilePath(string filePath)
    {
        _filePath = filePath;
        return this;
    }

    public async Task<string> GenerateHtmlAsync(IDictionary<string, object?> keyValues, Type componentType)
    {
        var htmlString = await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var parameters = ParameterView.FromDictionary(keyValues);
            var output = await _htmlRenderer.RenderComponentAsync(componentType, parameters);
            return output.ToHtmlString();
        });

        return htmlString;
    }

    public async Task<bool> GeneratePdfAsync(string htmlString, PagePdfOptions pdfOptions, BrowserTypeLaunchOptions? browserTypeLaunchOptions = null)
    {
        try
        {
            Microsoft.Playwright.Program.Main(["install"]);

            var playwright = await Playwright.CreateAsync();

            if (browserTypeLaunchOptions == null && _isHeadless)
            {
                browserTypeLaunchOptions = new BrowserTypeLaunchOptions()
                {
                    Headless = true
                };
            }

            var browser = await playwright.Chromium.LaunchAsync(browserTypeLaunchOptions);

            var page = await browser.NewPageAsync();
            await page.SetContentAsync(htmlString ?? string.Empty);

            pdfOptions = pdfOptions ?? new PagePdfOptions
            {
                Format = "A4",
                Path = _filePath ?? "./netzowright.pdf"
            };

            pdfOptions.Path = _filePath ?? pdfOptions.Path  ?? "./netzowright.pdf";

            await page.PdfAsync(pdfOptions);

            await page.CloseAsync();

            playwright.Dispose();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace NetzorwrightPdf;

public class Netzowright
{
    private static ServiceProvider _services { get; set; }
    private static ILoggerFactory _loggerFactory { get; set; }
    private static HtmlRenderer _htmlRenderer { get; set; }

    private static bool _isHeadless { get; set; }
    private static string? _filePath { get; set; }

    public static PagePdfOptions DefaultPagePdfOptions { get; set; }
    public static BrowserTypeLaunchOptions DefaultBrowserTypeLaunchOptions { get; set; }

    public static void Initialize(bool isHeadless = true)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();

        _services = services.BuildServiceProvider();
        _loggerFactory = _services.GetRequiredService<ILoggerFactory>();
        _htmlRenderer = new HtmlRenderer(_services, _loggerFactory);
        _isHeadless = isHeadless;
        DefaultPagePdfOptions = new PagePdfOptions { Format = "A4" };
        DefaultBrowserTypeLaunchOptions = new BrowserTypeLaunchOptions { Headless = _isHeadless };

        Task.Run(() => Microsoft.Playwright.Program.Main(["install"]));
    }

    public static void IsHeadless()
    {
        _isHeadless = true;
    }

    public static void NotHeadless()
    {
        _isHeadless = false;
    }

    public static void PdfFilePath(string filePath)
    {
        _filePath = filePath;
    }

    public static async Task<string> GenerateHtmlAsync(IDictionary<string, object?> keyValues, Type componentType)
    {
        var htmlString = await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var parameters = ParameterView.FromDictionary(keyValues);
            var output = await _htmlRenderer.RenderComponentAsync(componentType, parameters);
            return output.ToHtmlString();
        });

        return htmlString;
    }

    public static async Task<bool> GeneratePdfAsync(string htmlString, PagePdfOptions pagePdfOptions, BrowserTypeLaunchOptions? browserTypeLaunchOptions = null)
    {
        try
        {
            var playwright = await Playwright.CreateAsync();

            if (browserTypeLaunchOptions == null)
            {
                browserTypeLaunchOptions = new BrowserTypeLaunchOptions()
                {
                    Headless = _isHeadless
                };
            }

            var browser = await playwright.Chromium.LaunchAsync(browserTypeLaunchOptions);

            var page = await browser.NewPageAsync();
            await page.SetContentAsync(htmlString ?? string.Empty);

            pagePdfOptions = pagePdfOptions ?? DefaultPagePdfOptions;

            pagePdfOptions.Path = _filePath ?? pagePdfOptions.Path ?? "./netzowright.pdf";

            await page.PdfAsync(pagePdfOptions);

            await page.CloseAsync();

            playwright.Dispose();

            _filePath = null;
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }
}

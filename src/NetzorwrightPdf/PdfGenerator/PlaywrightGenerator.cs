using Microsoft.Playwright;

namespace NetzorwrightPdf.PdfGenerator;
public class PlaywrightGenerator
{
    private bool _isHeadless;
    private readonly PagePdfOptions _defaultPagePdfOptions;
    private readonly BrowserTypeLaunchOptions? _browserTypeLaunchOptions;
    private string? _filePath { get; set; }

    public PlaywrightGenerator(IServiceProvider serviceProvider, bool isHeadless, PagePdfOptions? defaultPagePdfOptions, BrowserTypeLaunchOptions? browserTypeLaunchOptions)
    {
        _isHeadless = isHeadless;
        _defaultPagePdfOptions = defaultPagePdfOptions;
        _browserTypeLaunchOptions = browserTypeLaunchOptions;
    }

    public void IsHeadless()
    {
        _isHeadless = true;
    }

    public void NotHeadless()
    {
        _isHeadless = false;
    }

    public void PdfFilePath(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<bool> GeneratePdfAsync(string htmlString, PagePdfOptions? pagePdfOptions, BrowserTypeLaunchOptions? browserTypeLaunchOptions)
    {
        try
        {
            var playwright = await Playwright.CreateAsync();

            if (browserTypeLaunchOptions == null)
            {
                browserTypeLaunchOptions = _browserTypeLaunchOptions;
            }

            var browser = await playwright.Chromium.LaunchAsync(browserTypeLaunchOptions);

            var page = await browser.NewPageAsync();
            await page.SetContentAsync(htmlString ?? string.Empty);

            pagePdfOptions = pagePdfOptions ?? _defaultPagePdfOptions;

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

using System.Diagnostics;

using DinkToPdf;
using DinkToPdf.Contracts;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Playwright;

using NetzorwrightPdf.PdfGenerator;
using NetzorwrightPdf.Renderer;

namespace NetzorwrightPdf;

public static class Netzorwright
{
    private static bool _isHeadless { get; set; }
    private static string? _filePath { get; set; }
    private static PagePdfOptions _defaultPagePdfOptions { get; set; } = new PagePdfOptions { Format = "A4" };

    public static PagePdfOptions DefaultPagePdfOptions { get; set; }
    public static BrowserTypeLaunchOptions DefaultBrowserTypeLaunchOptions { get; set; }

    public static BlazorRenderer BlazorRenderer { get; set; }
    public static RazorRenderer RazorRenderer { get; set; }

    public static PlaywrightGenerator PlaywrightGenerator { get; set; }
    public static DinkToPdfGenerator DinkToPdfGenerator { get; set; }

    public static void Initialize(bool isHeadless = true)
    {
        var serviceProvider = BuildServiceProvider();

        _isHeadless = isHeadless;
        DefaultPagePdfOptions = _defaultPagePdfOptions;
        DefaultBrowserTypeLaunchOptions = new BrowserTypeLaunchOptions { Headless = _isHeadless };

        RazorRenderer = new RazorRenderer(serviceProvider);
        BlazorRenderer = new BlazorRenderer(serviceProvider); //Can be optimized like: Razor.Templating.Core

        PlaywrightGenerator = new PlaywrightGenerator(serviceProvider, isHeadless, DefaultPagePdfOptions, DefaultBrowserTypeLaunchOptions);
        DinkToPdfGenerator = new DinkToPdfGenerator(serviceProvider);

        //Task.Run(() => Microsoft.Playwright.Program.Main(["install"]));
        Microsoft.Playwright.Program.Main(["install"]);
    }

    public static IServiceProvider BuildServiceProvider()
    {
        IServiceCollection services = new ServiceCollection();

        #region Razor & Blazor
        // Set up configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        services.AddSingleton<IConfiguration>(configuration);

#if DEBUG
        string rootPath = "..\\..\\..\\";
        string environmentName = Environments.Development;
#else
        string rootPath = "";
        string environmentName = Environments.Production;
#endif

        // Set up the file provider
        var path = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), rootPath));
        Directory.SetCurrentDirectory(path.Root);
        var fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory()));

        // Add necessary services
        var hostingEnvironment = new HostingEnvironment
        {
            EnvironmentName = environmentName,
            ApplicationName = typeof(RazorRenderer).Assembly.GetName().Name,
            ContentRootPath = Directory.GetCurrentDirectory(),
            WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
            ContentRootFileProvider = fileProvider,
            WebRootFileProvider = fileProvider
        };

        services.AddSingleton<IWebHostEnvironment>(hostingEnvironment);
        services.AddSingleton<IHostEnvironment>(hostingEnvironment);

        services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

        var diagnosticListener = new DiagnosticListener("Microsoft.AspNetCore");
        services.AddSingleton<DiagnosticSource>(diagnosticListener);
        services.AddSingleton(diagnosticListener);

        services.AddLogging();

        // Add MVC services
        var mvcBuilder = services.AddMvc();

        // Add Razor runtime compilation
        mvcBuilder.AddRazorRuntimeCompilation(options =>
        {
            options.FileProviders.Add(fileProvider);
        });

        // Configure the application part manager
        services.AddSingleton<ApplicationPartManager>(sp =>
        {
            var manager = new ApplicationPartManager();
            var assemblies = new[]
            {
                typeof(RazorRenderer).Assembly,
                typeof(Microsoft.AspNetCore.Mvc.Razor.RazorPage).Assembly,
                typeof(Microsoft.AspNetCore.Mvc.Razor.RazorPageBase).Assembly,
                typeof(Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper).Assembly
            };

            foreach (var assembly in assemblies)
            {
                manager.ApplicationParts.Add(new AssemblyPart(assembly));
            }

            return manager;
        });

        // Add file provider
        services.AddSingleton<IFileProvider>(fileProvider);
        #endregion Razor & Blazor

        #region DinkToPdf

        services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

        #endregion DinkToPdf


        return services.BuildServiceProvider();
    }

    public static void IsHeadless()
    {
        _isHeadless = true;
        PlaywrightGenerator.IsHeadless();
    }

    public static void NotHeadless()
    {
        _isHeadless = false;
        PlaywrightGenerator.NotHeadless();
    }


}
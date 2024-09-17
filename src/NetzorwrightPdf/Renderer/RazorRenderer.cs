using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace NetzorwrightPdf.Renderer;
public class RazorRenderer
{
    private readonly IServiceProvider _serviceProvider;

    public RazorRenderer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model, IDictionary<string, object?>? viewData = null)
    {
        using var scope = _serviceProvider.CreateScope();
        var razorViewEngine = scope.ServiceProvider.GetRequiredService<IRazorViewEngine>();
        var razorRenderer = scope.ServiceProvider.GetRequiredService<IRazorPageActivator>();

        var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };

        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        var razorView = razorViewEngine.FindView(actionContext, viewName, false);
        //var razorView = razorViewEngine.GetView(null, viewName, false); //viewName == Views/ViewName.cshtml

        if (razorView.View == null)
        {
            throw new InvalidOperationException($"Unable to find view '{viewName}'");
        }
        else
        {
            using (var writer = new StringWriter())
            {
                var viewDataDictionary = new ViewDataDictionary<TModel>(metadataProvider: new EmptyModelMetadataProvider(), modelState: new ModelStateDictionary())
                {
                    Model = model
                };

                if (viewData != null)
                {
                    foreach (var item in viewData)
                    {
                        viewDataDictionary.Add(item.Key, item.Value);
                    }
                }

                var tempDataProvider = _serviceProvider.GetRequiredService<ITempDataProvider>();
                var tempData = new TempDataDictionary(actionContext.HttpContext, tempDataProvider);

                var htmlHelperOptions = new HtmlHelperOptions();

                var viewContext = new ViewContext(
                    actionContext,
                    razorView.View,
                    viewDataDictionary,
                    tempData,
                    writer,
                    htmlHelperOptions
                );

                await razorView.View.RenderAsync(viewContext);
                return writer.ToString();
            }
        }
    }
}

public class HostingEnvironment : IWebHostEnvironment, IHostEnvironment
{
    public string EnvironmentName { get; set; }
    public string ApplicationName { get; set; }
    public string WebRootPath { get; set; }
    public IFileProvider WebRootFileProvider { get; set; }
    public string ContentRootPath { get; set; }
    public IFileProvider ContentRootFileProvider { get; set; }
}

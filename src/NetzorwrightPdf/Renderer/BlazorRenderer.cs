using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NetzorwrightPdf.Renderer;
public class BlazorRenderer
{
    private HtmlRenderer _htmlRenderer { get; set; }

    public BlazorRenderer(IServiceProvider serviceProvider)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);
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
}

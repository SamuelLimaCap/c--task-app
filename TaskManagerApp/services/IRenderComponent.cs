using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using TaskManagerApp.extensions;


namespace TaskManagerApp.Services
{
    public interface IRenderComponent
    {
        /// <summary>
        /// Method <c>ToStringAsync</c> renders a view component to a string.
        /// </summary>
        /// <param name="componentName">The name of the view component. Example: "Select" versus the name of the class file "SelectViewComponent"</param>
        /// <param name="model">The model passed into the view component class file.</param>
        /// <returns>A <c>string</c> representing the generated HTML of the view component's 'Default.cshtml' file.</returns>
        Task<string> ToStringAsync(string componentName, object? arguments);
    }
}

namespace TaskManagerApp.Services
{
    // Ensure the line 'builder.Services.AddHttpContextAccessor();'
    // is added into 'Program.cs'.
    public sealed class RenderComponent(
        ITempDataProvider tempDataProvider,
        IServiceProvider serviceProvider,
        IHttpContextAccessor httpContextAccessor
    ) : IRenderComponent
    {
        public async Task<string> ToStringAsync(string componentName, object? arguments)
        {
            // The following code is adapted from:
            // https://gist.github.com/pauldotknopf/b424e9b8b03d31d67f3cce59f09ab17f

            DefaultViewComponentHelper helper = new(
              serviceProvider.GetRequiredService<IViewComponentDescriptorCollectionProvider>(),
              HtmlEncoder.Default,
              serviceProvider.GetRequiredService<IViewComponentSelector>(),
              serviceProvider.GetRequiredService<IViewComponentInvokerFactory>(),
              serviceProvider.GetRequiredService<IViewBufferScope>());

            HttpContext? httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null) { return string.Empty; }

            ActionContext actionContext = new(httpContext, new RouteData(), new ActionDescriptor());

            ViewDataDictionary viewData = new(
                metadataProvider: new EmptyModelMetadataProvider(),
                modelState: new ModelStateDictionary())
            {
            };

            TempDataDictionary tempData = new(actionContext.HttpContext, tempDataProvider);

            using StringWriter stringWriter = new();

            // Create a 'NullView' class file. See file 'NullView.cs'.
            ViewContext viewContext = new(actionContext, NullView.Instance, viewData, tempData, stringWriter, new HtmlHelperOptions());

            helper.Contextualize(viewContext);

            IHtmlContent result = await helper.InvokeAsync(componentName, arguments);

            result.WriteTo(stringWriter, HtmlEncoder.Default);

            await stringWriter.FlushAsync();

            return stringWriter.ToString();
        }
    }
}
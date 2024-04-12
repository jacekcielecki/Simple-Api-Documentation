using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SimpleApiDocumentation.Core.Endpoints;
using System.Reflection;
using System.Text.Json;

namespace SimpleApiDocumentation.Core.Document;

internal interface IDocumentProvider
{
    string GenerateDocument();
}

internal class DocumentProvider : IDocumentProvider
{
    private readonly ApiDocsOptions _options;
    private readonly IServiceProvider _serviceProvider;

    public DocumentProvider(ApiDocsOptions options, IServiceProvider serviceProvider)
    {
        _options = options;
        _serviceProvider = serviceProvider;
    }

    public string GenerateDocument()
    {
        EndpointModel[] endpoints = [..MinimalApiEndpoints(), ..ControllerEndpoints()];

        var document = DocumentHtmlTemplate();

        document = document.Replace("%(EndpointsList)%", JsonSerializer.Serialize(endpoints));
        document = document.Replace("%(DocumentTitle)%", _options.DocumentTitle);
            
        return document;
    }

    private string DocumentHtmlTemplate()
    {
        var assembly = typeof(AssemblyReference).GetTypeInfo().Assembly;

        Stream resource = assembly.GetManifestResourceStream("SimpleApiDocumentation.Core.index.html")!;
        StreamReader reader = new StreamReader(resource);

        return reader.ReadToEnd();
    }

    private EndpointModel[] MinimalApiEndpoints()
    {
        return _serviceProvider.GetServices<EndpointDataSource>()
            .SelectMany(x => x.Endpoints)
            .Select(x => x.ToEndpointModel())
            .ToArray();
    }

    private EndpointModel[] ControllerEndpoints()
    {
        var mvcActionDescriptor = _serviceProvider.GetService<IActionDescriptorCollectionProvider>();

        if (mvcActionDescriptor == null)
        {
            return [];
        }

        return mvcActionDescriptor.ActionDescriptors.Items
            .Where(x => x.AttributeRouteInfo != null)
            .Select(x => x.ToEndpointModel())
            .ToArray();
    }
}
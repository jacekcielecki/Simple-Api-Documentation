using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SimpleApiDocumentation.Core;

internal interface IApiDocsProvider
{
    string GenerateDocument();
}

internal class ApiDocsProvider : IApiDocsProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ApiDocsProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public string GenerateDocument()
    {
        EndpointModel[] endpoints = [..MinimalApiEndpoints(), ..ControllerEndpoints()];

        var content = 
            endpoints.Aggregate("", (current, endpoint) => current + $"<li> Endpoint: {endpoint.Name} {endpoint.Method} </li>");

        var html = HtmlTemplate().Replace("{{content}}", content);
        return html;
    }

    private string HtmlTemplate()
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
            .Select(x => new EndpointModel
            {
                Method = x.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods.FirstOrDefault(),
                Name = x.DisplayName
            })
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
            .Where(ad => ad.AttributeRouteInfo != null)
            .Select(ad => new EndpointModel
            {
                Method = ad.EndpointMetadata[6].ToString(),
                Name = ad.AttributeRouteInfo.Template
            })
            .ToArray();
    }
}
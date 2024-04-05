using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SimpleApiDocumentation.Core.Mapping;

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
            endpoints.Aggregate("", (current, endpoint) => current +
            $"<li class=\"{endpoint.Method?.ToLower()}\"> " +
            $"<div class=\"method\">{endpoint.Method}</div>" +
            $"{endpoint.Name}" +
            "</li>");

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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace SimpleApiDocumentation.Core.Endpoints;

internal static class EndpointModelMapping
{
    internal static EndpointModel ToEndpointModel(this ActionDescriptor actionDescriptor)
    {
        var name = actionDescriptor.AttributeRouteInfo.Template;
        var method = actionDescriptor.EndpointMetadata[7].ToString()?.Split(" ").Skip(1)
            .FirstOrDefault()?.Replace(",", "");

        return new EndpointModel
        {
            Method = method,
            Name = name
        };
    }

    internal static EndpointModel ToEndpointModel(this Endpoint endpoint)
    {
        var name = endpoint.DisplayName.Split(" ").Skip(2).FirstOrDefault();
        var method = endpoint.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods.FirstOrDefault();

        return new EndpointModel
        {
            Method = method,
            Name = name
        };
    }
}
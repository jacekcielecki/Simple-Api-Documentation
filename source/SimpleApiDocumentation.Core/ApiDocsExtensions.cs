using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimpleApiDocumentation.Core;

public static class ApiDocsExtensions
{
    /// <summary>
    /// Register the ApiDocs middleware.
    /// </summary>
    public static IApplicationBuilder UseApiDocs(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return app.UseMiddleware<ApiDocsMiddleware>();
    }

    /// <summary>
    /// Register ApiDocs services.
    /// </summary>
    public static IServiceCollection AddApiDocs(this IServiceCollection services)
    {
        services.TryAddTransient<IApiDocsProvider, ApiDocsProvider>();
        return services;
    }
}
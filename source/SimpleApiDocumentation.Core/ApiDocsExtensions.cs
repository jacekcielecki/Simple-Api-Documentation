using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleApiDocumentation.Core.Document;

namespace SimpleApiDocumentation.Core;

public static class ApiDocsExtensions
{
    /// <summary>
    /// Register the ApiDocs middleware.
    /// </summary>
    public static IApplicationBuilder UseApiDocs(this IApplicationBuilder app, Action<ApiDocsOptions>? setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(app);

        ApiDocsOptions options = new();
        setupAction?.Invoke(options);
        app.ApplicationServices.GetService<IServiceCollection>()?.AddSingleton(options);

        return app.UseMiddleware<ApiDocsMiddleware>(options);
    }

    /// <summary>
    /// Register ApiDocs services.
    /// </summary>
    public static IServiceCollection AddApiDocs(this IServiceCollection services)
    {
        services.TryAddTransient<IDocumentProvider, DocumentProvider>();
        return services;
    }
}
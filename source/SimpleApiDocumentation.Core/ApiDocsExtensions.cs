using Microsoft.AspNetCore.Builder;

namespace SimpleApiDocumentation.Core;

public static class ApiDocsExtensions
{
    /// <summary>
    /// Adds endpoint for Api documentation.
    /// </summary>
    public static IApplicationBuilder UseApiDocs(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return app;
    }
}
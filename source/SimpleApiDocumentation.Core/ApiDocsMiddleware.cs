using Microsoft.AspNetCore.Http;
using System.Text;

namespace SimpleApiDocumentation.Core;

public class ApiDocsMiddleware
{
    private readonly RequestDelegate _next;

    public ApiDocsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (!RequestingApiDocs(httpContext.Request))
        {
            await _next(httpContext);
            return;
        }

        await RespondWithIndexHtml(httpContext.Response);
    }

    private async Task RespondWithIndexHtml(HttpResponse response)
    {
        response.StatusCode = 200;
        response.ContentType = "text/html;charset=utf-8";
        var html = "<html><head><title>API Documentation</title></head><body><h1>API Documentation</h1></body></html>";

        await response.WriteAsync(html, Encoding.UTF8);
    }

    private static bool RequestingApiDocs(HttpRequest request)
    {
        var httpMethod = request.Method;

        return httpMethod == "GET" && 
               request.Path.Value.EndsWith(ApiDocsOptions.Url, StringComparison.InvariantCultureIgnoreCase);
    }
}
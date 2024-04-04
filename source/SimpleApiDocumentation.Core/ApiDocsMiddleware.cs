using Microsoft.AspNetCore.Http;
using System.Text;

namespace SimpleApiDocumentation.Core;

internal class ApiDocsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IApiDocsProvider _apiDocsProvider;

    public ApiDocsMiddleware(RequestDelegate next,
        IApiDocsProvider apiDocsProvider)
    {
        _next = next;
        _apiDocsProvider = apiDocsProvider;
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

        var html = _apiDocsProvider.GenerateDocument();

        await response.WriteAsync(html, Encoding.UTF8);
    }

    private static bool RequestingApiDocs(HttpRequest request)
    {
        var httpMethod = request.Method;

        return httpMethod == "GET" && 
               request.Path.Value.EndsWith(ApiDocsOptions.Url, StringComparison.InvariantCultureIgnoreCase);
    }
}
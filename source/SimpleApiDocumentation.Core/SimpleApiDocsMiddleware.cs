using Microsoft.AspNetCore.Http;
using SimpleApiDocumentation.Core.Document;
using System.Text;

namespace SimpleApiDocumentation.Core;

internal class SimpleApiDocsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SimpleApiDocsOptions _options;
    private readonly IDocumentProvider _documentProvider;

    public SimpleApiDocsMiddleware(
        RequestDelegate next,
        SimpleApiDocsOptions? options,
        IDocumentProvider documentProvider)
    {
        _next = next;
        _options = options ?? new SimpleApiDocsOptions();
        _documentProvider = documentProvider;
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

        var html = _documentProvider.GenerateDocument();

        await response.WriteAsync(html, Encoding.UTF8);
    }

    private bool RequestingApiDocs(HttpRequest request)
    {
        return request.Method == "GET" && 
               request.Path.Value.EndsWith(_options.RoutePrefix, StringComparison.InvariantCultureIgnoreCase);
    }
}
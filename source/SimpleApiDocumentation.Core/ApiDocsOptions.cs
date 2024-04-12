namespace SimpleApiDocumentation.Core;

public class ApiDocsOptions
{
    /// <summary>
    /// Gets or sets a route prefix for accessing the document page
    /// </summary>
    public string RoutePrefix { get; set; } = "api/documentation";

    /// <summary>
    /// Gets or sets a title for the document page
    /// </summary>
    public string DocumentTitle { get; set; } = "Simple Api Docs";
}
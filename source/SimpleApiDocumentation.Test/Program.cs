using SimpleApiDocumentation.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // For MVC controllers api documentation testing
builder.Services.AddApiDocs();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("api/user", () => "test");
app.MapDelete("api/user/{id}", (int id) => "test");
app.MapPost("api/user", (string user) => "test");
app.MapPut("api/user/{id}", () => "test");

app.UseApiDocs();
app.Run();
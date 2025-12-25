using Microsoft.AspNetCore.Mvc;
using Minimal.API;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<PeopleService>();
builder.Services.AddScoped<GuidGenerator>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapGet("/get-example", () => "Success GET")
    .WithName("GetExample")
    .WithOpenApi();

app.MapPost("/post-example", () => "Success POST")
    .WithName("PostExample")
    .WithOpenApi();

app.MapGet("ok-object", () => Results.Ok(new { Message = "API IS WORKING" }));

app.MapGet("slow-request", async () =>
{
    await Task.Delay(2000);
    Results.Ok(new { Message = "SLOW API IS WORKING" });
});

app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    options
        .WithTitle("My API")
        .WithTheme(ScalarTheme.Purple);
});

app.MapGet("get", () => "this is a get");
app.MapPost("post", () => "this is a post");
app.MapPut("put", () => "this is a put");
app.MapDelete("delete", () => "this is a delete");

app.MapMethods("options-or-head", new[] { "options", "head" }, () => "options or head");

var handler = () => "its a var";
app.MapGet("handler", handler);


app.MapGet("fromClass", Example.SomeMethod);

app.MapGet("get-params/{age:int}", (int age) =>
{
    return $"Age provided was {age}";
});

app.MapGet("cars/{carId:regex(^[a-z0-9]+$)}", (string carId) =>
{
    return $"Car Id provided was : {carId}";
});


app.MapGet("books/{isbn:length(13)}", (string isbn) =>
{
    return $"Isbn provided was : {isbn}";
});


app.MapGet("people/search", (string? searchTerm, PeopleService peopleService) =>
{
    if (searchTerm is null) return Results.NotFound();
    var result = peopleService.Search(searchTerm);
    return Results.Ok(result);
});

app.MapGet("mix/{routeParams}", ([FromRoute] string routeParams, [FromQuery(Name = "Value")] int queryParams, [FromServices] GuidGenerator guidGenerator) =>
{
    return $"{routeParams} {queryParams} {guidGenerator.NewGuid}";
});
app.MapGet("httpContext", async context =>
{
    await context.Response.WriteAsync("Hello from the httpContext");
});

app.MapGet("http", async (HttpRequest request,HttpResponse response) =>
{
    var queryString = request.QueryString;
    await response.WriteAsync("Hello from the http "+queryString);
});


app.MapPost("people", (Person person) =>
{
    return Results.Ok(person);
});


app.MapGet("get-point", (MapPoint point) =>
{
    return Results.Ok(point);
});


app.MapGet("simple-string", () => "Hello world!");
app.MapGet("json-raw-obj", () => new {Message = "Hello world!" });
app.MapGet("ok-obj", () => Results.Ok(new { Message = "Hello world!" }));
app.MapGet("json-obj", () => Results.Json(new { Message = "Hello world!" }));
app.MapGet("text-string", () => Results.Text("Hello world!"));
app.MapGet("redirect", () => Results.Redirect("https://google.com"));


app.MapGet("logging", (ILogger<Program> logger) =>
{
    logger.LogInformation("log test");
    return Results.Ok();
});

app.MapControllers();
app.Run();
using FluentValidation;
using Library.API.Context;
using Library.API.Models;
using Library.API.Services;
using Library.API.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.custom.json", true,true);
builder.Services.AddAuthentication().AddJwtBearer( opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = "Umut Tosun",
        ValidAudience = "Umut Tosun",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fe1fbfb84daa049f153a9af62d88a7e23d7381050ad63f629d6c6b557ee7c108"))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<JwtProvider>();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseInMemoryDatabase("myDb");
});

var app = builder.Build();

app.MapGet("/health", () => "health!").RequireAuthorization();

app.MapOpenApi();
app.MapScalarApiReference();



app.MapPost("books", async (Book book, IBookService bookService,CancellationToken cancellationToken) =>
{
    BookValidator validator = new();

    FluentValidation.Results.ValidationResult validationResult = validator.Validate(book);
    if (!validationResult.IsValid)
    {
      return  Results.BadRequest(validationResult.Errors.Select(s => s.ErrorMessage));
    }

   var result = await bookService.CreateAsync(book,cancellationToken);
    if (!result) return Results.BadRequest("Something went wrong!");
   return Results.Ok(result);

});

app.MapGet("books", async (IBookService bookService,CancellationToken cancellationToken) =>
{
    var books = await bookService.GetAllAsync(cancellationToken);
    return Results.Ok(books);
});

app.MapGet("books/{isbn}", async (string isbn, IBookService bookService, CancellationToken cancellationToken) =>
{
    Book? book = await bookService.GetByIsbnAsync(isbn, cancellationToken);
    return Results.Ok(book);
});

app.MapGet("booksByTitle/{title}", async (string title, IBookService bookService, CancellationToken cancellationToken) =>
{
    IEnumerable<Book>? books = await bookService.SearchByTitleAsync(title, cancellationToken);
    return Results.Ok(books);
});

app.MapPut("books", async (Book book, IBookService bookService, CancellationToken cancellationToken) =>
{
    BookValidator validator = new();

    FluentValidation.Results.ValidationResult validationResult = validator.Validate(book);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors.Select(s => s.ErrorMessage));
    }

    var result = await bookService.UpdateAsync(book, cancellationToken);
    if (!result) return Results.BadRequest("Something went wrong!");
    return Results.Ok(result);

});

//app.MapDelete("books/{isbn}", async (string isbn,BookService bookService, CancellationToken cancellationToken) =>
//{
//    var result = await bookService.DeleteAsync(isbn, cancellationToken);
//    if (!result) return Results.BadRequest("Something went wrong!");
//    return Results.Ok("Success!");
//});
app.MapGet("login", (JwtProvider jwtProvider) =>
{
    return Results.Ok(new { Token = jwtProvider.CreateToken() });
});
app.Run();

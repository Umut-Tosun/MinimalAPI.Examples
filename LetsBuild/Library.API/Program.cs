using FluentValidation;
using Library.API.Context;
using Library.API.Models;
using Library.API.Services;
using Library.API.Validators;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseInMemoryDatabase("myDb");
});

var app = builder.Build();

app.MapGet("/health", () => "health!");

app.MapOpenApi();
app.MapScalarApiReference();



app.MapPost("books", async (Book book, IBookService bookService) =>
{
    BookValidator validator = new();

    FluentValidation.Results.ValidationResult validationResult = validator.Validate(book);
    if (!validationResult.IsValid)
    {
      return  Results.BadRequest(validationResult.Errors.Select(s => s.ErrorMessage));
    }

   var result = await bookService.CreateAsync(book);
    if (!result) return Results.BadRequest("Something went wrong!");
   return Results.Ok(result);

});



app.Run();

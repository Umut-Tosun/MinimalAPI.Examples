using FluentValidation;
using Library.API.Context;
using Library.API.Endpoints;
using Library.API.Models;
using Library.API.Services;
using Library.API.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using static System.Reflection.Metadata.BlobBuilder;

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
app.UseBookEndpoints();

app.Run();

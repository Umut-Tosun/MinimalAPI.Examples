using Library.API.Models;
using Library.API.Services;
using Library.API.Validators;

namespace Library.API.Endpoints
{
    public static class BookEndpoints
    {
        public static void UseBookEndpoints(this IEndpointRouteBuilder app)
        {


            app.MapPost("books", async (Book book, IBookService bookService, CancellationToken cancellationToken) =>
            {
                BookValidator validator = new();

                FluentValidation.Results.ValidationResult validationResult = validator.Validate(book);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors.Select(s => s.ErrorMessage));
                }

                var result = await bookService.CreateAsync(book, cancellationToken);
                if (!result) return Results.BadRequest("Something went wrong!");
                //  return Results.Ok(result);
                //    return Results.Created($"/books/{book?.Isbn}", book);
                return Results.CreatedAtRoute("GetBookByIsbn", new { isbn = book.Isbn });
            }).WithTags("books");

            app.MapGet("books", async (IBookService bookService, CancellationToken cancellationToken) =>
            {
                var books = await bookService.GetAllAsync(cancellationToken);
                return Results.Ok(books);
            }).WithTags("books");

            app.MapGet("books/{isbn}", async (string isbn, IBookService bookService, CancellationToken cancellationToken) =>
            {
                Book? book = await bookService.GetByIsbnAsync(isbn, cancellationToken);

                return Results.Ok(book);
            }).WithName("GetBookByIsbn").WithTags("books");

            app.MapGet("booksByTitle/{title}", async (string title, IBookService bookService, CancellationToken cancellationToken) =>
            {
                IEnumerable<Book>? books = await bookService.SearchByTitleAsync(title, cancellationToken);
                return Results.Ok(books);
            }).WithTags("books");

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

            }).WithTags("books");

            app.MapDelete("books/{isbn}", async (string isbn, IBookService bookService, CancellationToken cancellationToken) =>
            {
                var result = await bookService.DeleteAsync(isbn, cancellationToken);
                if (!result) return Results.BadRequest("Something went wrong!");
                return Results.Ok("Success!");
            }).WithTags("books");
            app.MapGet("login", (JwtProvider jwtProvider) =>
            {
                return Results.Ok(new { Token = jwtProvider.CreateToken() });
            }).WithTags("Auth");
        }
    }
}

using Library.API.Context;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Services
{
    public sealed class BookService(ApplicationDbContext applicationDbContext) : IBookService
    {
        public async Task<bool> CreateAsync(Book book, CancellationToken cancellationToken = default)
        {
           await applicationDbContext.books.AddAsync(book, cancellationToken); 
           return await applicationDbContext.SaveChangesAsync() > 0 ;
        }

        public async Task<bool> DeleteAsync(string isbn, CancellationToken cancellationToken = default)
        {
            Book? book = await applicationDbContext.books.FindAsync(isbn, cancellationToken);
            if (book is null) return false;

            applicationDbContext.books.Remove(book);
            return await applicationDbContext.SaveChangesAsync() > 0;
        }
      

        public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await applicationDbContext.books.ToListAsync(cancellationToken);
        }

        public async Task<Book?> GetByIsbnAsync(string isbn, CancellationToken cancellationToken = default)
        {
            return await applicationDbContext.books.FindAsync(isbn, cancellationToken);
        }

        public async Task<IEnumerable<Book>> SearchByTitleAsync(string title, CancellationToken cancellationToken = default)
        {
            return await applicationDbContext.books.Where(x=>x.Title.Contains(title)).ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateAsync(Book book, CancellationToken cancellationToken = default)
        {
            applicationDbContext.Update(book);
            return await applicationDbContext.SaveChangesAsync() > 0;

        }
    }
}

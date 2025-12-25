using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Book> books { get; set; }

       
    }
}

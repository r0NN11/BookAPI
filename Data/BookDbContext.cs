using BookAPI.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
    }
}

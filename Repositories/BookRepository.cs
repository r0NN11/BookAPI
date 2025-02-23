using BookAPI.Data;
using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;
        public BookRepository(BookDbContext context) => _context = context;

        public async Task AddBooksAsync(List<Book> books)
        {
            _context.Books.AddRange(books);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByPopularityAsync(int page, int pageSize)
        {
            return await _context.Books
                .Where(b => !b.IsDeleted)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<IEnumerable<Book>> GetBooksByTitleAsync(string title)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(title) && !b.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByIdsAsync(int[] ids)
        {
            return await _context.Books
                .Where(b => ids.Contains(b.Id) && !b.IsDeleted)
                .ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

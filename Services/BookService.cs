using BookAPI.Models;
using BookAPI.Repositories;

namespace BookAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;

        public BookService(IBookRepository repository) => _repository = repository;

        public async Task AddBooksAsync(List<Book> books)
        {
            foreach (var book in books)
            {
                ValidateBook(book);
            }

            await _repository.AddBooksAsync(books);
        }
        public async Task UpdateBookAsync(Book book)
        {
            ValidateBook(book, false);

            await _repository.UpdateBookAsync(book);
        }

        public async Task SoftDeleteBooksAsync(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentException("No book IDs provided.");
            }

            var books = await _repository.GetBooksByIdsAsync(ids);

            if (books.Count() != ids.Length)
            {
                var notFoundIds = ids.Except(books.Select(b => b.Id)).ToArray();
                throw new KeyNotFoundException($"Books with IDs {string.Join(", ", notFoundIds)} not found.");
            }

            foreach (var book in books)
            {
                book.IsDeleted = true;
            }

            await _repository.SaveChangesAsync();
        }
        public async Task<IEnumerable<Book>> GetBooksByPopularityAsync(int page, int pageSize)
        {
            var books = await _repository.GetBooksByPopularityAsync(page, pageSize);
            var sortedBooks = books
                .OrderByDescending(b => (b.ViewsCount * 0.5) + ((DateTime.UtcNow.Year - b.PublicationYear) * 2))
                .ToList();

            return sortedBooks;
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _repository.GetBookByIdAsync(id);
            if (book == null)
                throw new KeyNotFoundException($"Book with ID {id} not found or is marked as deleted.");
            book.ViewsCount++;
            await _repository.SaveChangesAsync();
            return book;
        }
        public async Task<IEnumerable<Book>> GetBooksByTitleAsync(string title)
        {
            var books = await _repository.GetBooksByTitleAsync(title);
            if (books == null||!books.Any())
                throw new KeyNotFoundException($"Books with title '{title}' not found or is marked as deleted.");
            foreach (var book in books)
            {
                book.ViewsCount++;
            }
            await _repository.SaveChangesAsync();
            return books;
        }

        private void ValidateBook(Book book, bool withExistanceCheck = true)
        {
            if (string.IsNullOrEmpty(book.Title))
                throw new ArgumentException("Title cannot be empty.");
            if (book.PublicationYear <= 0)
                throw new ArgumentException("PublicationYear must be a positive number.");
            if (book.ViewsCount < 0)
                throw new ArgumentException("ViewsCount cannot be negative.");
            if(withExistanceCheck)
            {
                var existingBooks = _repository.GetBooksByTitleAsync(book.Title).Result;
                if (existingBooks != null && existingBooks.Any(b => b.AuthorName == book.AuthorName))
                {
                    throw new InvalidOperationException($"A book with the title '{book.Title}' by '{book.AuthorName}' already exists.");
                }
            }
        }
    }
}

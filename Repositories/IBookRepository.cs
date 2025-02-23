using BookAPI.Models;

namespace BookAPI.Repositories
{
    public interface IBookRepository
    {
        Task AddBooksAsync(List<Book> books);
        Task UpdateBookAsync(Book book);
        Task<IEnumerable<Book>> GetBooksByPopularityAsync(int page, int pageSize);
        Task<Book?> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetBooksByIdsAsync(int[] ids);
        Task<IEnumerable<Book>> GetBooksByTitleAsync(string title);
        Task SaveChangesAsync();
    }
}

using BookAPI.Models;

namespace BookAPI.Services
{
    public interface IBookService
    {
        Task AddBooksAsync(List<Book> books);
        Task UpdateBookAsync(Book book);
        Task SoftDeleteBooksAsync(int[] ids);
        Task<IEnumerable<Book>> GetBooksByPopularityAsync(int page, int pageSize);
        Task<Book> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetBooksByTitleAsync(string title);
    }
}

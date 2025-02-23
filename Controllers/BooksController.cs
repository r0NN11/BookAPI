using AutoMapper;
using BookAPI.Models;
using BookAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookService _service;

        public BooksController(IMapper mapper, IBookService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddBooks([FromBody] List<UpdateBookDto> updateBookDtos)
        {
            try
            {
                if (updateBookDtos == null || !updateBookDtos.Any())
                {
                    return BadRequest("No books provided.");
                }

                var books = _mapper.Map<List<Book>>(updateBookDtos);

                await _service.AddBooksAsync(books);

                return Ok("Books added successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto)
        {
            try
            {
                var book = _mapper.Map<Book>(updateBookDto);

                await _service.UpdateBookAsync(book);
                return Ok("Book updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> SoftDeleteBooks([FromBody] int[] ids)
        {
            try
            {
                if (ids == null || !ids.Any())
                {
                    return BadRequest("No book IDs provided.");
                }

                await _service.SoftDeleteBooksAsync(ids);
                return Ok("Books deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBooksByPopularity([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var books = await _service.GetBooksByPopularityAsync(page, pageSize);

                var bookTitles = books.Select(b => b.Title).ToList();

                return Ok(bookTitles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var book = await _service.GetBookByIdAsync(id);
                book.ViewsCount++;
                await _service.UpdateBookAsync(book);
                var bookDto = _mapper.Map<BookDto>(book);
                return Ok(bookDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("title")]
        public async Task<IActionResult> GetBooksByTitle(string title)
        {
            try
            {
                var books = await _service.GetBooksByTitleAsync(title);

                if (books == null || !books.Any())
                {
                    return NotFound($"No books found with title '{title}'.");
                }

                foreach (var book in books)
                {
                    book.ViewsCount++;
                    await _service.UpdateBookAsync(book);
                }

                var bookDtos = _mapper.Map<List<BookDto>>(books);

                return Ok(bookDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}

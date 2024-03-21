using DataAccess.Entities;
using DataAccess.Repositories;
using LibraryConsoleApp;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookControler : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAllBooks()
		{
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			List<Book> books = await bookRepository.ReadAllAsync();
			return Ok(books);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetBook(long id)
		{
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			Book book = await bookRepository.ReadAsync(id);
			return Ok(book);
		}
		[HttpPost]
		public async Task<IActionResult> CreateBook(Book book)
		{
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			await bookRepository.CreateAsync(book);
			return Ok(book);
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBook(long id, string title, string genre, long authorId)
		{
			var book = new Book()
			{
				Id = id,
				Title = title,
				Genre = genre,
				AuthorId = authorId
			};
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			await bookRepository.UpdateAsync(book);
			return Ok(book);
		}
		[HttpPut("{id}/json")]
		public async Task<IActionResult> UpdateBook(Book book)
		{
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			await bookRepository.UpdateAsync(book);
			return Ok(book);
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBook(long id)
		{
			var dbcf = new LibraryDbContextFactory();
			var context = dbcf.CreateDbContext([]);
			var bookRepository = new BookRepository(context);
			await bookRepository.DeleteAsync(id);
			return NoContent();
		}
	}
}

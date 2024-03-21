using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
	public class BookRepository
	{

		private readonly LibraryDbContext _context;

		public BookRepository(LibraryDbContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(Book book)
		{
			_context.Books.Add(book);
			await _context.SaveChangesAsync();
		}
		public async Task UpdateAsync(Book book)
		{
			_context.Books.Update(book);
			await _context.SaveChangesAsync();
		}
		public async Task DeleteAsync(long id)
		{
			var deleteBook = await _context.Books.FindAsync(id);
			_context.Books.Remove(deleteBook);
			await _context.SaveChangesAsync();
		}
		public async Task<List<Book>> ReadAllAsync()
		{
			return await _context.Books
		   .Select(b => new Book
		   {
			   Id = b.Id,
			   Title = b.Title,
			   Genre = b.Genre,
			   AuthorId = b.AuthorId,
			   Author = new Author
			   {
				   Id = b.Author.Id,
				   Name = b.Author.Name,
				   Surname = b.Author.Surname
			   }
		   })
		   .ToListAsync();
		}
		public async Task<Book> ReadAsync(long id)
		{
			return await _context.Books
				 .Select(b => new Book
				 {
					 Id = b.Id,
					 Title = b.Title,
					 Genre = b.Genre,
					 AuthorId = b.AuthorId,
					 Author = new Author
					 {
						 Id = b.Author.Id,
						 Name = b.Author.Name,
						 Surname = b.Author.Surname
					 }
				 })
				 .AsNoTracking()
				.FirstOrDefaultAsync(a => a.Id == id);
		}
		public async Task<List<Book>> FindByTextAsync(string text)
		{
			text = text.Trim().ToLower();

			return await _context.Books
				.Include(b => b.Author)
				.Where(b => b.Title.Contains(text) || b.Genre.Contains(text))
				.ToListAsync();
		}

	}
}
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
	public class AuthorRepository
	{
		private readonly LibraryDbContext _context;

		public AuthorRepository(LibraryDbContext context)
		{
			_context = context;
		}

		public async Task<long> CreateAsync(Author author)
		{
			_context.Authors.Add(author);
			await _context.SaveChangesAsync();
			return author.Id;
		}

		public async Task UpdateAsync(Author author)
		{
			var authorEdit = await _context.Authors.FindAsync(author.Id);
			_context.Authors.Entry(authorEdit).CurrentValues.SetValues(author);
			await _context.SaveChangesAsync();
		}
		public async Task DeleteAsync(long id)
		{
			var deleteAuthor = await _context.Authors.FindAsync(id);
			_context.Authors.Remove(deleteAuthor);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Author>> ReadAllAsync()
		{
			return await _context.Authors
		 .Select(a => new Author
		 {
			 Id = a.Id,
			 Name = a.Name,
			 Surname = a.Surname,

			 Books = a.Books.Select(b => new Book { Id = b.Id, Title = b.Title }).ToList()
		 })
		 .ToListAsync();
		}
		public async Task<Author> ReadAsync(long id)
		{
			return await _context.Authors
				 .Select(a => new Author
				 {
					 Id = a.Id,
					 Name = a.Name,
					 Surname = a.Surname,
					 Books = a.Books.Select(b => new Book { Id = b.Id, Title = b.Title, Genre = b.Genre }).ToList()
				 })
				.FirstOrDefaultAsync(a => a.Id == id);
		}
		public async Task<List<Author>> FindByNameAsync(string name)
		{
			name = name.Trim().ToLower();

			return await _context.Authors
				.Include(a => a.Books)
				.Where(a => a.Name.Contains(name) || a.Surname.Contains(name))
				.ToListAsync();
		}
	}
}

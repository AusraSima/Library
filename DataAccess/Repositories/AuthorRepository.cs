using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            var newAuthor = await ReadAsync(author.Id);
            _context.Authors.Entry(newAuthor).CurrentValues.SetValues(author);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Author author)
        {
            var deleteAuthor = await ReadAsync(author.Id);
            _context.Authors.Remove(deleteAuthor);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Author>> ReadAllAsync()
        {
            return await _context.Authors.Include(a => a.Books).ToListAsync();
        }
        public async Task<Author> ReadAsync(long id)
        {
            return await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}

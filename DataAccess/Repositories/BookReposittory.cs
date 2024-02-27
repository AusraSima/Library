using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

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
            var newBook = await ReadAsync(book.Id);
            _context.Books.Entry(newBook).CurrentValues.SetValues(book);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Book book)
        {
            var deleteBook = await ReadAsync(book.Id);
            _context.Books.Remove(deleteBook);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Book>> ReadAllAsync()
        {
            return await _context.Books.Include(b => b.Author).ToListAsync();
        }
        public async Task<Book> ReadAsync(long id)
        {
            return await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
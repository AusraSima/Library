using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
	public class LibraryDbContext : DbContext
	{
		public LibraryDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Book>()
				.HasOne(a => a.Author)
				.WithMany(b => b.Books)
				.HasForeignKey(b => b.AuthorId);
		}
	}
}

using DataAccess.Repositories;
using LibraryConsoleApp.Handlers;

namespace LibraryConsoleApp;

internal class Program
{
	private static AuthorRepository authorRepository;
	private static BookRepository bookRepository;
	static async Task Main(string[] args)
	{
		var dbcf = new LibraryDbContextFactory();
		var context = dbcf.CreateDbContext(args);

		authorRepository = new AuthorRepository(context);
		bookRepository = new BookRepository(context);

		AuthorsHandler authorsHandler = new AuthorsHandler(authorRepository, null); //booksHandler);
		BooksHandler booksHandler = new BooksHandler(bookRepository, authorsHandler);

		ApplicationUI app = new ApplicationUI(authorsHandler, booksHandler);

		await app.RunAsync();
	}
}

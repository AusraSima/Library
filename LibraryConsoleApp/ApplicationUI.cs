using DataAccess.Repositories;
using LibraryConsoleApp.Handlers;

namespace LibraryConsoleApp
{
    public class ApplicationUI
    {
        private readonly AuthorsHandler _authorsHandler;
        private readonly BooksHandler _booksHandler;

        public ApplicationUI()
        {
            LibraryDbContextFactory dbcf = new LibraryDbContextFactory();
            var context = new LibraryDbContextFactory().CreateDbContext([]);

            AuthorRepository authorRepository = new AuthorRepository(context);
            BookRepository bookRepository = new BookRepository(context);

            _authorsHandler = new AuthorsHandler(authorRepository, _booksHandler);
            _booksHandler = new BooksHandler(bookRepository, _authorsHandler);
        }

        public ApplicationUI(AuthorsHandler authorsHandler, BooksHandler booksHandler)
        {
            _authorsHandler = authorsHandler;
            _booksHandler = booksHandler;
        }
        public ApplicationUI(BooksHandler booksHandler)
        {
            _booksHandler = booksHandler;
        }
        public async Task RunAsync()
        {
            await Console.Out.WriteLineAsync("Welcome to Library!");
            await Console.Out.WriteLineAsync();

            while (true)
            {
                await Console.Out.WriteLineAsync("Select from the menu: ");
                MainMenu();
                int input = Convert.ToInt32(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        await _authorsHandler.HandleAsync();
                        break;
                    case 2:
                        await _booksHandler.HandleAsync();
                        break;
                    case 3:
                        Environment.Exit(1);
                        break;
                }
            }
        }

        public void MainMenu()
        {
            Console.WriteLine("1 - Go to Authors page");
            Console.WriteLine("2 - Go to Books page");
            Console.WriteLine("3 - Exit program");
            Console.WriteLine();
        }
    }
}

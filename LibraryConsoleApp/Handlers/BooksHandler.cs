using DataAccess.Entities;
using DataAccess.Repositories;

namespace LibraryConsoleApp.Handlers
{
	public class BooksHandler
	{
		private readonly BookRepository _bookRepository;
		private readonly AuthorsHandler _authorsHandler;
		public BooksHandler(BookRepository bookRepository, AuthorsHandler authorsHandler)
		{
			_bookRepository = bookRepository;
			_authorsHandler = authorsHandler;
		}
		public async Task HandleAsync()
		{
			await Console.Out.WriteLineAsync("Welcome to Books page");

			bool continueOrNot = true;
			while (continueOrNot)
			{
				BooksPageMenu();

				await Console.Out.WriteLineAsync("What do you want to do? Enter a number from menu list: ");
				int input = Convert.ToInt32(Console.ReadLine());

				switch (input)
				{
					case 1:
						PrintBooks(await GetBooks());
						break;
					case 2:
						PrintOneBook(await GetOneBook());
						break;
					case 3:
						await CreateBook();
						break;
					case 4:
						await UpdateBook();
						break;
					case 5:
						await DeleteBook();
						break;
					case 6:
						await FindBookByText();
						break;
					case 7:
						return;
				}
				continueOrNot = continueInBooksPage();
			}
		}
		public static void BooksPageMenu()
		{
			Console.WriteLine();
			Console.WriteLine("1 - Get list of books");
			Console.WriteLine("2 - Get info on the book");
			Console.WriteLine("3 - Create new book");
			Console.WriteLine("4 - Update book");
			Console.WriteLine("5 - Delete book");
			Console.WriteLine("6 - Find book by title or text");
			Console.WriteLine("7 - Exit Books page");
			Console.WriteLine();
		}
		private async Task<List<Book>> GetBooks()
		{
			return await _bookRepository.ReadAllAsync();
		}

		private void PrintBooks(List<Book> books)
		{
			Console.WriteLine("List of books:");
			Console.WriteLine("-------------------------------------------------");
			Console.WriteLine($"{"ID",-4} {"AUTHOR",-25} {"TITLE",-50} {"GENRE",-20}");
			foreach (var book in books)
			{
				Console.WriteLine($"{book.Id,-4} {book.Author.Name,-10} {book.Author.Surname,-15} {book.Title,-50} {book.Genre,-20}");
			}
			Console.WriteLine("-------------------------------------------------");
			Console.WriteLine();
		}
		private async Task<Book> GetOneBook()
		{
			await Console.Out.WriteLineAsync("Enter ID of the book you want to review: ");
			long id = Convert.ToInt64(Console.ReadLine());

			Book bookToSee = await _bookRepository.ReadAsync(id);
			if (bookToSee == null)
			{
				await Console.Out.WriteLineAsync($"A book with ID {id} not found.");
				return new Book();
			}
			return bookToSee;
		}
		private void PrintOneBook(Book book)
		{
			if (book.Id != 0)
			{
				Console.WriteLine($"{book.Id,-4} {book.Author.Name,-10} {book.Author.Surname,-15} {book.Title,-50} {book.Genre,-20}");
			}
			Console.WriteLine();
		}


		private async Task CreateBook()
		{
			await Console.Out.WriteLineAsync("Enter book title: ");
			string title = Console.ReadLine();

			await Console.Out.WriteLineAsync("Enter book genre: ");
			string genre = Console.ReadLine();

			await Console.Out.WriteLineAsync("Enter book's author: ");
			await Console.Out.WriteLineAsync("Select what do you want to do:");
			await Console.Out.WriteLineAsync("1 - Select from the presented list of authors");
			await Console.Out.WriteLineAsync("2 - Enter a new author");
			await Console.Out.WriteLineAsync("3 - Leave new book creation");

			int input = Convert.ToInt32(Console.ReadLine());

			Author aut = new();
			switch (input)
			{
				case 1:
					await Console.Out.WriteLineAsync("Enter book author's ID from the presented list of authors:");
					_authorsHandler.PrintAuthors(await _authorsHandler.GetAuthors());

					aut = await _authorsHandler.GetOneAuthor(Convert.ToInt64(Console.ReadLine()));
					break;
				case 2:
					await Console.Out.WriteLineAsync("Enter author's name: ");
					string authorName = Console.ReadLine();
					await Console.Out.WriteLineAsync("Enter author's surname: ");
					string authorSurname = Console.ReadLine();

					aut = new Author
					{
						Name = authorName,
						Surname = authorSurname
					};

					long authorCreated = _authorsHandler.CreateAuthor(aut).Result;
					break;
				case 3:
					return;
			}
			var book = new Book()
			{
				Title = title,
				Genre = genre,
				AuthorId = aut.Id
			};
			await _bookRepository.CreateAsync(book);
			await Console.Out.WriteLineAsync($"New book {book.Title} created successfully.");
		}

		private async Task UpdateBook()
		{
			await Console.Out.WriteLineAsync("Enter ID of the book you want to update: ");

			int id;
			if (!int.TryParse(Console.ReadLine(), out id))
			{
				Console.WriteLine("Invalid input. Please enter a valid integer ID.");
				return;
			}

			Book bookToUpdate = await _bookRepository.ReadAsync(id);

			if (bookToUpdate == null)
			{
				Console.WriteLine($"A book with ID {id} not found.");
				return;
			}
			bool isBookEdited = true;
			while (isBookEdited)
			{
				await Console.Out.WriteLineAsync("What do you want to update? Select from the list: ");
				UpdateBookMenu();
				int input = Convert.ToInt32(Console.ReadLine());
				switch (input)
				{
					case 1:
						await Console.Out.WriteLineAsync("Enter updated book title: ");
						string title = Console.ReadLine();
						bookToUpdate.Title = title;
						break;
					case 2:
						await Console.Out.WriteLineAsync("Enter updated book genre: ");
						string genre = Console.ReadLine();
						bookToUpdate.Genre = genre;
						break;
					case 3:
						await Console.Out.WriteLineAsync("Enter updated book author's ID from the presented list of authors: (under development)");
						_authorsHandler.PrintAuthors(await _authorsHandler.GetAuthors());
						long authorID = Convert.ToInt64(Console.ReadLine());
						bookToUpdate.AuthorId = authorID;
						break;
					case 4:
						isBookEdited = false;
						break;
				}
			}

			await _bookRepository.UpdateAsync(bookToUpdate);
			await Console.Out.WriteLineAsync("Book data updated successfully");
			Console.WriteLine();
		}
		public static void UpdateBookMenu()
		{
			Console.WriteLine("Select what should be updated: ");
			Console.WriteLine(" 1 - Update book's title");
			Console.WriteLine(" 2 - Update book's genre");
			Console.WriteLine(" 3 - Update book's author");
			Console.WriteLine(" 4 - Save update");
			Console.WriteLine();
		}
		private async Task DeleteBook()
		{
			await Console.Out.WriteLineAsync("Enter ID of the book you want to delete: ");
			long id = Convert.ToInt64(Console.ReadLine());
			Book bookToDelete = await _bookRepository.ReadAsync(id);

			if (bookToDelete == null)
			{
				Console.WriteLine($"A book with ID {id} not found.");
				return;
			}

			await _bookRepository.DeleteAsync(bookToDelete.Id);
			await Console.Out.WriteLineAsync($"The book ID {bookToDelete.Id}, title {bookToDelete.Title} deleted successfully.");
			Console.WriteLine();
		}
		public bool continueInBooksPage()
		{
			string continueKey = "y";
			while (true)
			{
				Console.Out.WriteLine("Do you want to continue work in Books page?(y/n)");
				continueKey = Console.ReadLine().ToLower();
				if (continueKey.Equals("y"))
				{
					return true;
				}

				if (continueKey.Equals("n"))
				{
					return false;
				}
			}
		}
		public async Task FindBookByText()
		{
			Console.WriteLine("Enter title or text fragment: ");
			string bookText = Console.ReadLine();
			List<Book> books = new();
			books = await _bookRepository.FindByTextAsync(bookText);

			if (books != null)
			{
				foreach (Book book in books)
				{
					await Console.Out.WriteLineAsync($"Book found: {book.Title}, {book.Genre} by {book.Author.Name} {book.Author.Surname}.");
				}
			}
			else
			{
				Console.WriteLine("Book not found.");
			}
		}
	}
}

using DataAccess.Entities;
using DataAccess.Repositories;

namespace LibraryConsoleApp.Handlers
{
	public class AuthorsHandler
	{
		private readonly AuthorRepository _authorRepository;
		private readonly BooksHandler _booksHandler;

		public AuthorsHandler(AuthorRepository authorRepository, BooksHandler booksHandler)
		{
			_authorRepository = authorRepository;
			_booksHandler = booksHandler;
		}
		public async Task HandleAsync()
		{
			await Console.Out.WriteLineAsync("Welcome to Authors page");

			bool continueOrNot = true;
			while (continueOrNot)
			{
				AuthorsPageMenu();
				await Console.Out.WriteLineAsync("What do you want to do? Enter a number from menu list: ");
				int input = Convert.ToInt32(Console.ReadLine());

				switch (input)
				{
					case 1:
						PrintAuthors(await GetAuthors());
						break;
					case 2:
						PrintOneAuthor(await GetOneAuthor());
						break;
					case 3:
						await CreateAuthor();
						break;
					case 4:
						await UpdateAuthor();
						break;
					case 5:
						await DeleteAuthor();
						break;
					case 6:
						await FindByName();
						break;
					case 7:
						return;
				}
				continueOrNot = continueInAuthorsPage();
			}
		}
		public static void AuthorsPageMenu()
		{
			Console.WriteLine();
			Console.WriteLine("1 - Get list of authors");
			Console.WriteLine("2 - Get info about the author");
			Console.WriteLine("3 - Create a new author");
			Console.WriteLine("4 - Update author's info");
			Console.WriteLine("5 - Delete author");
			Console.WriteLine("6 - Find author by name");
			Console.WriteLine("7 - Exit Authors page");
			Console.WriteLine();
		}
		public async Task<List<Author>> GetAuthors()
		{
			return await _authorRepository.ReadAllAsync();
		}

		public void PrintAuthors(List<Author> authors)
		{
			Console.WriteLine("List of authors:");
			Console.WriteLine();
			Console.WriteLine($"{"ID",-4}   {"NAME",-10}   {"SURNAME",-15}   {"WROTE BOOKS"}");
			foreach (var author in authors)
			{
				Console.WriteLine($"{author.Id,-4}   {author.Name,-10}   {author.Surname,-15}   {author.Books.Count}");
			}
			Console.WriteLine();
		}
		public async Task<Author> GetOneAuthor(long id)
		{
			Author authorToSee = await _authorRepository.ReadAsync(id);
			if (authorToSee == null)
			{
				await Console.Out.WriteLineAsync($"An author with ID {id} not found.");
				return new Author();
			}
			return await _authorRepository.ReadAsync(authorToSee.Id);

		}
		public async Task<Author> GetOneAuthor()
		{
			await Console.Out.WriteLineAsync("Enter VALID ID of the author: ");
			long id = Convert.ToInt64(Console.ReadLine());

			return await GetOneAuthor(id);
		}
		private async void PrintOneAuthor(Author author)
		{
			Console.WriteLine($"The author ID {author.Id}, {author.Name} {author.Surname} wrote {author.Books.Count} book(s):");
			foreach (var book in author.Books)
			{
				await Console.Out.WriteLineAsync($"* {book.Title},  zanras : {book.Genre}");
			}
			Console.WriteLine();
		}
		public async Task CreateAuthor()
		{
			await Console.Out.WriteLineAsync("Enter author name: ");
			string name = Console.ReadLine();
			await Console.Out.WriteLineAsync("Enter author surname: ");
			string surname = Console.ReadLine();

			var author = new Author()
			{
				Name = name,
				Surname = surname
			};
			await _authorRepository.CreateAsync(author);
			await Console.Out.WriteLineAsync($"New author {author.Name} {author.Surname} created successfully.");
			Console.WriteLine();
		}
		public async Task<long> CreateAuthor(Author author)
		{
			try
			{
				return await _authorRepository.CreateAsync(author);
			}
			catch (Exception)
			{
				return 0;
			}
		}

		private async Task UpdateAuthor()
		{
			await Console.Out.WriteLineAsync("Enter ID of the author you want to update: ");
			int id;
			if (!int.TryParse(Console.ReadLine(), out id))
			{
				Console.WriteLine("Invalid input. Please enter a valid integer ID.");
				return;
			}

			Author authorToUpdate = await _authorRepository.ReadAsync(id);
			if (authorToUpdate == null)
			{
				Console.WriteLine($"An author with ID {id} not found.");
				return;
			}
			bool isAuthorEdited = true;
			while (isAuthorEdited)
			{
				await Console.Out.WriteLineAsync("What do you want to update? Select from the list: ");
				AuthorDataUpdateActions();
				int input = Convert.ToInt32(Console.ReadLine());
				switch (input)
				{
					case 1:
						await Console.Out.WriteLineAsync("Enter updated author name: ");
						string name = Console.ReadLine();
						authorToUpdate.Name = name;
						break;
					case 2:
						await Console.Out.WriteLineAsync("Enter updated author surname: ");
						string surname = Console.ReadLine();
						authorToUpdate.Surname = surname;
						break;
					case 3:
						isAuthorEdited = false;
						break;
				}
			}
			await _authorRepository.UpdateAsync(authorToUpdate);
			await Console.Out.WriteLineAsync($"Data of the author ID {authorToUpdate.Id}, {authorToUpdate.Name} {authorToUpdate.Surname} updated successfully.");
			Console.WriteLine();
		}
		public static void AuthorDataUpdateActions()
		{
			Console.WriteLine("Select what should be updated: ");
			Console.WriteLine(" 1 - Update author's name");
			Console.WriteLine(" 2 - Update author's surname");
			Console.WriteLine(" 3 - Save update");
			Console.WriteLine();
		}
		private async Task DeleteAuthor()
		{
			await Console.Out.WriteLineAsync("Enter ID of the author you want to delete: ");
			long id = Convert.ToInt64(Console.ReadLine());
			Author authorToDelete = await _authorRepository.ReadAsync(id);
			if (authorToDelete == null)
			{
				Console.WriteLine($"An author with ID {id} not found.");
				return;
			}

			if (authorToDelete.Books.Count() > 0)
			{
				Console.WriteLine($"The author ID {authorToDelete.Id} cannot be deleted because he/she has books assigned.");
				return;
			}

			await _authorRepository.DeleteAsync(authorToDelete.Id);
			await Console.Out.WriteLineAsync($"The author {authorToDelete.Id}, {authorToDelete.Name} {authorToDelete.Surname} deleted successfully.");
			Console.WriteLine();
		}

		public bool continueInAuthorsPage()
		{
			string continueKey = "y";
			while (true)
			{
				Console.Out.WriteLine("Do you want to continue work in Authors page?(y/n)");
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
		public async Task FindByName()
		{
			Console.WriteLine("Enter the name of the author: ");
			string authorName = Console.ReadLine();
			List<Author> authors = new();
			authors = await _authorRepository.FindByNameAsync(authorName);

			if (authors != null)
			{
				foreach (var author in authors)
				{
					await Console.Out.WriteLineAsync($"Author found: {author.Name} {author.Surname} wrote {author.Books.Count()} book(s):");
					foreach (var book in author.Books)
					{
						await Console.Out.WriteLineAsync($"* {book.Title},  zanras : {book.Genre}");
					}
				}
			}
			else
			{
				Console.WriteLine("Author not found.");
			}
		}
	}
}

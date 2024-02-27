using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Channels;

namespace LibraryConsoleApp;

internal class Program
{
    private static AuthorRepository authorRepository;
    private static BookRepository bookRepository;
    static async Task Main(string[] args)
    {
        var dbcf = new LibraryDbContextFactory();
        using var context = dbcf.CreateDbContext(args);

        authorRepository = new AuthorRepository(context);
        bookRepository = new BookRepository(context);

        while (true)
        {
            StartMenu();

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
                    PrintBooks(await GetBooks());
                    break;
                case 7:
                    PrintOneBook(await GetOneBook());
                    break;
                case 8:
                    await CreateBook();
                    break;
                case 9:
                    await UpdateBook();
                    break;
                case 10:
                    await DeleteBook();
                    break;
                case 11:
                    ExitProgram();
                    break;
                default:
                    Console.WriteLine("Not a valid input, enter a number from menu list");
                    break;
            }
        }
    }




    private static async Task DeleteBook()
    {
        await Console.Out.WriteLineAsync("Enter ID of the book you want to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());
        Book bookToDelete = await bookRepository.ReadAsync(id);
        if (bookToDelete == null)
        {
            Console.WriteLine($"A book with ID {id} not found.");
            return;
        }
        await bookRepository.DeleteAsync(bookToDelete);
        await Console.Out.WriteLineAsync($"The book ID {bookToDelete.Id}, title {bookToDelete.Title} deleted successfully.");
        Console.WriteLine();
    }

    private static async Task UpdateBook()
    {
        await Console.Out.WriteLineAsync("Enter ID of the book you want to update: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Book bookToUpdate = await bookRepository.ReadAsync(id);
        if (bookToUpdate == null)
        {
            Console.WriteLine($"A book with ID {id} not found.");
            return;
        }

        await Console.Out.WriteLineAsync("Enter updated book title: ");
        string title = Console.ReadLine();
        await Console.Out.WriteLineAsync("Enter updated book genre: ");
        string genre = Console.ReadLine();
        await Console.Out.WriteLineAsync("Enter updated book author's ID from the presented list of authors: ");
        PrintAuthors(await GetAuthors());
        long authorID = Convert.ToInt64(Console.ReadLine());

        bookToUpdate.Title = title;
        bookToUpdate.Genre = genre;
        bookToUpdate.AuthorId = authorID;
        await bookRepository.UpdateAsync(bookToUpdate);
        await Console.Out.WriteLineAsync("Book data updated successfully");
        Console.WriteLine();
    }

    private static async Task CreateBook()
    {
        await Console.Out.WriteLineAsync("Enter book title: ");
        string title = Console.ReadLine();
        await Console.Out.WriteLineAsync("Enter book genre: ");
        string genre = Console.ReadLine();
        await Console.Out.WriteLineAsync("Enter book author's ID from the presented list of authors:");
        PrintAuthors(await GetAuthors());
        long authorId = Convert.ToInt64(Console.ReadLine());

        var book = new Book()
        {
            Title = title,
            Genre = genre,
            AuthorId = authorId
        };
        await bookRepository.CreateAsync(book);
        await Console.Out.WriteLineAsync($"New book {book.Title} by {book.AuthorId} created successfully.");
        Console.WriteLine();
    }
    private static async Task<Book> GetOneBook()
    {
        await Console.Out.WriteLineAsync("Enter ID of the book you want to review: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Book bookToSee = await bookRepository.ReadAsync(id);
        if (bookToSee == null)
        {
            throw new KeyNotFoundException($"A book with ID {id} not found.");
        }

        return await bookRepository.ReadAsync(bookToSee.Id);
    }
    private static void PrintOneBook(Book book)
    {
        Console.WriteLine("ID   AUTHOR\t\t\tTITLE\t\t\t\t\t\t   GENRE");
        Console.WriteLine($"{book.Id,-4} {book.Author.Name,-10} {book.Author.Surname,-15} {book.Title,-50} {book.Genre,-20}");
        Console.WriteLine();
    }

    private static async Task<List<Book>> GetBooks()
    {
        return await bookRepository.ReadAllAsync();
    }

    private static void PrintBooks(List<Book> books)
    {
        Console.WriteLine("List of books:");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("ID   AUTHOR\t\t\tTITLE\t\t\t\t\t\t   GENRE");
        foreach (var book in books)
        {
            Console.WriteLine($"{book.Id,-4} {book.Author.Name,-10} {book.Author.Surname,-15} {book.Title,-50} {book.Genre,-20}");
        }
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine();
    }

    public static void ExitProgram()
    {
        Environment.Exit(0);
    }


    private static async Task DeleteAuthor()
    {
        await Console.Out.WriteLineAsync("Enter ID of the author you want to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());
        Author authorToDelete = await authorRepository.ReadAsync(id);
        if (authorToDelete == null)
        {
            Console.WriteLine($"An author with ID {id} not found.");
            return;
        }
        await authorRepository.DeleteAsync(authorToDelete);
        await Console.Out.WriteLineAsync($"The author {authorToDelete.Id}, {authorToDelete.Name} {authorToDelete.Surname} deleted successfully.");
        Console.WriteLine();
    }

    private static async Task UpdateAuthor()
    {
        await Console.Out.WriteLineAsync("Enter ID of the author you want to update: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Author authorToUpdate = await authorRepository.ReadAsync(id);
        if (authorToUpdate == null)
        {
            Console.WriteLine($"An author with ID {id} not found.");
            return;
        }

        await Console.Out.WriteLineAsync("Enter updated author name: ");
        string name = Console.ReadLine();
        await Console.Out.WriteLineAsync("Enter updated author surname: ");
        string surname = Console.ReadLine();

        authorToUpdate.Name = name;
        authorToUpdate.Surname = surname;
        await authorRepository.UpdateAsync(authorToUpdate);
        await Console.Out.WriteLineAsync($"Data of the author {authorToUpdate.Id}, {authorToUpdate.Name} {authorToUpdate.Surname} updated successfully.");
        Console.WriteLine();
    }

    private static async Task CreateAuthor()
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
        await authorRepository.CreateAsync(author);
        await Console.Out.WriteLineAsync($"New author {author.Name} {author.Surname} created successfully.");
        Console.WriteLine();
    }

    private static async Task<Author> GetOneAuthor()
    {
        await Console.Out.WriteLineAsync("Enter ID of the author you want to review: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Author authorToSee = await authorRepository.ReadAsync(id);
        if (authorToSee == null)
        {
            throw new KeyNotFoundException($"An author with ID {id} not found.");
        }

        return await authorRepository.ReadAsync(authorToSee.Id);
    }
    private static void PrintOneAuthor(Author author)
    {
        Console.WriteLine("ID     NAME\t    SURNAME\t QUANTITY OF BOOKS");
        Console.WriteLine($"{author.Id,-4}   {author.Name,-10}   {author.Surname,-15}   {author.Books.Count}");
        Console.WriteLine(author.Books); // noriu spausdinti konkretaus autoriaus knygu pavadinimus
        Console.WriteLine();
    }

    private static async Task<List<Author>> GetAuthors()
    {
        return await authorRepository.ReadAllAsync();
    }

    private static void PrintAuthors(List<Author> authors)
    {
        Console.WriteLine("List of authors:");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("ID     NAME\t    SURNAME\t QUANTITY OF BOOKS");
        foreach (var author in authors)
        {
            Console.WriteLine($"{author.Id,-4}   {author.Name,-10}   {author.Surname,-15}   {author.Books.Count}");
        }
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine();
    }

    public static void StartMenu()
    {
        Console.WriteLine("List of possible actions: ");
        Console.WriteLine(" 1 - Get list of Authors");
        Console.WriteLine(" 2 - Info on the Author");
        Console.WriteLine(" 3 - Create new author");
        Console.WriteLine(" 4 - Update author");
        Console.WriteLine(" 5 - Delete author");
        Console.WriteLine(" 6 - Get list of books");
        Console.WriteLine(" 7 - Get info on the book");
        Console.WriteLine(" 8 - Create new book");
        Console.WriteLine(" 9 - Update book");
        Console.WriteLine("10 - Delete book");
        Console.WriteLine("11 - Exit program");
        Console.WriteLine("---------------------");
        Console.WriteLine();
    }
}

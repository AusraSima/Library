using LibraryWeb.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace LibraryWeb.Services;

public class AuthorService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://localhost:7012/api/AuthorControler";

    public AuthorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<Author>> getAuthorsAsync()
    {
        var response = await _httpClient.GetAsync(_baseUrl);
        return await response.Content.ReadFromJsonAsync<List<Author>>();
    }
    public async Task<Author> GetAuthorByIdAsync(long id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Author>();
        }
        else
        {
            throw new Exception($"Failed to fetch author with ID {id}: {response.StatusCode}");
        }
    }
    //public async Task CreateAuthorAsync(Author author)
    //{
    //    var content = JsonContent.Create(author, MediaTypeHeaderValue.Parse("application/json"));
    //    var response = await _httpClient.PostAsync(_baseUrl, content);

    //    if (!response.IsSuccessStatusCode)
    //        throw new Exception($"Failed to create author: {response.StatusCode}");
    //}
    public async Task CreateAuthorAsync(Author author)
    {
        // Perform validation
        if (string.IsNullOrWhiteSpace(author.Name))
        {
            throw new ArgumentException("Author name cannot be empty or null.", nameof(author));
        }

        if (!Regex.IsMatch(author.Name, @"^[a-zA-Z]+(?: [a-zA-Z]+)*$"))
        {
            throw new ArgumentException("Author name can only contain letters.", nameof(author));
        }

        //if (string.IsNullOrWhiteSpace(author.Surname))
        //{
        //	throw new ArgumentException("Author surname cannot be empty or null.", nameof(author));
        //}

        if (!Regex.IsMatch(author.Surname, @"^[a-zA-Z]+(?: [a-zA-Z]+)*$"))
        {
            throw new ArgumentException("Author surname can only contain letters.", nameof(author));
        }

        // You can add more specific validation rules for name and surname if needed

        var content = JsonContent.Create(author, MediaTypeHeaderValue.Parse("application/json"));
        var response = await _httpClient.PostAsync(_baseUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to create author: {response.StatusCode}");
        }
    }

    public async Task UpdateAuthorAsync(Author author)
    {
        author.Books = null;
        var json = System.Text.Json.JsonSerializer.Serialize(author);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_baseUrl}/{author.Id}/json", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"{json}");
            throw new Exception($"----------{response.StatusCode}-----------");
            throw new Exception($"Failed to update author with ID {content}");
            throw new Exception($"Failed to update author with ID {author.Id} {author.Name} {author.Surname}:  {_baseUrl}/{author.Id}/edit {response.StatusCode}");
        }
    }
    public async Task DeleteAuthorAsync(long id)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to delete author with ID {id}: {response.StatusCode}");
        }
    }
}
using LibraryWeb.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace LibraryWeb.Services
{
	public class BookService
	{
		private readonly HttpClient _httpClient;
		private readonly string _baseUrl = "https://localhost:7012/api/BookControler";

		public BookService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<List<Book>> GetBooksAsync()
		{
			var response = await _httpClient.GetAsync(_baseUrl);
			return await response.Content.ReadFromJsonAsync<List<Book>>();
		}
		public async Task<Book> GetBookByIdAsync(long id)
		{
			var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<Book>();
			}
			else
			{
				throw new Exception($"Failed to fetch author with ID {id}: {response.StatusCode}");
			}
		}

		public async Task CreateBookAsync(Book book)
		{
			var json = System.Text.Json.JsonSerializer.Serialize(book);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(_baseUrl, content);
			if (!response.IsSuccessStatusCode)
				throw new Exception($"Failed to create author: {response.StatusCode}");
		}

		public async Task UpdateBookAsync(Book book)
		{
			book.Author = null;
			var json = System.Text.Json.JsonSerializer.Serialize(book);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync($"{_baseUrl}/{book.Id}/json", content);
		}
		public async Task DeleteBookAsync(long id)
		{
			var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"Failed to delete author with ID {id}: {response.StatusCode}");
			}
		}
	}
}

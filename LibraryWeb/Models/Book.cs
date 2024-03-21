
namespace LibraryWeb.Models
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public long AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}


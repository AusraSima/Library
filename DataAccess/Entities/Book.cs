using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
	public class Book
	{
		[Key]
		public long Id { get; set; }
		[MaxLength(255)]
		public string Title { get; set; }
		[MaxLength(255)]
		public string Genre { get; set; }
		[ForeignKey("author_id")]
		[Column(name: "author_id")]
		public long AuthorId { get; set; }
		public Author? Author { get; set; }
	}
}

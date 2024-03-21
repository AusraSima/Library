using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
	public class Author
	{
		public long Id { get; set; }
		[MaxLength(255)]
		public string Name { get; set; }
		[MaxLength(255)]
		public string? Surname { get; set; }
		public ICollection<Book>? Books { get; set; }
	}
}

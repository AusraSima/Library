using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        [Column(name: "author_id")]
        public long AuthorId { get; set; }
        public Author Author { get; set; }
    }
}

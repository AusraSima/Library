using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Author
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Surname { get; set; }
        public ICollection<Book> Books { get; set; }
    }


}

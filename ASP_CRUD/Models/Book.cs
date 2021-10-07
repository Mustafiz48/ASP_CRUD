using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_CRUD.Models
{
    public class Book
    {
        [Key]

        public int Id { get; set; }

        public string BookName { get; set; }

        public List<BookCharacter> Characters { get; set; }

    }
}

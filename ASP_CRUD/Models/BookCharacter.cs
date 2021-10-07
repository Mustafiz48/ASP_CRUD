using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_CRUD.Models
{
    public class BookCharacter
    {
        [Key]
        public int Id { get; set; }

        public string CharacterName { get; set; }


        public Book Book { get; set; }

    }
}

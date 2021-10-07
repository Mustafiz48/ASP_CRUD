using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_CRUD.Models
{
    public class Hero
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Nemesis { get; set; }

        [Required]
        [StringLength(100)]
        public string Power { get; set; }
    }
}

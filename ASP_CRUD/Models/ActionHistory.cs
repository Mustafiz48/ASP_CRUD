using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_CRUD.Models
{
    public class ActionHistory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Action { get; set; }
    }
}

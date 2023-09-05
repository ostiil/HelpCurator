using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP.Model
{
    public class TypeEvent
    {
        [Key]
        public int Id_type { get; set; }
        public string? Name_type { get; set; }
        public List<Event> Events { get; set; } = new();
    }
}

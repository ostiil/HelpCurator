using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP.Model
{
    public class Event
    {
        [Key]
        public int Id_event { get; set; }
        public string? Name_event { get; set; }
        public DateTime Date_event { get; set; }
        public bool Status { get; set; }
        public string? Description { get; set; }
        public int Students { get; set; }
        public int Type_id { get; set; }

        [ForeignKey("Type_id")]
        public TypeEvent? TypeEvent { get; set; }
    }
}

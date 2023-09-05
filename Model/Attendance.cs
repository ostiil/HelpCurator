using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP.Model
{
    public class Attendance
    {
        [Key]
        public int Id_record { get; set; }
        public string? Month { get; set; }
        public string? Student { get; set; }
        public double InTotal { get; set; }
        public double Respectful { get; set; }
        public double NotRespectful { get; set; }
        public double Delay { get; set; }
        public string? Description { get; set; }
    }
}

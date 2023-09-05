using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP.Model
{

    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_student { get; set; }
        public string? Fio_student { get; set; }
        public string? Specialnost { get; set; }
        public DateTime? Birth { get; set; }
        public string? AdressRegistr { get; set; }
        public string? Adress { get; set; }
        public string? Phone { get; set; }
        public string? Fio_mother { get; set; }
        public string? Phone_mother { get; set; }
        public string? Fio_father { get; set; }
        public string? Phone_father { get; set; }

        public string? Benefits { get; set; }
        public string? Order_of_enrollment { get; set; }
        public DateTime? Date_enrollmant { get; set; }
        public DateTime? Period { get; set; }
    }
}

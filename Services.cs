using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;

using System.Windows.Controls;
using OfficeOpenXml;
using DP.Model;

namespace DP
{
    public class Services
    {
       
        public Student AddStudent(string fio, string group, DateTime birth, string adressRegistr, string adress, string phone, 
            string fio_mother, string phone_mother, string fio_father, string phone_father,
            string benefits, string order, DateTime date_enrollment, DateTime period)
        {
            Student student = new Student 
            { 
                Fio_student = fio,
                Specialnost = group,
                Birth = birth,
                AdressRegistr = adressRegistr,
                Adress = adress,
                Phone = phone,
                Fio_mother = fio_mother,
                Phone_mother = phone_mother,
                Fio_father = fio_father,
                Phone_father = phone_father,
                Benefits = benefits,
                Order_of_enrollment = order,
                Date_enrollmant = date_enrollment,
                Period = period
            };
            using (Context context = new Context())
            {
                context.student.Add(student);
                context.SaveChanges();
            }
            return student;
        }

        public Event AddEvent(string name, DateTime date, bool status, string description, int student, int type_id)
        {
            Event report = new Event
            {
                Name_event = name,
                Date_event = date,
                Status = status,
                Description = description,
                Students = student,
                Type_id = type_id
            };
            using (Context context = new Context())
            {
                context.@event.Add(report);
                context.SaveChanges();
            }
            return report;
        }

    }
}

using DP.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP
{
    public class Context : DbContext
    {
        public Context() 
        {
            Database.EnsureCreated();
        }
        public DbSet<Student> student { get; set; }
        public DbSet<Event> @event { get; set; }
        public DbSet<Attendance> attendance { get; set; }
        public DbSet<Autorize> autorize { get; set; }
        public DbSet<TypeEvent> typeEvent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source = BuscoSexo\SQLEXPRESS; Database=StudyGroup; Integrated Security = True; TrustServerCertificate=True; MultipleActiveResultSets=true");
        }
    }

    
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmployee.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ModelsApp.Employee.Employee> Employee { get; set; }

        public ApplicationContext()
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-ICVUT79;Initial Catalog=galaxy;Integrated Security=True;");
        }
    }
}

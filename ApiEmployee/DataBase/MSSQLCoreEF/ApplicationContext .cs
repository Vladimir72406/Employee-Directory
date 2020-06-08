using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiEmployee;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ApiEmployee.DataBase
{
    public class ApplicationContext : DbContext
    {
        IConfigurationRoot configuration;
        public DbSet<ModelsApp.Employee.Employee> Employee { get; set; }


        public ApplicationContext()
        {
            //Database.EnsureCreated();  
            configuration = this.GetConfiguration();            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStr = configuration["ConnectionStrings:DefaultConnection"];

            optionsBuilder.UseSqlServer(connectionStr);
            //optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-ICVUT79;Initial Catalog=galaxy;Integrated Security=True;");
        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelsApp.Employee;
using ModelsApp.Result;

namespace ApiEmployee.DataBase
{
    public class RepositoryMSSQLCore : IRepository
    {
        public Employee createEmployee(Employee newEmployee)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var i = db.Employee.Add(newEmployee);
                var r = db.SaveChanges();
            }

            return newEmployee;
        }

        public Employee getEmployee(int id)
        {
            Employee empl;
            using (ApplicationContext db = new ApplicationContext())
            {
                empl = db.Employee.FirstOrDefault(e => e.employee_id == id);                
            }

            return empl;
        }

        public List<Employee> getListEmployee(FiltrEmployee filterEmpl)
        {
            List<Employee> listEmployee = new List<Employee>();
            using (ApplicationContext db = new ApplicationContext())
            {
                if (filterEmpl == null | (filterEmpl.Surname == null && filterEmpl.Name== null && filterEmpl.Middle_name == null))
                {
                    listEmployee = db.Employee.ToList();
                }
                else
                {
                    listEmployee = db.Employee.Where(p => (p.surname.StartsWith(filterEmpl.Surname) || filterEmpl.Surname==null ) && 
                                                           (p.name.StartsWith(filterEmpl.Name) || filterEmpl.Name == null) &&
                                                            (p.middle_name.StartsWith(filterEmpl.Middle_name) || filterEmpl.Middle_name == null)).ToList();
                }
            }
            return listEmployee;
        }

        public ResultApi updateEmployee(int employee_id, Employee empl)
        {
            ResultApi result = new ResultApi();
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Update(empl);
                db.SaveChanges();
            }

            result.employee = empl;
            result.code = 0;
            result.info = "";
                        
            return result;
        }
    }
}

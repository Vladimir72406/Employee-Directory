using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelsApp.Employee;
using ModelsApp.Result;

namespace ApiEmployee.DataBase
{
    public class RepositoryMSSQLCoreEF : IRepository
    {
        public ResultApi createEmployee(Employee newEmployee)
        {
            ResultApi resultApi = new ResultApi();


            using (ApplicationContext db = new ApplicationContext())
            {
                var i = db.Employee.Add(newEmployee);
                var r = db.SaveChanges();
            }

            if (newEmployee.employee_id != null && newEmployee.employee_id > 0)
            {
                resultApi.code = 0;
                resultApi.employee = newEmployee;
            }
            else
            {
                resultApi.code = -1;
                resultApi.info = "Ошибка создания";
            }

            return resultApi;
        }

        public ResultApi getEmployee(int id)
        {
            ResultApi resultApi = new ResultApi();
            
            Employee empl;
            using (ApplicationContext db = new ApplicationContext())
            {
                empl = db.Employee.FirstOrDefault(e => e.employee_id == id);                
            }

            if (empl != null)
            {
                resultApi.employee = empl;
                resultApi.code = 0;
            }
            else
            {
                resultApi.code = -1;
                resultApi.info = "Не найден.";
            }

            return resultApi;
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

        public Result deleteEmployee(int employee_id)
        {
            Result resultDeleteEmployee = new Result();
            using (ApplicationContext db = new ApplicationContext())
            {
                var empl = this.getEmployee(employee_id);

                db.Employee.Remove(empl.employee);
            }

            resultDeleteEmployee.code = 0;
            resultDeleteEmployee.info = "";

            return resultDeleteEmployee;
        }
    }
}

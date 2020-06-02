using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelsApp.Employee;
using ModelsApp.Result;

namespace ApiEmployee.DataBase
{
    public interface IRepository
    {
        public ResultApi getEmployee(int id);

        public List<Employee> getListEmployee(FiltrEmployee filterEmpl);

        public ResultApi createEmployee(Employee newEmployee);

        public ResultApi updateEmployee(int id, Employee empl);

        public Result deleteEmployee(int employee_id);
    }
}

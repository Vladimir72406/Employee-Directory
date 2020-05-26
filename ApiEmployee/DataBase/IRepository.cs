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
        public Employee getEmployee(int id);

        public List<Employee> getListEmployee(FiltrEmployee filterEmpl);

        public Employee createEmployee(Employee newEmployee);

        public ResultApi updateEmployee(int id, Employee empl);
    }
}

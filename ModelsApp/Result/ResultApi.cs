using System;
using System.Collections.Generic;
using System.Text;
using ModelsApp.Employee;

namespace ModelsApp.Result
{
    public class ResultApi : Result
    {

        public Employee.Employee employee { get; set; }
        public List<Employee.Employee> lstEmployee { get; set; }
    }
}

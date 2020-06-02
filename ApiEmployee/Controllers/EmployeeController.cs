using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApp.Employee;
using ModelsApp.Result;
using ApiEmployee.DataBase;
//using System.Web.Http;


/*
create table Employee
(
	employee_id		numeric(10,0)	primary key identity(1,1),
    surname			varchar(100)	null,
	[name]			varchar(100)	null,
	middle_name		varchar(100)	null,
	birthday		datetime		null
)


insert into Employee(surname,[name],middle_name	,birthday)
values('Жук','Владимир', 'Николаевич','19911223')

select * from Employee

    GRANT SELECT ON dbo.Employee TO public

GRANT UPDATE ON dbo.Employee TO public

GRANT insert ON dbo.Employee TO public
*/

namespace ApiEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IRepository db = new RepositoryMSSQLRF();//RepositoryMSSQLCoreEF();
        //IRepository db = new RepositoryMSSQLCoreEF();

        // GET: api/Employee
        [HttpGet]
        public ResultApi Get([FromQuery] FiltrEmployee filterEmpl)
        
        {
            ResultApi result = new ResultApi();
            
            try
            {
                result.lstEmployee = db.getListEmployee(filterEmpl);
                result.code = 0;
                result.info = "";                
            }
            catch (Exception e)
            {
                result.code = -1;
                result.info = e.Message.ToString() + "\n" + e.Message;
            }
            return result;
        }

        // GET: api/Employee/5
        [HttpGet("{id}", Name = "Get")]
        public ResultApi Get(int id)
        {
            var result = new ResultApi();
            var employeeResult = db.getEmployee(id);

            result.employee = employeeResult.employee;
            result.code = 0;
            result.info = "";
            return result;            
        }

        // POST: api/Employee
        [HttpPost]
        public ResultApi Post([FromBody] Employee newEmpl)
        {
            var resultApi = new ResultApi();


            var createdEmpl = db.createEmployee(newEmpl);

            if (createdEmpl.employee.employee_id > 0)
            {
                resultApi.code = 0;
                resultApi.employee = createdEmpl.employee;
            }
            else
            {
                resultApi.code = -1;
                resultApi.info = "Ошибка создания сотрудника";
            }

            return resultApi;
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public ResultApi Put(int employee_id, [FromBody] Employee editEmployee)
        {
            var resultApi = new ResultApi();
            
            ResultApi result = db.updateEmployee(employee_id, editEmployee);

            if (result.code == 0)
            {
                resultApi.code = 0;
                resultApi.employee = editEmployee;
            }
            else
            {
                resultApi.code = -1;
                resultApi.info = "Ошибка изменения данных сотрудника";
            }

            return resultApi;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public Result Delete(int id)
        {
            Result resultDeleteEmployee = new Result();

            try
            {
                resultDeleteEmployee = db.deleteEmployee(id);
            }
            catch (Exception e)
            {
                resultDeleteEmployee.code = -1;
                resultDeleteEmployee.info = "delete error => " + e.Message.ToString();
            }
         
            return resultDeleteEmployee;
        }
    }
}

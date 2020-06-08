using ModelsApp.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelsApp.Result;

namespace ApiEmployee.DataBase
{
    public interface IRepositoryContact
    {
        public ResultApiContact getContactsOfEmployee(int employee_id);

        public ResultApiContact addNewContact(Contact contact);

        public Result deleteContact(int contact_id);                
    }
}

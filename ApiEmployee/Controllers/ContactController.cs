using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApp.Result;
using ModelsApp.Employee;
using ApiEmployee.DataBase;

namespace ApiEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        IRepositoryContact db = InstanceDB.getInstanceContact();
        // GET: api/Contact
        [HttpGet]
        public List<Contact> Get()
        {
            //return new string[] { "value1", "value2" };
            return new List<Contact>();
        }

        // GET: api/Contact/5
        [HttpGet("{employee_id}")/*, Name = "Get")*/]        
        public ResultApiContact Get(int employee_id)
        {
            ResultApiContact resultApiContact = new ResultApiContact();

            try
            {
                var lstContacts = db.getContactsOfEmployee(employee_id).lstContact;
                resultApiContact.lstContact = lstContacts;
                resultApiContact.code = 0;

            } 
            catch (Exception e)
            {
                resultApiContact.code = -1;
                resultApiContact.info = "Ошибка получения списка контактов" + e.Message.ToString();
            }

            return resultApiContact;
        }

        // POST: api/Contact
        [HttpPost]
        public ResultApiContact Post([FromBody] Contact newContact)
        {
            ResultApiContact result;

            try
            {
                result = db.addNewContact(newContact);
            }
            catch (Exception e)
            {
                result = new ResultApiContact();

                result.code = -1;
                result.info = "Ошибка полкчения данных. " + e.Message.ToString();
            }

            return result;
        }

        // PUT: api/Contact/5
        [HttpPut("{contact_id}")]
        public void Put(int contact_id)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{contact_id}")]
        public Result Delete(int contact_id)
        {
            Result deleteResult = new Result();

            try
            {
                deleteResult = db.deleteContact(contact_id);
            }
            catch (Exception e)
            {
                deleteResult.code = -1;
                deleteResult.info = "Ошибка при обращении к бд." + e.Message.ToString();
            }

            return deleteResult;
        }
    }
}

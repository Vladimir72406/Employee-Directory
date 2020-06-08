using Microsoft.Extensions.Configuration;
using ModelsApp.Employee;
using ModelsApp.Result;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmployee.DataBase.MSSQLRF
{
    public class RepositoryContactMSSQLRF : IRepositoryContact
    {

        string sqlConnectionString = "";
        IConfigurationRoot configuration;
        public RepositoryContactMSSQLRF()
        {
            configuration = this.GetConfiguration();
            this.sqlConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public ResultApiContact addNewContact(Contact newContact)
        {
            string info = string.Empty;
            int ret = 0;
            ResultApiContact resultApiContact = new ResultApiContact();
            string iud_contact = "iud_contact";

            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand command = new SqlCommand(iud_contact, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter sp_iud = new SqlParameter("@iud", System.Data.SqlDbType.Int);
                sp_iud.Value = 1;
                sp_iud.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_contact_id = new SqlParameter("@contact_id", System.Data.SqlDbType.Int);
                sp_contact_id.Value = null;
                sp_contact_id.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_employee_id = new SqlParameter("@employee_id", System.Data.SqlDbType.Int);
                sp_employee_id.Value = newContact.employee_id;
                sp_employee_id.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_contact_type = new SqlParameter("@contact_type", System.Data.SqlDbType.Int);
                sp_contact_type.Value = newContact.contact_type;
                sp_contact_type.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_content = new SqlParameter("@content", System.Data.SqlDbType.NVarChar);
                sp_content.Value = newContact.content;
                sp_content.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_comment = new SqlParameter("@comment", System.Data.SqlDbType.NVarChar);
                sp_comment.Value = newContact.comment;
                sp_comment.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_verify = new SqlParameter("@verify ", System.Data.SqlDbType.Int);
                sp_verify.Value = newContact.verify;
                sp_verify.Direction = System.Data.ParameterDirection.Input;
                                                
                command.Parameters.Add(sp_iud);
                command.Parameters.Add(sp_contact_id);
                command.Parameters.Add(sp_employee_id);
                command.Parameters.Add(sp_contact_type);
                command.Parameters.Add(sp_content);
                command.Parameters.Add(sp_comment);
                command.Parameters.Add(sp_verify);



                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ret = Convert.ToInt32(reader.GetValue(0));
                        info = reader.GetValue(1).ToString();
                    }
                }
                command.Connection.Close();

                if (ret > 0)
                {
                    newContact.contact_id = ret;

                    resultApiContact.code = 0;
                    resultApiContact.info = info;
                    resultApiContact.contact = newContact;
                }
                else
                {
                    resultApiContact.code = -1;
                    resultApiContact.info = info;
                    resultApiContact.contact = newContact;
                }

                return resultApiContact;
            }
        }

        public Result deleteContact(int contact_id)
        {
            string info = string.Empty;
            int ret = 0;
            Result resultApiContact = new Result();
            string iud_contact = "iud_contact";

            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand command = new SqlCommand(iud_contact, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter sp_iud = new SqlParameter("@iud", System.Data.SqlDbType.Int);
                sp_iud.Value = 3;
                sp_iud.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_contact_id = new SqlParameter("@contact_id", System.Data.SqlDbType.Int);
                sp_contact_id.Value = contact_id;
                sp_contact_id.Direction = System.Data.ParameterDirection.Input;

                command.Parameters.Add(sp_iud);
                command.Parameters.Add(sp_contact_id);               

                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ret = Convert.ToInt32(reader.GetValue(0));
                        info = reader.GetValue(1).ToString();
                    }
                }
                command.Connection.Close();

                if (ret == 0)
                {
                    resultApiContact.code = 0;
                    resultApiContact.info = info;                    
                }
                else
                {
                    resultApiContact.code = -1;
                    resultApiContact.info = info;                    
                }

                return resultApiContact;
            }
        }

        public ResultApiContact getContactsOfEmployee(int employee_id)
        {
            List<Contact> listContact = new List<Contact>();
            var result = new ResultApiContact();
            string get_contacts_of_employee = "get_contacts_of_employee";

            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand command = new SqlCommand(get_contacts_of_employee, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter sp_employee_id = new SqlParameter("@employee_id", System.Data.SqlDbType.Int);
                sp_employee_id.Value = employee_id;
                sp_employee_id.Direction = System.Data.ParameterDirection.Input;

                command.Parameters.Add(sp_employee_id);
                
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contact contact = new Contact();

                        contact.employee_id = 


                        contact.contact_id = Convert.ToInt32(reader.GetValue(0));
                        contact.employee_id = Convert.ToInt32(reader.GetValue(1));
                        contact.contact_type = Convert.ToInt32(reader.GetValue(2));
                        contact.content = (reader.GetValue(3)).ToString();
                        contact.verify = Convert.ToInt32(reader.GetValue(4));

                        listContact.Add(contact);
                    }
                }
                command.Connection.Close();

                result.code = 0;
                result.lstContact = listContact;

                return result;
            }
        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}

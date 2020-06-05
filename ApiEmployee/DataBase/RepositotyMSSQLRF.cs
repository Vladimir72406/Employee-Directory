using Microsoft.Extensions.Configuration;
using ModelsApp.Employee;
using ModelsApp.Result;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmployee.DataBase
{
    public class RepositoryMSSQLRF : IRepository
    {
        IConfigurationRoot configuration;
        string sqlConnectionString = "";//@"Data Source=DESKTOP-ICVUT79;Initial Catalog=galaxy;Integrated Security=True;";

        public RepositoryMSSQLRF()
        {
            configuration = this.GetConfiguration();
            this.sqlConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public ResultApi createEmployee(Employee newEmployee)
        {
            ResultApi resultApi = new ResultApi();
            int ret = 0;
            string info = string.Empty;

            string prc_iud_employee = "iud_employee";

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    SqlCommand command = new SqlCommand(prc_iud_employee, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter sp_iud = new SqlParameter("@iud", SqlDbType.Int);
                    sp_iud.Value = 1;
                    sp_iud.Direction = ParameterDirection.Input;

                    SqlParameter sp_employee_id = new SqlParameter("@employee_id", SqlDbType.Int);
                    sp_employee_id.Value = newEmployee.employee_id;
                    sp_employee_id.Direction = ParameterDirection.Input;

                    SqlParameter sp_surname = new SqlParameter("@surname", System.Data.SqlDbType.NVarChar);
                    sp_surname.Value = newEmployee.surname;
                    sp_surname.Direction = System.Data.ParameterDirection.Input;

                    SqlParameter sp_name = new SqlParameter("@name", System.Data.SqlDbType.NVarChar); //sp_name.DbType = System.Data.DbType.String;?
                    sp_name.Value = newEmployee.name;
                    sp_name.Direction = System.Data.ParameterDirection.Input;

                    SqlParameter sp_middleName = new SqlParameter("@middle_name", System.Data.SqlDbType.NVarChar);
                    sp_middleName.Value = newEmployee.middle_name;
                    sp_middleName.Direction = System.Data.ParameterDirection.Input;

                    command.Parameters.Add(sp_iud);
                    command.Parameters.Add(sp_employee_id);
                    command.Parameters.Add(sp_surname);
                    command.Parameters.Add(sp_name);
                    command.Parameters.Add(sp_middleName);

                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ret = Convert.ToInt32(reader.GetValue(0));
                            info = Convert.ToString(reader.GetValue(1));
                        }
                    }

                    newEmployee.employee_id = ret;

                    command.Connection.Close();

                    resultApi.code = 0;
                    resultApi.employee = newEmployee;
                }
            }
            catch (Exception e)
            {
                resultApi.code = -1;
                resultApi.info = "create error => " + e.Message.ToString();
            }

            
               

            return resultApi;

        }

        public Result deleteEmployee(int employee_id)
        {
            Result resultDeleteEmployee = new Result();
            string prc_iud_employee = "iud_employee";
            int ret = 0;
            string info = string.Empty;

            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand command = new SqlCommand(prc_iud_employee, connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter sp_iud = new SqlParameter("@iud", SqlDbType.Int);
                sp_iud.Value = 3;
                sp_iud.Direction = ParameterDirection.Input;

                SqlParameter sp_employee_id = new SqlParameter("@employee_id", SqlDbType.Int);
                sp_employee_id.Value = employee_id;
                sp_employee_id.Direction = ParameterDirection.Input;

                command.Parameters.Add(sp_iud);
                command.Parameters.Add(sp_employee_id);

                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ret = Convert.ToInt32(reader.GetValue(0));
                        info = Convert.ToString(reader.GetValue(1));
                    }
                }

                resultDeleteEmployee.code = ret;
                resultDeleteEmployee.info = info;

                command.Connection.Close();
            }

            return resultDeleteEmployee;
        }

        public ResultApi getEmployee(int employee_id)
        {
            ResultApi resultApi = new ResultApi();
            Employee empl = new Employee();
            string prc_get_employee_by_id = "get_employee_by_id";
                        
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand command = new SqlCommand(prc_get_employee_by_id, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter sp_employee_id = new SqlParameter("@id", System.Data.SqlDbType.Int);
                sp_employee_id.ParameterName = "@id";
                sp_employee_id.Value = employee_id;
                sp_employee_id.Direction = System.Data.ParameterDirection.Input;
                command.Parameters.Add(sp_employee_id);

                command.Connection.Open();                                
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        empl.employee_id = Convert.ToInt32(reader.GetValue(0));
                        empl.surname = Convert.ToString(reader.GetValue(1));
                        empl.name = Convert.ToString(reader.GetValue(2));
                        empl.middle_name = Convert.ToString(reader.GetValue(3));
                        empl.birthday = reader.GetValue(4) == DBNull.Value ? default(DateTime) : Convert.ToDateTime(reader.GetValue(4));
                    }
                }

                command.Connection.Close();                
            }

            if (empl != null)
            {
                resultApi.code = 0;
                resultApi.employee = empl;
            }
            else
            {
                resultApi.code = -1;
                resultApi.info = "Ошибка поиска";
            }

            return resultApi;
        }

        public List<Employee> getListEmployee(FiltrEmployee filterEmpl)
        {
            List<Employee> listEmployee = new List<Employee>();
            String get_employee_list = "get_employee_list";

            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand command = new SqlCommand(get_employee_list, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter sp_surname = new SqlParameter("@surname", System.Data.SqlDbType.NVarChar);
                sp_surname.Value = filterEmpl.Surname;
                sp_surname.Direction = System.Data.ParameterDirection.Input;
                
                SqlParameter sp_name = new SqlParameter("@name", System.Data.SqlDbType.NVarChar); //sp_name.DbType = System.Data.DbType.String;?
                sp_name.Value = filterEmpl.Name;
                sp_name.Direction = System.Data.ParameterDirection.Input;                

                SqlParameter sp_middleName = new SqlParameter("@middle_name", System.Data.SqlDbType.NVarChar);                
                sp_middleName.Value = filterEmpl.Middle_name;
                sp_middleName.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_numberPage = new SqlParameter("@numberPage", System.Data.SqlDbType.Int);
                sp_numberPage.Value = filterEmpl.numberPage;
                sp_numberPage.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_countInPage = new SqlParameter("@countInPage", System.Data.SqlDbType.Int);
                sp_countInPage.Value = filterEmpl.countInPage;
                sp_countInPage.Direction = System.Data.ParameterDirection.Input;
                
                command.Parameters.Add(sp_surname);
                command.Parameters.Add(sp_name);
                command.Parameters.Add(sp_middleName);
                command.Parameters.Add(sp_numberPage);
                command.Parameters.Add(sp_countInPage);

                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Employee empl = new Employee();

                        empl.employee_id = Convert.ToInt32(reader.GetValue(0));
                        empl.surname = Convert.ToString(reader.GetValue(1));
                        empl.name = Convert.ToString(reader.GetValue(2));
                        empl.middle_name = Convert.ToString(reader.GetValue(3));
                        empl.birthday =  reader.GetValue(4) == DBNull.Value ? default(DateTime) : Convert.ToDateTime(reader.GetValue(4)) ;

                        listEmployee.Add(empl);
                    }
                }
                command.Connection.Close();

                return listEmployee;
            }
        }

        public ResultApi updateEmployee(int id, Employee empl)
        {
            ResultApi resultUpdateEmplouee = new ResultApi();
            string sp_iud_empliyee = "iud_employee";
            int ret = 0;
            string info = string.Empty;


            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand command = new SqlCommand(sp_iud_empliyee, connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter sp_iud = new SqlParameter("@iud", SqlDbType.Int);
                sp_iud.Value = 2;
                sp_iud.Direction = ParameterDirection.Input;

                SqlParameter sp_employee_id = new SqlParameter("@employee_id", SqlDbType.Int);
                sp_employee_id.Value = empl.employee_id;
                sp_employee_id.Direction = ParameterDirection.Input;

                SqlParameter sp_surname = new SqlParameter("@surname", System.Data.SqlDbType.NVarChar);
                sp_surname.Value = empl.surname;
                sp_surname.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_name = new SqlParameter("@name", System.Data.SqlDbType.NVarChar); //sp_name.DbType = System.Data.DbType.String;?
                sp_name.Value = empl.name;
                sp_name.Direction = System.Data.ParameterDirection.Input;

                SqlParameter sp_middleName = new SqlParameter("@middle_name", System.Data.SqlDbType.NVarChar);
                sp_middleName.Value = empl.middle_name;
                sp_middleName.Direction = System.Data.ParameterDirection.Input;

                command.Parameters.Add(sp_iud);
                command.Parameters.Add(sp_employee_id);
                command.Parameters.Add(sp_surname);
                command.Parameters.Add(sp_name);
                command.Parameters.Add(sp_middleName);

                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ret = Convert.ToInt32(reader.GetValue(0));
                        info = Convert.ToString(reader.GetValue(1));                        
                    }
                }

                command.Connection.Close();
            }

            resultUpdateEmplouee.code = ret;
            resultUpdateEmplouee.info = info;

            return resultUpdateEmplouee;
        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}

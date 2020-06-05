using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using ModelsApp.Employee;
using ModelsApp.Result;
using Newtonsoft.Json;

namespace WpfClientEmployee.Logic
{
    
    public class HttpClientHR
    {
        string urlEmployeeApi = "http://localhost:802/api/Employee";
        public async Task<List<Employee>> getListEmployeeAsync(FiltrEmployee filtrEmpl)
        {
            var listEmployee = new List<Employee>();
            var resultApi = new ResultApi();

            HttpClient client = new HttpClient();

            string req = urlEmployeeApi + "?Surname=" + filtrEmpl.Surname + "&Name=" +
                filtrEmpl.Name + "&Middle_name=" + filtrEmpl.Middle_name + "&countInPage=" + filtrEmpl.countInPage + "&numberPage=" + filtrEmpl.numberPage;

            HttpResponseMessage response = await client.GetAsync(req);
            response.EnsureSuccessStatusCode();            

            string responseBody = await response.Content.ReadAsStringAsync();
            resultApi = JsonConvert.DeserializeObject<ResultApi>(responseBody);

            if (resultApi.code >= 0)
            {
                listEmployee = resultApi.lstEmployee;
            }

            return listEmployee;
        }
        public async Task<ResultApi> createNewEmployeeAsunc(Employee newEmployee)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(urlEmployeeApi);
            var contentJson = JsonConvert.SerializeObject(newEmployee);
            var data = new StringContent(contentJson, Encoding.UTF8, "application/json");
            var responce = await httpClient.PostAsync(httpClient.BaseAddress, data);

            string resultStr = responce.Content.ReadAsStringAsync().Result;

            ResultApi resultApi = (ResultApi)JsonConvert.DeserializeObject(resultStr, typeof(ResultApi));

            return resultApi;
        }

        public async Task<Employee> getEmployeeAsync(int id)
        {
            Employee employee = new Employee();
            ResultApi resultApi = new ResultApi();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(urlEmployeeApi + "/" + id.ToString());
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            resultApi = JsonConvert.DeserializeObject<ResultApi>(responseBody);

            if (resultApi.code >= 0)
            {
                employee = resultApi.employee;
            }
            return employee;
        }

        public async Task<ResultApi> changedEmployeeAsync(Employee empl)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(urlEmployeeApi);

            var contentJson = JsonConvert.SerializeObject(empl);
            var data = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var responce = await httpClient.PutAsync(urlEmployeeApi + "/" + empl.employee_id.ToString(), data);
                        
            string resultStr = await responce.Content.ReadAsStringAsync();

            ResultApi resultApi = (ResultApi)JsonConvert.DeserializeObject(resultStr, typeof(ResultApi));

            return resultApi;
        }

        public async Task<Result> deleteEmployeeAsync(int employee_id)
        {
            HttpClient httpClient = new HttpClient();
            Result resultDelete = new Result();

            httpClient.BaseAddress = new Uri(urlEmployeeApi + "/" + employee_id.ToString());

            var result = await httpClient.DeleteAsync(httpClient.BaseAddress);

            string stringResult = await result.Content.ReadAsStringAsync();
            resultDelete = JsonConvert.DeserializeObject<Result>(stringResult);

            return resultDelete;

        }

    }
}

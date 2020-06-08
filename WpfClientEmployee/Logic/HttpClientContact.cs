using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModelsApp.Employee;
using ModelsApp.Result;
using Newtonsoft.Json;

namespace WpfClientEmployee.Logic
{
    class HttpClientContact
    {
        string urlEmployeeApi = "http://localhost:802/api/Contact";
        public async Task<List<Contact>> getListContactAsyns(int employee_id)
        {
            ResultApiContact resultApiContact = new ResultApiContact();
            List<Contact> lstContacts = new List<Contact>();

            HttpClient client = new HttpClient();

            //string req = urlEmployeeApi + "?employee_id=" + employee_id.ToString();
            string req = urlEmployeeApi + "/" + employee_id.ToString();

            HttpResponseMessage response = await client.GetAsync(req);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            resultApiContact = JsonConvert.DeserializeObject<ResultApiContact>(responseBody);

            if (resultApiContact.code >= 0)
            {
                lstContacts = resultApiContact.lstContact;
            }

            return lstContacts;           

        }


        public async Task<ResultApiContact> createNewContactAsyns(Contact contact)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(urlEmployeeApi);
            var contentJson = JsonConvert.SerializeObject(contact);
            var data = new StringContent(contentJson, Encoding.UTF8, "application/json");


            var responce = await httpClient.PostAsync(httpClient.BaseAddress, data);
            string resultStr = responce.Content.ReadAsStringAsync().Result;
            ResultApiContact resultApi = (ResultApiContact)JsonConvert.DeserializeObject(resultStr, typeof(ResultApiContact));
            return resultApi;
        }

        public void getContactAsyns(int employee_id)
        {
            throw new Exception();
        }

        public void changeContactAsyns(Contact contact)
        {
            throw new Exception();
        }

        public async Task<Result> deleteContactAsyns(int contact_id)
        {
            Result resultDeleteContact = new Result();
            HttpClient httpClientContact = new HttpClient();

            string req = urlEmployeeApi + "/" + contact_id.ToString();

            HttpResponseMessage response = await httpClientContact.DeleteAsync(req);

            string stringResult = await response.Content.ReadAsStringAsync();
            resultDeleteContact = JsonConvert.DeserializeObject<Result>(stringResult);
            return resultDeleteContact;
        }

    }
}

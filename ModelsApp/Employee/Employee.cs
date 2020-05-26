using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelsApp.Employee
{
    public class Employee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), JsonProperty("employee_id")]
        public int employee_id { get; set; }

        [JsonProperty("surname")]
        public string surname { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("middle_name")]
        public string middle_name { get; set; }

        [JsonProperty("birthday")]
        public DateTime? birthday { get; set; }
    }
}

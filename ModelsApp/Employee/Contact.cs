using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsApp.Employee
{
    public class Contact
    {
        public int contact_id { get; set; }
        public int employee_id { get; set; }
        public int contact_type { get; set; }
        public string content { get; set; }
        public string comment { get; set; }
        public int verify { get; set; }
    }
}

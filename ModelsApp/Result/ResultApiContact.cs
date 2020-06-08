using ModelsApp.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsApp.Result
{
    public class ResultApiContact : Result
    {
        public List<Contact> lstContact { get; set; }
        public Contact contact { get; set; }
    }
}

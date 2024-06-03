using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace student_management_system.Models
{
    public class AddWorkshop
    {
        public int id { get; set; }
        public int collegename { get; set;}
        public DateTime workshopdate { get; set;}
        public string remark { get; set;}

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace student_management_system.Models
{
    public class WorkshopData
    {
        public int id {  get; set; }
        public int collage { get; set; }
        public string studentname { get; set; }
        public string branch { get; set;}
        public string year { get; set; }
        public long mobno { get; set; }
        public string email { get; set; }

    }
}
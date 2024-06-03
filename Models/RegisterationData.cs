using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace student_management_system.Models
{
    public class RegisterationData
    {
        public int id {  get; set; }
        public string studentname { get; set; }
        public string course { get; set; }
        public string year { get; set; }
        public long mobno { get; set; }
        public string email { get; set; }
        public int collage { get; set; }
        public int training { get; set; }
        public int fee { get; set; }
        public int discount { get; set; }
        public int finalfee { get; set; }
        public int regfee { get; set; }
        public int remaining { get; set;}
        public string mode { get; set; }
        public int di {  get; set; }
    }
}
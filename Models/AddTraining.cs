using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace student_management_system.Models
{
    public class AddTraining
    {
        public int id {  get; set; }
        public string trainingname { get; set; }
        public string trainingcode { get; set; }
        public int trainingfee {  get; set; }
        public int fromyear {  get; set; }
        public int toyear { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace student_management_system.Models
{
    public class enrollData
    {
        //enroll
        public int id {  get; set; }
        public DateTime date { get; set; }
        public int training {  get; set; }
        public string enrollmode { get; set; }
        public string enrollid {  get; set; }

        //basic details

        public string studentname { get; set; }
        public string gname { get; set; }
        public int collage { get; set; }
        public string course { get; set; }
        public string year { get; set; }
        public string email {  get; set; }
        public long mobno {  get; set; }
        public long gmobno { get; set; }
        public long adharnum { get; set; }
        public string address {  get; set; }

        //fee details

        public int totalfee { get; set; }
        public int discount { get; set; }
        public int finalfee { get; set; }
        public int subfee { get; set; }
        public string feemode { get; set; }
        public DateTime feedate { get; set; }
        public int remaining { get; set; }
        public DateTime nextinstdate { get; set; }
    }
}
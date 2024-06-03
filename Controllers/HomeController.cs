using OfficeOpenXml;
using project1_online_selling.Models;
using student_management_system.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using System.Web.Security;

namespace student_management_system.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        DBLayer db=new DBLayer();  //DBLayer Object

        //------------------Dashboard Page--------------------
      public ActionResult Index()
        {
            DataTable dt = db.ExecuteSelect("sp_dashboard",new SqlParameter[] {new SqlParameter("@action",1)});
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return View(ds);
        }

        //------------------------Login Page-------------------
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost,AllowAnonymous]
        public ActionResult Login(string email,string password)
        {
            if(email!=null&& password!=null)
            {
                if (email.Equals("rudrasingh400101@gmail.com") && password.Equals("12345"))
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                }
                else
                {
                    return Content("<script>alert('Details Not Matched');location.href='/home/login'</script>");
                }
            }

            return Content($"<script>location.href='{Request.QueryString["ReturnUrl"]}'</script>");
        }

        //-----------------------------logout-------------------------------------------
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("login");
        }

        //--------------------------------AddCollege Page-------------------------------

        public ActionResult AddCollege()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCollege(AddCollege data)
        {
            if (data.remark != null)
            {
                //if remark has value

                SqlParameter[] prms = new SqlParameter[]
                {
                new SqlParameter("@collegename",data.collegename),
                new SqlParameter("@collegecode",data.collegecode),
                new SqlParameter("@remark",data.remark),
                new SqlParameter("action",1)
                };
                int res = db.ExecuteDML("sp_college", prms);
                if (res > 0)
                {
                    return Content("<script>alert('Record Added Successfully');location.href='/home/AddCollege'</script>");

                }
            }
            else
            {
                //if remark does not cotain any value

                SqlParameter[] prms = new SqlParameter[] {
                new SqlParameter("@collegename", data.collegename),
                new SqlParameter("@collegecode", data.collegecode),
                new SqlParameter("action", 1)
                };
                int res = db.ExecuteDML("sp_college", prms);
                if (res > 0)
                {
                    return Content("<script>alert('Record Added Successfully');location.href='/home/AddCollege'</script>");

                }
            }
            return RedirectToAction("AddCollege");
        }

        //-----------------------------Add Training---------------------------
        public ActionResult AddTraining()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTraining(AddTraining data)
        {
            SqlParameter[] prms = new SqlParameter[]
               {
                new SqlParameter("@trainingname",data.trainingname),
                new SqlParameter("@Trainingcode",data.trainingcode),
                new SqlParameter("@trainingfee",data.trainingfee),
                new SqlParameter("@fromyear",data.fromyear),
                new SqlParameter("@toyear",data.toyear),
                new SqlParameter("action",1)
               };
            int res = db.ExecuteDML("sp_training", prms);
            if (res > 0)
            {
                return Content("<script>alert('Record Added Successfully');location.href='/home/AddTraining'</script>");

            }
            return RedirectToAction("AddTraining");

        }

        //---------------------------Add Workshop ------------------------------
        public ActionResult AddWorkshop()
        {
            //select collage name
            DataTable dt = db.ExecuteSelect("sp_college", new SqlParameter[] { new SqlParameter("@action", 2) });

            return View(dt);
        }

        [HttpPost]
        public ActionResult AddWorkshop(AddWorkshop data)
        {
            //with remark
            if (data.remark != null)
            {
                SqlParameter[] prms = new SqlParameter[]
                  {
                new SqlParameter("@collegename",data.collegename),
                new SqlParameter("@workshopdate",data.workshopdate),
                new SqlParameter("@remark",data.remark),
                new SqlParameter("action",1)
                  };
                int res = db.ExecuteDML("sp_workshop", prms);
                if (res > 0)
                {
                    return Content("<script>alert('Record Added Successfully');location.href='/home/AddWorkshop'</script>");

                }
            }
            else
            {//without remark
                SqlParameter[] prms = new SqlParameter[]
                 {
                new SqlParameter("@collegename",data.collegename),
                new SqlParameter("@workshopdate",data.workshopdate),
                new SqlParameter("action",1)
                 };
                int res = db.ExecuteDML("sp_workshop", prms);
                if (res > 0)
                {
                    return Content("<script>alert('Record Added Successfully');location.href='/home/AddWorkshop'</script>");

                }
            }
            return RedirectToAction("AddWorkshop");

        }
        //---------------------------------Add Workshop datas--------------------------------------
        public ActionResult AddWorkshopDatas()
        {
            //get college name,workshop Id,date
            DataTable dt = db.ExecuteSelect("sp_workshop", new SqlParameter[] { new SqlParameter("@action", 2) });

            return View(dt);
        }

        [HttpPost]
        public JsonResult AddWorkshopRecords(WorkshopData data)
        {
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@workshop",data.collage),
                new SqlParameter("@studentname",data.studentname),
                new SqlParameter("@branch",data.branch),
                new SqlParameter("@year",data.year),
                new SqlParameter("@mobno",data.mobno),
                new SqlParameter("@email",data.email),
                new SqlParameter("action",1)
            };
            object res = db.ExecuteDML("sp_workshopData", prms);
            return Json(res.ToString());
        }

        //--------------------------------Add Registeration-----------------------------------
        public ActionResult AddRegisteration()
        {
            DataTable dt = db.ExecuteSelect("sp_college", new SqlParameter[] { new SqlParameter("@action", 2) });  //collage name
            DataTable dt2 = db.ExecuteSelect("sp_training", new SqlParameter[] { new SqlParameter("@action", 2) }); //training name

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.Tables.Add(dt2);
            return View(ds);
        }
        //Registeration form for indirect student
        public ActionResult AddRegisterationIndirect(int? id)
        {
            if (id.HasValue)
            {
                DataTable dt = db.ExecuteSelect("sp_college", new SqlParameter[] { new SqlParameter("@action", 2) });  //collage name
                DataTable dt2 = db.ExecuteSelect("sp_training", new SqlParameter[] { new SqlParameter("@action", 2) }); //training name
                DataTable dt3 = db.ExecuteSelect("sp_workshopData", new SqlParameter[] { new SqlParameter("@action", 4), new SqlParameter("@id", id) });
                DataSet ds2 = new DataSet();
                ds2.Tables.Add(dt);
                ds2.Tables.Add(dt2);
                ds2.Tables.Add(dt3);

                if (dt.Rows.Count > 0 && dt2.Rows.Count > 0 && dt3.Rows.Count > 0)
                {
                    return View(ds2);
                }
                else
                {
                    return RedirectToAction("workshopmanagement");
                }
            }
            else
            {
               return RedirectToAction("workshopmanagement");
            }

        }

        //get training fee using ajax
        public ActionResult Trainingfee(int? id)
        {
            if (id.HasValue)
            {
                //select training fee
                DataTable dt = db.ExecuteSelect("sp_training", new SqlParameter[] { new SqlParameter("@id", id), new SqlParameter("@action", 3) });
                if (dt.Rows.Count > 0)
                {
                    int trainingfee = Convert.ToInt32(dt.Rows[0]["trainingfee"]);
                    return Json(trainingfee, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json("Record Not Found");
                }
            }
            else
            {
                return RedirectToAction("AddRegisteration");
            }
        }
        [HttpPost]
        public ActionResult AddRegisteration(RegisterationData data)
        {
            SqlParameter[] prms = new SqlParameter[] { 
            
                new SqlParameter("@studentname",data.studentname),
                new SqlParameter("@course",data.course),
                new SqlParameter("@year",data.year),
                new SqlParameter("@mobno",data.mobno),
                new SqlParameter("@email",data.email),
                new SqlParameter("@collage",data.collage),
                new SqlParameter("@training",data.training),
                new SqlParameter("@trainingfee",data.fee),
                new SqlParameter("@discount",data.discount),
                new SqlParameter("@finalfee",data.finalfee),
                new SqlParameter("@regfee",data.regfee),
                new SqlParameter("@remaining",data.remaining),
                new SqlParameter("@mode",data.mode),
                new SqlParameter("@action",1),
                new SqlParameter("@blank",2)
            };
            if (data.di!=0)
            {
                prms[14] = new SqlParameter("@di",data.di);
            }
            int res = db.ExecuteDML("sp_registeration", prms);
            if (res > 0)
            {
                return Content("<script>alert('Record Added Successfully');location.href='/home/AddRegisteration'</script>");

            }
            else
            {
                return Content("<script>alert('Error While Adding Records');location.href='/home/AddRegisteration'</script>");
            }
        }

        //---------------------------------Add Enrollment----------------------------------------------
        public ActionResult AddEnrollment()
        {

            DataTable dt = db.ExecuteSelect("sp_college", new SqlParameter[] { new SqlParameter("@action", 2) });  //collage name
            DataTable dt2 = db.ExecuteSelect("sp_training", new SqlParameter[] { new SqlParameter("@action", 2) }); //training name

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.Tables.Add(dt2);
            return View(ds);
        }

        //get unique Id for Enroll Id
        public ActionResult EnrollId(int? id)
        {
            object res = db.ExecuteScalar("sp_training", new SqlParameter[] { new SqlParameter("@action", 4),new SqlParameter("@id",id) });

            return Json(res.ToString(),JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEnrollment(enrollData d)
        {
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@date",d.date),
                new SqlParameter("@training",d.training),
                new SqlParameter("@enrollmode",d.enrollmode),
                new SqlParameter("@enrollid",d.enrollid),
                new SqlParameter("@studentname",d.studentname),
                new SqlParameter("@course",d.course),
                new SqlParameter("@year",d.year),
                new SqlParameter("@guardian",d.gname),
                new SqlParameter("@collage",d.collage),
                new SqlParameter("@email",d.email),
                new SqlParameter("@mobno",d.mobno),
                new SqlParameter("@gmobno",d.gmobno),
                new SqlParameter("@adharnumber",d.adharnum),
                new SqlParameter("@address",d.address),
                new SqlParameter("@totalfee",d.totalfee),
                new SqlParameter("@discount",d.discount),
                new SqlParameter("@finalfee",d.finalfee),
                new SqlParameter("@subfee",d.subfee),
                new SqlParameter("@feemode",d.feemode),
                new SqlParameter("@feedate",d.feedate),
                new SqlParameter("@remaining",d.remaining),
                new SqlParameter("@action",1),
                new SqlParameter("@blank",1)
            };
            if (DateTime.Compare(d.nextinstdate.Date,d.feedate.Date)>=0)
            {
                prms[22] = new SqlParameter("@nextInstallment", d.nextinstdate);
            }

            int res = db.ExecuteDML("sp_enroll", prms);
            if (res > 0)
            {
                return Content("<script>alert('Record Added Successfully');location.href='/home/AddEnrollment'</script>");

            }
            else
            {
                return Content("<script>alert('Error While Adding Records. may be email already registered');location.href='/home/AddEnrollment'</script>");
            }
        }

        //---------------Add Enrollment by registeration management-------------------

        public ActionResult AddEnrollmentByRegmng(int? id)
        {
            if (id.HasValue)
            {

                DataTable dt = db.ExecuteSelect("sp_college", new SqlParameter[] { new SqlParameter("@action", 2) });  //collage name
                DataTable dt2 = db.ExecuteSelect("sp_training", new SqlParameter[] { new SqlParameter("@action", 2) }); //training name
                DataTable dt3 = db.ExecuteSelect("sp_registeration", new SqlParameter[] { new SqlParameter("@action", 4), new SqlParameter("@id", id) });   //select registered student records according to id


                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ds.Tables.Add(dt2);
                ds.Tables.Add(dt3);

                return View(ds);

            }
            else
            {
                return RedirectToAction("Registerationmanagement");
            }
        }

        [HttpPost]
        public ActionResult AddEnrollmentByRegmng(enrollData d)
        {
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@date",d.date),
                new SqlParameter("@training",d.training),
                new SqlParameter("@enrollmode",d.enrollmode),
                new SqlParameter("@enrollid",d.enrollid),
                new SqlParameter("@studentname",d.studentname),
                new SqlParameter("@course",d.course),
                new SqlParameter("@year",d.year),
                new SqlParameter("@guardian",d.gname),
                new SqlParameter("@collage",d.collage),
                new SqlParameter("@email",d.email),
                new SqlParameter("@mobno",d.mobno),
                new SqlParameter("@gmobno",d.gmobno),
                new SqlParameter("@adharnumber",d.adharnum),
                new SqlParameter("@address",d.address),
                new SqlParameter("@totalfee",d.totalfee),
                new SqlParameter("@discount",d.discount),
                new SqlParameter("@finalfee",d.finalfee),
                new SqlParameter("@subfee",d.subfee),
                new SqlParameter("@feemode",d.feemode),
                new SqlParameter("@feedate",d.feedate),
                new SqlParameter("@remaining",d.remaining),
                new SqlParameter("@action",1),
                new SqlParameter("@blank",1)
            };
            if (DateTime.Compare(d.nextinstdate.Date, d.feedate.Date) >= 0)
            {
                prms[22] = new SqlParameter("@nextInstallment", d.nextinstdate);
            }
            int res = db.ExecuteDML("sp_enroll", prms);
            if (res > 0)
            {
                return Content("<script>alert('Record Added Successfully');location.href='/home/AddEnrollment'</script>");

            }
            else
            {
                return Content("<script>alert('Error While Adding Records. may be email already registered');location.href='/home/AddEnrollment'</script>");
            }
        }

        //-------------------------------------workshop management------------------------------------------------------
        public ActionResult workshopmanagement(int? id,int? deleteid)
        {
            if (deleteid.HasValue)
            {
                int res = db.ExecuteDML("sp_workshopdata", new SqlParameter[] { new SqlParameter("@action", 5), new SqlParameter("@id", deleteid) });
                if (res > 0)
                {
                    return Content("<script>alert('Record Deleted Successfully');location.href='/home/workshopmanagement'</script>");

                }
                else
                {
                    return Content("<script>alert('Error While Deleting Records');location.href='/home/workshopmanagement'</script>");

                }
            }
            else
            {
                //get college name,workshop Id,date
                DataTable dt = db.ExecuteSelect("sp_workshop", new SqlParameter[] { new SqlParameter("@action", 2) });
                DataTable dt2;
                DataTable dt3 = db.ExecuteSelect("sp_registeration", new SqlParameter[] { new SqlParameter("@action", 2) });
                if (id.HasValue)
                {
                    dt2 = db.ExecuteSelect("sp_workshopdata", new SqlParameter[] { new SqlParameter("@action", 3), new SqlParameter("@id", id) });
                }
                else
                {
                    dt2 = db.ExecuteSelect("sp_workshopdata", new SqlParameter[] { new SqlParameter("@action", 2) });
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ds.Tables.Add(dt2);
                ds.Tables.Add(dt3);
                return View(ds);
            }
        }

        //---------------------------------------------REgisteration management---------------------------------
        public ActionResult RegisterationManagement(int? id,int? deleteid)
        {
            if (deleteid.HasValue)
            {
                int res = db.ExecuteDML("sp_registeration", new SqlParameter[] { new SqlParameter("@action", 5), new SqlParameter("@id", deleteid) });
                if (res > 0)
                {
                    return Content("<script>alert('Record Deleted Successfully');location.href='/home/RegisterationManagement'</script>");

                }
                else
                {
                    return Content("<script>alert('Error While Deleting Records');location.href='/home/RegisterationManagement'</script>");

                }
            }
            else
            {
                DataTable dt = db.ExecuteSelect("sp_training", new SqlParameter[] { new SqlParameter("@action", 2) }); //training name
                DataTable dt2;
                if (id.HasValue)
                {
                    dt2 = db.ExecuteSelect("sp_registeration", new SqlParameter[] { new SqlParameter("@action", 3), new SqlParameter("@id", id) }); //select Registeratioon data according to id

                }
                else
                {
                    dt2 = db.ExecuteSelect("sp_registeration", new SqlParameter[] { new SqlParameter("@action", 2) }); //select Registeratioon data

                }
                DataTable dt3 = db.ExecuteSelect("sp_enroll", new SqlParameter[] { new SqlParameter("@action", 2) });

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ds.Tables.Add(dt2);
                ds.Tables.Add(dt3);
                return View(ds);
            }
        }
        //----------------------------------invoice for register user-------------------------------------------------
        public ActionResult Invoice(int? id)
        {
            if (id.HasValue)
            {
                DataTable dt = db.ExecuteSelect("sp_registeration", new SqlParameter[] { new SqlParameter("@action",4),new SqlParameter("@id",id) });   //select registered student records according to id
                return View(dt.Rows[0]);
            }
            return RedirectToAction("registerationmanagement");
        }

        //----------------------------------collage management---------------------------------------------------------
        public ActionResult collageManagement()
        {
            DataTable dt = db.ExecuteSelect("sp_college", new SqlParameter[] { new SqlParameter("@action", 2) });
            return View(dt);
        }
        //delete collage
        public ActionResult deleteCollage(int? id)
        {
            if (id.HasValue)
            {
                int res = db.ExecuteDML("sp_college", new SqlParameter[] { new SqlParameter("@action", 3), new SqlParameter("@id", id) });
                if (res > 0)
                {
                    return Content("<script>alert('Record Deleted Successfully');location.href='/home/collageManagement'</script>");

                }
                else
                {
                    return Content("<script>alert('Error While Deleting Records');location.href='/home/collageManagement'</script>");

                }
            }
            else
            {
                return RedirectToAction("collageManagement");
            }
        }
        [HttpPost]
        public ActionResult editCollage(AddCollege data)
        {
            if (data.remark != null)
            {
                //if remark has value

                SqlParameter[] prms = new SqlParameter[]
                {
                new SqlParameter("@collegename",data.collegename),
                new SqlParameter("@collegecode",data.collegecode),
                new SqlParameter("@remark",data.remark),
                new SqlParameter("@id",data.id),
                new SqlParameter("action",4)
                };
                int res = db.ExecuteDML("sp_college", prms);
                if (res > 0)
                {
                    return Content("<script>alert('Record Updated Successfully');location.href='/home/collagemanagement'</script>");

                }
            }
            else
            {
                //if remark does not cotain any value

                SqlParameter[] prms = new SqlParameter[] {
                new SqlParameter("@collegename", data.collegename),
                new SqlParameter("@collegecode", data.collegecode),
                new SqlParameter("@id",data.id),
                new SqlParameter("action", 4)
                };
                int res = db.ExecuteDML("sp_college", prms);
                if (res > 0)
                {
                    return Content("<script>alert('Record Updated Successfully');location.href='/home/collagemanagement'</script>");

                }
            }
            return RedirectToAction("collagemanagement");
        }

        //------------------------------training management---------------------------------------
        public ActionResult trainingManagement(int? id)
        {
            if (id.HasValue)
            {
                int res = db.ExecuteDML("sp_training", new SqlParameter[] { new SqlParameter("@action", 5), new SqlParameter("@id", id) });
                if (res > 0)
                {
                    return Content("<script>alert('Record Deleted Successfully');location.href='/home/trainingManagement'</script>");

                }
                else
                {
                    return Content("<script>alert('Error While Deleting Records');location.href='/home/trainingManagement'</script>");

                }
            }
            else
            {
                DataTable dt = db.ExecuteSelect("sp_training", new SqlParameter[] { new SqlParameter("@action", 2) });
                return View(dt);
            }
        }

        [HttpPost]
        public ActionResult editTraining(AddTraining data)
        {
            SqlParameter[] prms = new SqlParameter[]
               {
                new SqlParameter("@trainingname",data.trainingname),
                new SqlParameter("@Trainingcode",data.trainingcode),
                new SqlParameter("@trainingfee",data.trainingfee),
                new SqlParameter("@fromyear",data.fromyear),
                new SqlParameter("@toyear",data.toyear),
                new SqlParameter("@id",data.id),
                new SqlParameter("action",6)
               };
            int res = db.ExecuteDML("sp_training", prms);
            if (res > 0)
            {
                return Content("<script>alert('Record Updated Successfully');location.href='/home/trainingManagement'</script>");

            }
            else
            {
                return Content("<script>alert('Error While Updating Records');location.href='/home/trainingManagement'</script>");

            }

        }

        //-----------------------------------workshop management---------------------------------------

        public ActionResult workshop(int? id)
        {
            if (id.HasValue)
            {
                int res = db.ExecuteDML("sp_workshop", new SqlParameter[] { new SqlParameter("@action",3),new SqlParameter("@id",id) });
                if (res > 0)
                {
                    return Content("<script>alert('Record Deleted Successfully');location.href='/home/workshop'</script>");

                }
                else
                {
                    return Content("<script>alert('Error While Deleting Records');location.href='/home/workshop'</script>");

                }
            }
            else
            {
                DataTable dt = db.ExecuteSelect("sp_college", new SqlParameter[] { new SqlParameter("@action", 2) });
                DataTable dt2 = db.ExecuteSelect("sp_workshop", new SqlParameter[] { new SqlParameter("@action", 2) });
                DataSet ds =new DataSet();
                ds.Tables.Add(dt);
                ds.Tables.Add(dt2);
                return View(ds);
            }

        }

        [HttpPost]
        public ActionResult editWorkshop(AddWorkshop data)
        {
            int res = 0;
            if (data.remark != null)
            {
                SqlParameter[] prms = new SqlParameter[]
                  {
                new SqlParameter("@collegename",data.collegename),
                new SqlParameter("@workshopdate",data.workshopdate),
                new SqlParameter("@remark",data.remark),
                new SqlParameter("@id",data.id),
                new SqlParameter("action",4)
                  };
                 res = db.ExecuteDML("sp_workshop", prms);
            }
        else{
                SqlParameter[] prms = new SqlParameter[]
                          {
                new SqlParameter("@collegename",data.collegename),
                new SqlParameter("@workshopdate",data.workshopdate),
                new SqlParameter("@id",data.id),
                new SqlParameter("action",4)
                          };
                 res = db.ExecuteDML("sp_workshop", prms);
            }
            if (res > 0)
            {
                return Content("<script>alert('Record Updated Successfully');location.href='/home/workshop'</script>");

            }
            else
            {
                return Content("<script>alert('Error While Updating Records');location.href='/home/workshop'</script>");

            }
        }

        //---------------------fee management-------------------------------

        public ActionResult feeManagement()
        {
            DataTable dt = db.ExecuteSelect("sp_enroll", new SqlParameter[] {new SqlParameter("@action",2)});
            return View(dt);
        }
        [HttpPost]
        public ActionResult updateFee(enrollData data)
        {
            SqlParameter[] prms = new SqlParameter[]
                          {
                new SqlParameter("@subfee",data.subfee),
                new SqlParameter("@feedate",data.feedate),
                new SqlParameter("@remaining",data.remaining),
                new SqlParameter("@feemode",data.feemode),
                new SqlParameter("@enrollid",data.enrollid),
                new SqlParameter("@action",3),
                new SqlParameter("@blank",3)
                          };
            if (data.nextinstdate >= data.feedate)
            {
                prms[6] = new SqlParameter("@nextinstallment",data.nextinstdate);
            }
            int res = db.ExecuteDML("sp_enroll", prms);
            if (res > 0)
            {
                return Content("<script>alert('Record Updated Successfully');location.href='/home/feeManagement'</script>");

            }
            else
            {
                return Content("<script>alert('Error While Updating Records');location.href='/home/feeManagement'</script>");

            }
        }
        //------------------------invoice for  fees management-----------------------
        public ActionResult feeInvoice(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                DataTable dt = db.ExecuteSelect("sp_enroll",new SqlParameter[] {new SqlParameter("@action",4),new SqlParameter("@enrollid",id)});
                return View(dt.Rows[0]);

            }
            else
            {
                return RedirectToAction("feemanagement");
            }

        }

        //--------------------------------------enrollment Management--------------------------------------

        public ActionResult enrollManagement(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                int res = db.ExecuteDML("sp_enroll", new SqlParameter[] { new SqlParameter("@action", 5),new SqlParameter("@enrollid",id) });
                if (res > 0)
                {
                    return Content("<script>alert('Record Deleted Successfully');location.href='/home/enrollManagement'</script>");

                }
                else
                {
                    return Content("<script>alert('Error While Deleting Records');location.href='/home/enrollManagement'</script>");

                }
            }

            DataTable dt = db.ExecuteSelect("sp_enroll",new SqlParameter[] {new SqlParameter("@action",2)});
            return View(dt);
        }
        [HttpPost]
        public ActionResult editEnroll(enrollData d)
        {
            SqlParameter[] prms = new SqlParameter[]
          {
              
                new SqlParameter("@enrollid",d.enrollid),
                new SqlParameter("@studentname",d.studentname),
                new SqlParameter("@course",d.course),
                new SqlParameter("@year",d.year),
                new SqlParameter("@guardian",d.gname),
                new SqlParameter("@collage",d.collage),
                new SqlParameter("@email",d.email),
                new SqlParameter("@mobno",d.mobno),
                new SqlParameter("@gmobno",d.gmobno),
                new SqlParameter("@adharnumber",d.adharnum),
                new SqlParameter("@address",d.address),
                new SqlParameter("@action",6),
          };
            int res = db.ExecuteDML("sp_enroll",prms);
            if (res > 0)
            {
                return Content("<script>alert('Record Updated Successfully');location.href='/home/enrollManagement'</script>");

            }
            else
            {
                return Content("<script>alert('Error While Updating Records');location.href='/home/enrollManagement'</script>");

            }
        }
        //-----------------------------fee history----------------------------------------
        public ActionResult FeeHistory(int? deleteid)
        {
            if(deleteid.HasValue)
            {
                int res = db.ExecuteDML("sp_feebackup", new SqlParameter[] { new SqlParameter("@action", 2), new SqlParameter("@id", deleteid) });
                if (res > 0)
                {
                    return Content("<script>alert('Record Deleted Successfully');location.href='/home/FeeHistory'</script>");

                }
                else
                {
                    return Content("<script>alert('Error While Deleting Records');location.href='/home/FeeHistory'</script>");

                }
            }
            else
            {
                DataTable dt = db.ExecuteSelect("sp_feebackup",new SqlParameter[] {new SqlParameter("@action",1)});
                return View(dt);

            }

        }

        
    }
}
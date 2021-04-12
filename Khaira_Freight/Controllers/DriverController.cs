using Khaira_Freight.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Khaira_Freight.Controllers
{
    [Authorize]
    public class DriverController : Controller
    {
        // GET: Driver
        private KhairaFreightEntities db = new KhairaFreightEntities();
        public ActionResult UserProfile()
        {
            int id = Convert.ToInt32(Session["EmpId"]);
            List<table_driver> driverList = db.table_driver.Where(x => x.driver_id.Equals(id)).ToList();

            return View(driverList);
        }

        public ActionResult LoginInfo()
        {
            int id = Convert.ToInt32(Session["EmpId"]);
            List<UserTable> LoginList = db.UserTables.Where(x => x.EmpId.Equals(id)).ToList();

            return View(LoginList);
        }
       
        public ActionResult Home()
        {
            return View();
        }


        //public ActionResult Home(int? id)
        //{
        //    table_driver table_driver = db.table_driver.Find(id);
        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    //table_driver table_driver = db.table_driver.Find(id);
        //    //if (table_driver == null)
        //    //{
        //    //    return HttpNotFound();
        //    //}
        //    //return View(table_driver);
        //    ViewBag.Name = table_driver.first_name + " " + table_driver.last_name;

        //    return View();
        //}

        //-------------------Full Calender--------------------------------------------------

        public ActionResult Calender(int id)
        {
            // Info.  
            ViewBag.para = id;
            return this.View();
        }

        public ActionResult GetCalendarData(int id)
        {
            // Initialization.  
            JsonResult result = new JsonResult();

            try
            {
                // Loading.  
                List<planner> data = this.LoadData(id);

                // Processing.  
                result = this.Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Return info.  
            return result;
        }



        /// <summary>  
        /// Load data method.  
        /// </summary>  
        /// <returns>Returns - Data</returns>  
        private List<planner> LoadData(int id)
        {
           
            // Initialization.  
            List<planner> plannerList = new List<planner>();
            try
            {
                plannerList = db.planners.Where(x => x.driver_id==id).ToList();

            }
            catch (Exception ex)
            {

                Console.Write(ex);
            }


            return plannerList;
        }


        //
        //--------------------------------Pay Statement
        public ActionResult PayStatement()
        {
            return View(GetPayFiles());
        }

        [HttpPost]
        public ActionResult PayStatement(HttpPostedFileBase postedFile, FormCollection coll)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            //
            int empId = Convert.ToInt32(coll["empid"]);
            string dept = coll["SelectDept"].ToString();
            //
            if (dept.Equals(""))
            {
                ViewBag.ErrorMessage = "Please select Department";

            }
            else
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "INSERT INTO pay_statement VALUES (@Name, @ContentType, @Data,@emp_id,@department)";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Name", Path.GetFileName(postedFile.FileName));
                        cmd.Parameters.AddWithValue("@ContentType", postedFile.ContentType);
                        cmd.Parameters.AddWithValue("@Data", bytes);
                        cmd.Parameters.AddWithValue("@emp_id", empId);
                        cmd.Parameters.AddWithValue("@department", dept);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    ViewBag.Success = "Upload Successful";
                }
            }


            return View(GetPayFiles());
        }

        [HttpPost]
        public FileResult DownloadPayFile(int? fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))

            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Name, Data, ContentType FROM pay_statement WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", fileId);

                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["Data"];
                        contentType = sdr["ContentType"].ToString();
                        fileName = sdr["Name"].ToString();
                    }
                    con.Close();
                }
            }

            return File(bytes, contentType, fileName);
        }
        //private static List<pay_statement> GetPayFiles()

        private List<pay_statement> GetPayFiles()
        {
            List<pay_statement> files = new List<pay_statement>();

            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Id, Name,emp_id,department FROM pay_statement WHERE emp_id=@empid";
                    int id = Convert.ToInt32(Session["EmpId"]);
                    cmd.Parameters.AddWithValue("@empid", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            files.Add(new Models.pay_statement
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Name = sdr["Name"].ToString(),
                                emp_id = Convert.ToInt32(sdr["emp_id"]),
                                department = sdr["department"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return files;
        }
        //--------------------------------pay

        //-------------------Full Calender--------------------------------------------------
        public ActionResult EditProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            table_driver table_driver = db.table_driver.Find(id);
            //

            List<SelectListItem> provinceList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Alberta - AB" ,Value="AB" },
                new SelectListItem() {Text="British Columbia - BC" ,Value="BC" },
                new SelectListItem() {Text="Manitoba - MB" ,Value="MB" },
                new SelectListItem() {Text="New Brunswick - NB" ,Value="NB" },
                new SelectListItem() {Text="Newfoundland and Labrador - NL" ,Value="NL" },
                new SelectListItem() {Text="Northwest Territories - NT" ,Value="NT" },
                new SelectListItem() {Text="Nova Scotia - NS" ,Value="NS" },
                new SelectListItem() {Text="Nunavut - NU" ,Value="NU" },
                new SelectListItem() {Text="Ontario - ON" ,Value="ON" },
                new SelectListItem() {Text="Prince Edward Island - PE" ,Value="PE" },
                new SelectListItem() {Text="Quebec - QC" ,Value="QC" },
                new SelectListItem() {Text="Saskatchewan - SK" ,Value="SK" },
                  new SelectListItem() {Text="Yukon - YT" ,Value="YT" }
            };

            table_driver.provinceList = provinceList;

            List<SelectListItem> genderList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Male - M" ,Value="M" },
                new SelectListItem() {Text="Female - F" ,Value="F" }
            };

            table_driver.genderList = genderList;

            //
            if (table_driver == null)
            {
                return HttpNotFound();
            }
            return View(table_driver);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "driver_id,first_name,last_name,address,postal_code,city,province,corporation,phone,dob,nationality,gender,pp_number,license_number,license_expiry,medical_date,rate,rate_team")] table_driver table_driver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(table_driver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserProfile");
            }
            return View(table_driver);
        }



    }
}
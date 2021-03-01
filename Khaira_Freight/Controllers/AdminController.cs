using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security;
using Khaira_Freight.Models;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace Khaira_Freight.Controllers
{
    public class AdminController : Controller
    {
        private KhairaFreightEntities db = new KhairaFreightEntities();

        //View Driver List
        public ActionResult Index()
        {
            return View(db.table_driver.ToList());
        }

        public ActionResult Admin()
        {
            ViewBag.Message = "Your admin page.";
            return View();
        }

        //View Driver Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            table_driver table_driver = db.table_driver.Find(id);
            if (table_driver == null)
            {
                return HttpNotFound();
            }
            return View(table_driver);
        }


        //Add Driver
        public ActionResult Create()
        {
            table_driver obj = new table_driver();
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

            obj.provinceList = provinceList;

            List<SelectListItem> genderList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Male - M" ,Value="M" },
                new SelectListItem() {Text="Female - F" ,Value="F" }
            };

            obj.genderList = genderList;

            return View(obj);
            //return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "driver_id,first_name,last_name,address,postal_code,city,province,corporation,phone,dob,nationality,gender,pp_number,license_number,license_expiry,medical_date,rate,rate_team")] table_driver table_driver)
        {
            if (ModelState.IsValid)
            {

                db.table_driver.Add(table_driver);
                db.SaveChanges();
                return RedirectToAction("Index");
               
            }

            return View(table_driver);
        }


        //Edit Driver 
        public ActionResult Edit(int? id)
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
        public ActionResult Edit([Bind(Include = "driver_id,first_name,last_name,address,postal_code,city,province,corporation,phone,dob,nationality,gender,pp_number,license_number,license_expiry,medical_date,rate,rate_team")] table_driver table_driver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(table_driver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table_driver);
        }


        //Delete Driver
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed.Try again.";
            }
            //
            table_driver table_driver = db.table_driver.Find(id);
            if (table_driver == null)
            {
                return HttpNotFound();
            }
            return View(table_driver);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                table_driver table_driver = db.table_driver.Find(id);
                db.table_driver.Remove(table_driver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException)
            {

                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }
        //
        public ActionResult IndexLogin()
        {
            return View(db.employee_login.ToList());
        }
        public ActionResult AddLogin()
        {
            employee_login obj = new employee_login();

            List<SelectListItem> departmentList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Driver" ,Value="Driver" },
                new SelectListItem() {Text="Dispatch" ,Value="Dispatch" }

            };
            obj.departmentList = departmentList;

            return View(obj);
            //return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLogin(employee_login obj)
        {
            if (ModelState.IsValid)
            {

           
                db.employee_login.Add(obj);
                db.SaveChanges();
                return RedirectToAction("IndexLogin");
              
            }

            return View(obj);
        }

        // 
        public ActionResult EditLogin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
  
            employee_login obj = db.employee_login.Find(id);

            List<SelectListItem> departmentList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Driver" ,Value="Driver" },
                new SelectListItem() {Text="Dispatch" ,Value="Dispatch" }

            };
            obj.departmentList = departmentList;

            //
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLogin(employee_login obj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexLogin");
            }
            return View(obj);
        }

        public ActionResult DeleteLogin(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed.Try again.";
            }
            //
            employee_login obj = db.employee_login.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        [HttpPost, ActionName("DeleteLogin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLogin(int id)
        {
            try
            {
                employee_login obj = db.employee_login.Find(id);
                db.employee_login.Remove(obj);
                db.SaveChanges();
                return RedirectToAction("IndexLogin");
            }
            catch (DataException)
            {

                return RedirectToAction("DeleteLogin", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("IndexLogin");
        }

        public ActionResult DetailsLogin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            employee_login obj = db.employee_login.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }

        //
        //public ActionResult GenerateUName(String role )
        //{

        //  //  string st = employee_Login.first_name;

        //    Random rnd = new Random();
        //int randomNumber = rnd.Next(1, 100);
        //  //  obj.username = st.Append(randomNumber.ToString());
        //    var username = role + randomNumber;

        //    return RedirectToAction("IndexLogin");
        //}

        //

        //--------------------DISPATCH--------------------------------------
        //View Dispatch List
        public ActionResult IndexDispatch()
        {
            return View(db.dispatches.ToList());
        }


        //View Dispatch Details
        public ActionResult DispatchDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
      
            dispatch obj = db.dispatches.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        //Add Dispatch
        public ActionResult CreateDispatch()
        {
           
            dispatch obj = new dispatch();
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

            obj.provinceList = provinceList;

            List<SelectListItem> genderList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Male - M" ,Value="M" },
                new SelectListItem() {Text="Female - F" ,Value="F" }
            };

            obj.genderList = genderList;

            return View(obj);
            //return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDispatch(dispatch obj)
        {
            if (ModelState.IsValid)
            {

                db.dispatches.Add(obj);
                db.SaveChanges();
                return RedirectToAction("IndexDispatch");

            }

            return View(obj);
        }

        public ActionResult EditDispatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
            dispatch obj = db.dispatches.Find(id);
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

            obj.provinceList = provinceList;

            List<SelectListItem> genderList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Male - M" ,Value="M" },
                new SelectListItem() {Text="Female - F" ,Value="F" }
            };

            obj.genderList = genderList;

            //
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDispatch(dispatch obj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexDispatch");
            }
            return View(obj);
        }


        public ActionResult DeleteDispatch(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed.Try again.";
            }
            //
            table_driver table_driver = db.table_driver.Find(id);
            dispatch obj = db.dispatches.Find(id);
            if (table_driver == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        [HttpPost, ActionName("DeleteDispatch")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDispatch(int id)
        {
            try
            {
                dispatch obj = db.dispatches.Find(id);
                db.dispatches.Remove(obj);
                db.SaveChanges();
                return RedirectToAction("IndexDispatch");
            }
            catch (DataException)
            {

                return RedirectToAction("DeleteDispatch", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("IndexDispatch");
        }
        //------------------------DISPATCH----------------------------------
        //PAY-----------------------------------------------------------
        public ActionResult ManagePay()
        {
            return View(GetFiles());
        }

        [HttpPost]
        public ActionResult ManagePay(HttpPostedFileBase postedFile, FormCollection coll)
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

        private static List<pay_statement> GetPayFiles()
        {
            List<pay_statement> files = new List<pay_statement>();

            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name,emp_id,department FROM pay_statement"))
                {
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
        //PAY-----------------------------------------------------------

        //--------------------------------driver application
        public ActionResult DriverApplication()
        {
            return View(GetFiles());
        }
        [HttpPost]
        public ActionResult DriverApplication(HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                //string insertSQL = "INSERT INTO driver_application VALUES (@Col1) ";
                string query = "INSERT INTO driver_application VALUES (@Name, @ContentType, @Data)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Name", Path.GetFileName(postedFile.FileName));
                    cmd.Parameters.AddWithValue("@ContentType", postedFile.ContentType);
                    cmd.Parameters.AddWithValue("@Data", bytes);
                    //SQLCommand myCmd = new SQLCommand(insertSQL, con);
                    //cmd.Parameters.AddWithValue("@Col1", TextBox1.Text.trim());
                    Response.Write("Record Inserted...");



                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return View(GetFiles());
        }


        //public ActionResult DriverApplication()
        //{
        //    return View();
        //}
        [HttpPost]
        public FileResult DownloadFile(int? fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Name, Data, ContentType FROM driver_application WHERE Id=@Id";
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

        private static List<driver_application> GetFiles()
        {
            List<driver_application> files = new List<driver_application>();
            string constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM driver_application"))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            files.Add(new driver_application
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Name = sdr["Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return files;
        }
		//--------------------------------driver application


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

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
using PagedList;
using System.Text.RegularExpressions;

namespace Khaira_Freight.Controllers
{
   [Authorize]
    public class AdminController : Controller
    {
        private KhairaFreightEntities db = new KhairaFreightEntities();

        //View Driver List

        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "first_name_asc" : "";
            ViewBag.LastNameSortParm = sortOrder == "last_name" ? "last_name_desc" : "last_name";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var drivers = from s in db.table_driver
                        select s;
          
            if (!String.IsNullOrEmpty(searchString))
            {
                drivers = drivers.Where(s => s.first_name.Contains(searchString)
                                       || s.last_name.Contains(searchString)
                                       || s.license_number.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "first_name_asc":
                    drivers = drivers.OrderBy(s => s.first_name);

                    break;
                case "last_name":
                    drivers = drivers.OrderBy(s => s.last_name);
                    break;
                case "last_name_desc":
                    drivers = drivers.OrderByDescending(s => s.last_name);
                    break;
                default:
                    drivers = drivers.OrderBy(s => s.driver_id);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(drivers.ToPagedList(pageNumber, pageSize));
           // return View(db.table_driver.ToList());
        }
       
        public ActionResult Admin()
        {
            ViewBag.Message = "Your admin page.";
            return View();
        }
        public ActionResult Dashboard()
        {
            var driver = db.table_driver.Count();
            ViewBag.driver = driver;
            var dispatcher = db.dispatches.Count();
            ViewBag.dispatcher = dispatcher;
            ViewBag.Message = "Your admin dashboard page.";
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




        public ActionResult IndexLogin(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmpNameSortParm = String.IsNullOrEmpty(sortOrder) ? "EmpName_asc" : "";
            ViewBag.UsernameSortParm = sortOrder == "Username" ? "Username_desc" : "Username";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var users = from s in db.UserTables
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.EmpName.Contains(searchString)
                                       || s.Username.Contains(searchString)
                                       || s.RoleTable.RoleName.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "EmpName_asc":
                    users = users.OrderBy(s => s.EmpName);
        
                    break;
                case "Username":
                    users = users.OrderBy(s => s.Username);
                    break;
                case "Username_desc":
                    users = users.OrderByDescending(s => s.Username);
                    break;
                default:
                    users = users.OrderBy(s => s.UserId);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));

            //return View(db.UserTables.ToList());
        }
        public ActionResult AddLogin()
        {
            UserTable obj = new UserTable();

            List<SelectListItem> roleList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Dispatch" ,Value="2" },
                new SelectListItem() {Text="Driver" ,Value="3" }

            };
            obj.roleList = roleList;

            List<SelectListItem> emptyList = new List<SelectListItem>();
            //{
            //    new SelectListItem() {Text="Select value" ,Value=" " }
            //};
            

            ViewBag.empList = db.table_driver.Select(x=>x.driver_id).ToList(); 
            
            obj.CreatedDate = DateTime.Now;

            return View(obj);
            //return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLogin(UserTable obj)

        {
            List<SelectListItem> roleList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Dispatch" ,Value="2" },
                new SelectListItem() {Text="Driver" ,Value="3" }

            };
            obj.roleList = roleList;
            //
            if (obj.Password != null)
            {
                string pass = obj.Password.ToString();
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,20}";
                bool passValid = Regex.IsMatch(pass, pattern);
                if (passValid == false)
                {
                    ModelState.AddModelError("Password", "Password must contain atleast 1 lowercase aplhabet,1 uppercase alphabet,1 numeric character,1 special character(!@#$%^&*) and must be atleast 8 characters or longer");
                }
            }
            //
            if(obj.Username!=null)
            {
                bool IsUserExists = db.UserTables.Any
                         (x => x.Username == obj.Username);
                if (IsUserExists == true)
                {
                    ModelState.AddModelError("Username", "User Name already exists");
                }
            }
            if (obj.EmpId != null)
            {
                bool IsEmpExists = db.UserTables.Any
         (x => x.EmpId == obj.EmpId);
                if (IsEmpExists == true)
                {
                    ModelState.AddModelError("EmpId", "Employee ID already exists");
                }
            }
            if (obj.RoleId != null)
            {
                if (obj.RoleId == 2)
                {
                    bool IsDispatcherExists = db.dispatches.Any(x => x.dispatch_id == obj.EmpId);
                    if (IsDispatcherExists == false)
                    {
                        ModelState.AddModelError("EmpId", "Invalid ! Dispatcher does not exist in Database");
                    }

                }
                if (obj.RoleId == 3)
                {
                    bool IsDriverExists = db.table_driver.Any(x => x.driver_id == obj.EmpId);
                    if (IsDriverExists == false)
                    {
                        ModelState.AddModelError("EmpId", "Invalid ! Driver does not exist in Database");
                    }
                }
            }

                


            if (ModelState.IsValid)
            {
                obj.RoleId = Convert.ToInt32(obj.RoleId);

                db.UserTables.Add(obj);
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
  
            UserTable obj = db.UserTables.Find(id);

            List<SelectListItem> roleList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Dispatch" ,Value="2" },
                new SelectListItem() {Text="Driver" ,Value="3" }

            };
            obj.roleList = roleList;
            obj.CreatedDate = DateTime.Now;

            //
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLogin(UserTable obj)
        {
            List<SelectListItem> roleList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Dispatch" ,Value="2" },
                new SelectListItem() {Text="Driver" ,Value="3" }

            };
            obj.roleList = roleList;
            if (obj.RoleId != null)
            {
                if (obj.RoleId == 2)
                {
                    bool IsDispatcherExists = db.dispatches.Any(x => x.dispatch_id == obj.EmpId);
                    if (IsDispatcherExists == false)
                    {
                        ModelState.AddModelError("EmpId", "Invalid ! Dispatcher does not exist in Database");
                    }

                }
                if (obj.RoleId == 3)
                {
                    bool IsDriverExists = db.table_driver.Any(x => x.driver_id == obj.EmpId);
                    if (IsDriverExists == false)
                    {
                        ModelState.AddModelError("EmpId", "Invalid ! Driver does not exist in Database");
                    }
                }
            }
            //
            if (obj.Password!=null)
            {
                string pass = obj.Password.ToString();
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,20}";
                bool passValid = Regex.IsMatch(pass, pattern);
                if (passValid == false)
                {
                    ModelState.AddModelError("Password", "Password must contain atleast 1 lowercase aplhabet,1 uppercase alphabet,1 numeric character,1 special character(!@#$%^&*) and must be atleast 8 characters or longer");
                }
            }
            //

            if (ModelState.IsValid)
            {
                obj.RoleId = Convert.ToInt32(obj.RoleId);
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
            UserTable obj = db.UserTables.Find(id);
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
                UserTable obj = db.UserTables.Find(id);
                db.UserTables.Remove(obj);
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
           
            UserTable obj = db.UserTables.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }

        //
       
        //

        //--------------------DISPATCH--------------------------------------
        //View Dispatch List
        public ActionResult IndexDispatch(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "first_name_asc" : "";
            ViewBag.LastNameSortParm = sortOrder == "last_name" ? "last_name_desc" : "last_name";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var dispatchers = from s in db.dispatches
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                dispatchers = dispatchers.Where(s => s.first_name.Contains(searchString)
                                       || s.last_name.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "first_name_asc":
                    dispatchers = dispatchers.OrderBy(s => s.first_name);

                    break;
                case "last_name":
                    dispatchers = dispatchers.OrderBy(s => s.last_name);
                    break;
                case "last_name_desc":
                    dispatchers = dispatchers.OrderByDescending(s => s.last_name);
                    break;
                default:
                    dispatchers = dispatchers.OrderBy(s => s.dispatch_id);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(dispatchers.ToPagedList(pageNumber, pageSize));
           // return View(db.dispatches.ToList());
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
         
            dispatch obj = db.dispatches.Find(id);
            if (obj == null)
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
            return View(GetPayFiles());
        }

        [HttpPost]
        public ActionResult ManagePay(HttpPostedFileBase postedFile, FormCollection coll)
        {
            if (postedFile!=null)
            {
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                }
                string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                string dept="";
                int empId=0;
                //
                if (coll["empid"]!=null)
                {
                     empId = Convert.ToInt32(coll["empid"]);
                }
                if(coll["SelectDept"]!=null)
                {
                      dept = coll["SelectDept"].ToString();
                }
                
               
                //
                if (dept.Equals(""))
                {
                    ViewBag.ErrorMessage1 = "Please select Department";

                }
                else
                {
                    if(dept.Equals("Driver"))
                    {
                        bool IsDriverExists = db.table_driver.Any
        (x => x.driver_id == empId);
                        if (IsDriverExists == false)
                        {
                            ViewBag.ErrorMessage3 = "Invalid ! Driver does not exist in Database";

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
                    }
                    if (dept.Equals("Dispatch"))
                    {
                        bool IsDispacherExists = db.dispatches.Any
        (x => x.dispatch_id == empId);
                        if (IsDispacherExists == false)
                        {
                            ViewBag.ErrorMessage3 = "Invalid ! Dispacher does not exist in Database";

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
                    }
                   
                }

            }
            else
            {
                ViewBag.ErrorMessage2 = "Please select file";
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

        //---------------------------------------------------------------------------------------
       
        public ActionResult DeleteFile(int? id, bool? saveChangesError = false)
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
            pay_statement obj = db.pay_statement.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        [HttpPost, ActionName("DeleteFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(int id)
        {
            try
            {
                pay_statement obj = db.pay_statement.Find(id);
                db.pay_statement.Remove(obj);
                db.SaveChanges();
                return RedirectToAction("ManagePay");
            }
            catch (DataException)
            {

                return RedirectToAction("DeleteFile", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("ManagePay");
        }

        //----------------------------------------------

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
        public ActionResult DeleteDriverApplication(int? id, bool? saveChangesError = false)
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
            driver_application obj = db.driver_application.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        [HttpPost, ActionName("DeleteDriverApplication")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDriverApplication(int id)
        {
            try
            {
                driver_application obj = db.driver_application.Find(id);
                db.driver_application.Remove(obj);
                db.SaveChanges();
                return RedirectToAction("DriverApplication");
            }
            catch (DataException)
            {

                return RedirectToAction("DeleteDriverApplication", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("DriverApplication");
        }
        //--------------------------------driver application
        //Quote Request
        public ActionResult DisplayQuote(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            
            ViewBag.PickupDateSort = sortOrder == "pickup_date" ? "pickup_date_desc" : "pickup_date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var quoteList = from s in db.quotes
                               select s;

            switch (sortOrder)
            {

              
                case "pickup_date":
                    quoteList = quoteList.OrderBy(s => s.pickup_date);
                    break;
                case "pickup_date_desc":
                    quoteList = quoteList.OrderByDescending(s => s.pickup_date);
                    break;
                default:
                    quoteList = quoteList.OrderBy(s => s.id);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(quoteList.ToPagedList(pageNumber, pageSize));
            //return View(db.quotes.ToList());
        }
        public ActionResult QuoteDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            quote obj = db.quotes.Find(id);
            if (obj == null)

            {
                return HttpNotFound();
            }
            return View(obj);
        }
        public ActionResult DeleteQuote(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed.Try again.";
            }

            quote obj = db.quotes.Find(id);

            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        [HttpPost, ActionName("DeleteQuote")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQuote(int id)
        {
            try
            {

                quote obj = db.quotes.Find(id);
                db.quotes.Remove(obj);
                db.SaveChanges();
                return RedirectToAction("DisplayQuote");
            }
            catch (DataException)
            {

                return RedirectToAction("DeleteQuote", new { id = id, saveChangesError = true });
            }
            //return RedirectToAction("DisplayQuote");
        }
        //QuoteRequestDetails

        //----------------View Feedback--------------------
        public ActionResult ViewFeedback(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "added_on" ? "added_on_desc" : "added_on";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var feedbackList= from s in db.feedbacks
                              select s;

            switch (sortOrder)
            {
                
                case "added_on":
                    feedbackList = feedbackList.OrderBy(s => s.added_on);
                    break;
                case "added_on_desc":
                    feedbackList = feedbackList.OrderByDescending(s => s.added_on);
                    break;
                default:
                    feedbackList = feedbackList.OrderBy(s => s.id);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(feedbackList.ToPagedList(pageNumber, pageSize));
            // return View(db.table_driver.ToList());
        }

        public ActionResult FeedbackDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            feedback obj = db.feedbacks.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }

        //----------------View Feedback--------------------


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

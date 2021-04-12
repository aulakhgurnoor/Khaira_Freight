using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Khaira_Freight.Models;
using PagedList;

namespace Khaira_Freight.Controllers
{
    [Authorize]
    public class DispatchController : Controller
    {
        private KhairaFreightEntities db = new KhairaFreightEntities();

        // GET: Dispatch
        public ActionResult UserProfile()
        {
            int id = Convert.ToInt32(Session["EmpId"]);

            //TempData["Emp_Id"] = id;

            List<dispatch> dsipatcherList = db.dispatches.Where(x => x.dispatch_id.Equals(id)).ToList();

            return View(dsipatcherList);
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
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.MakeSortParm = String.IsNullOrEmpty(sortOrder) ? "make_asc" : "";
            ViewBag.YearSortParm = sortOrder == "year" ? "year_desc" : "year";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var truks = from s in db.trucks
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                truks = truks.Where(s => s.plate_number.Contains(searchString)
                                       || s.vin_number.Contains(searchString) ||
                                       s.make.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "make_asc":
                    truks = truks.OrderBy(s => s.make);

                    break;
                case "year":
                    truks = truks.OrderBy(s => s.year);
                    break;
                case "year_desc":
                    truks = truks.OrderByDescending(s => s.year);
                    break;
                default:
                    truks = truks.OrderBy(s => s.unit_number);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(truks.ToPagedList(pageNumber, pageSize));
           // return View(db.trucks.ToList());
        }
        
        // GET: Dispatch/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            truck truck = db.trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        // GET: Dispatch/Create
        public ActionResult Create()
        {
            truck truck = new truck();
            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Company" ,Value="PICK" },
                new SelectListItem() {Text="Owner Operator" ,Value="DROP" }

            };

            truck.statusList = statusList;
            return View(truck);
        }

        // POST: Dispatch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "unit_number,plate_number,registration,vin_number,make,year,weight,status,owner_company,driver,last_location,last_trip")] truck truck)
        {
            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Company" ,Value="PICK" },
                new SelectListItem() {Text="Owner Operator" ,Value="DROP" }

            };

            truck.statusList = statusList;
            if (ModelState.IsValid)
            {
                db.trucks.Add(truck);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(truck);
        }

        // GET: Dispatch/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            truck truck = db.trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        // POST: Dispatch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "unit_number,plate_number,registration,vin_number,make,year,weight,status,owner_company,driver,last_location,last_trip")] truck truck)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truck).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(truck);
        }

        // GET: Dispatch/Delete/5
    
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
            
            truck truck = db.trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                

                truck truck = db.trucks.Find(id);
                db.trucks.Remove(truck);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException)
            {

                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        //-------------------Weekly Planner--------------------------------------------------
        public ActionResult PlannerList(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.StartSortParm = String.IsNullOrEmpty(sortOrder) ? "start_date_asc" : "";
            ViewBag.DueSortParm = sortOrder == "due_date" ? "due_date_desc" : "due_date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var planers = from s in db.planners
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                planers = planers.Where(s => s.driver_name.Contains(searchString)
                                       || s.status.Contains(searchString)
                                       || s.trailer.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "start_date_asc":
                    planers = planers.OrderBy(s => s.start_date);

                    break;
                case "due_date":
                    planers = planers.OrderBy(s => s.due_date);
                    break;
                case "due_date_desc":
                    planers = planers.OrderByDescending(s => s.due_date);
                    break;
                default:
                    planers = planers.OrderBy(s => s.event_id);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(planers.ToPagedList(pageNumber, pageSize));
            //return View(db.planners.ToList());
        }
        

        // GET: Dispatch/Details/5
        public ActionResult PlannerDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planner planner = db.planners.Find(id);
            if (planner == null)
            {
                return HttpNotFound();
            }
            return View(planner);
        }
        //
       
        //

        public ActionResult AddPlanner()
        {
            planner planner = new planner();
            
            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Not Started" ,Value="Not Started" },
                new SelectListItem() {Text="In Progress" ,Value="In Progress" },
                new SelectListItem() {Text="Completed" ,Value="Completed" }
            };

            planner.statusList = statusList;

            List<SelectListItem> activityList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="PICK" ,Value="PICK" },
                new SelectListItem() {Text="DROP" ,Value="DROP" }
        
            };

            planner.activityList = activityList;

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

            planner.provinceList = provinceList;

            ViewBag.driverIdList = db.table_driver.Select(x => x.driver_id).ToList();


            return View(planner);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPlanner([Bind(Include = "event_id,status,trip,activity,driver_id,driver_name,city,province,truck,trailer,start_date,due_date")] planner planner)
        {
            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Not Started" ,Value="Not Started" },
                new SelectListItem() {Text="In Progress" ,Value="In Progress" },
                new SelectListItem() {Text="Completed" ,Value="Completed" }
            };

            planner.statusList = statusList;

            List<SelectListItem> activityList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="PICK" ,Value="PICK" },
                new SelectListItem() {Text="DROP" ,Value="DROP" }

            };

            planner.activityList = activityList;

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

            planner.provinceList = provinceList;

            ViewBag.driverIdList = db.table_driver.Select(x => x.driver_id).ToList();
            //

            if (planner.due_date != null && planner.start_date != null)
            {
                if (!(planner.due_date >= planner.start_date))
                {
                    ModelState.AddModelError("due_date", "Due date cannot be before start date");
                }
                if (!(planner.due_date >= DateTime.Now))
                {
                    ModelState.AddModelError("due_date", "Due date cannot be before current date");
                }


                if (!(planner.start_date >= DateTime.Now))
                {
                    ModelState.AddModelError("start_date", "Start date cannot be before current date");
                }
            }

            if(planner.driver_id!=null)
            {
                bool IsDriverExists = db.table_driver.Any(x => x.driver_id == planner.driver_id);
                if (IsDriverExists == false)
                {
                    ModelState.AddModelError("EmpId", "Invalid ! Driver does not exist in Database");
                }
            }

           
            //

            if (ModelState.IsValid)
            {

                db.planners.Add(planner);
                db.SaveChanges();
                return RedirectToAction("PlannerList");
            }

            return View(planner);
        }

        // GET: Dispatch/Edit/5
        public ActionResult EditPlanner(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planner planner = db.planners.Find(id);
            if (planner == null)
            {
                return HttpNotFound();
            }
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

            planner.provinceList = provinceList;

            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Not Started" ,Value="Not Started" },
                new SelectListItem() {Text="In Progress" ,Value="In Progress" },
                new SelectListItem() {Text="Completed" ,Value="Completed" }
            };

            planner.statusList = statusList;

            List<SelectListItem> activityList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="PICK" ,Value="PICK" },
                new SelectListItem() {Text="DROP" ,Value="DROP" }

            };

            planner.activityList = activityList;

            ViewBag.driverIdList = db.table_driver.Select(x => x.driver_id).ToList();

            return View(planner);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPlanner(planner planner)
        {
            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Not Started" ,Value="Not Started" },
                new SelectListItem() {Text="In Progress" ,Value="In Progress" },
                new SelectListItem() {Text="Completed" ,Value="Completed" }
            };

            planner.statusList = statusList;

            List<SelectListItem> activityList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="PICK" ,Value="PICK" },
                new SelectListItem() {Text="DROP" ,Value="DROP" }

            };

            planner.activityList = activityList;

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

            ViewBag.driverIdList = db.table_driver.Select(x => x.driver_id).ToList();

            planner.provinceList = provinceList;
            if (planner.due_date != null && planner.start_date != null)
            {
                if (!(planner.due_date >= planner.start_date))
                {
                    ModelState.AddModelError("due_date", "Due date cannot be before start date");
                }
               
            }
            if (ModelState.IsValid)
            {
                db.Entry(planner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PlannerList");
            }
            return View(planner);
        }

        // GET: Dispatch/Delete/5

        public ActionResult DeletePlanner(int? id, bool? saveChangesError = false)
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

            planner planner = db.planners.Find(id);
            if (planner == null)
            {
                return HttpNotFound();
            }
            return View(planner);
        }


        [HttpPost, ActionName("DeletePlanner")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePlanner(int id)
        {
            try
            {

                planner planner = db.planners.Find(id);
                db.planners.Remove(planner);
                db.SaveChanges();
                return RedirectToAction("PlannerList");
            }
            catch (DataException)
            {

                return RedirectToAction("DeletePlanner", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("PlannerList");
        }
        //-------------------Weekly Planner--------------------------------------------------

        //-------------------Full Calender--------------------------------------------------

        public ActionResult Calender()
        {
          
            return this.View();
        }

        public ActionResult GetCalendarData()
        {
            // Initialization.  
            JsonResult result = new JsonResult();

            try
            {
                // Loading.  
                List<planner> data = this.LoadData();

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
        private List<planner> LoadData()
        {

            // Initialization.  
            List<planner> plannerList = new List<planner>();
            try
            {
                //plannerList = db.planners.Where(x => x.driver_id.Equals(id)).ToList();

                plannerList = db.planners.ToList();
            }
            catch (Exception ex)
            {
                
                Console.Write(ex);
            }

           
            return plannerList;
        }

        //

        //-------------------Full Calender--------------------------------------------------

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

        private  List<pay_statement> GetPayFiles()
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

        public ActionResult EditProfile(int? id)
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
        public ActionResult EditProfile(dispatch obj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserProfile");
            }
            return View(obj);
        }


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
